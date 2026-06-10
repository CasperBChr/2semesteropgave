using System.Collections.ObjectModel;
using _2SemesterOpgave.Algoritme; // Giver adgang til algoritme-klasser, fx FakeConversation og UserProfile
using _2SemesterOpgave.Data; // Giver adgang til data-laget
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User, Message og Conversation
using _2SemesterOpgave.Repositories; // Giver adgang til UserRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til UserDTO
using _2SemesterOpgave.Utils; // Giver adgang til SessionContext

namespace _2SemesterOpgave.Services
{
    // Serviceklasse der håndterer logik for brugere
    public class UserServices
    {
        // Repository der bruges til databasekald for brugere
        UserRepository _userRepository;

        // SessionContext bruges til at holde styr på sessionen
        SessionContext _session;

        // Gemmer den nuværende bruger
        public User CurrentUser;

        // Gemmer en bruger som fx kan vises på en profilside
        public User? TargetUser;

        // Gemmer brugerprofil-data
        public UserProfile? UserProfile;

        // Bruges til fake samtaler/bots
        public FakeConversation FakeConversation;

        // Liste med brugerens samtaler
        public ObservableCollection<Conversation> Conversations;

        // Cache der gemmer brugere ud fra deres id
        Dictionary<int, User> _cache = new Dictionary<int, User>();

        // Constructor der modtager UserRepository og SessionContext
        public UserServices(UserRepository userRepository, SessionContext session)
        {
            // Gemmer UserRepository, så den kan bruges i metoderne
            _userRepository = userRepository;

            // Gemmer sessionen
            _session = session;

            // Indlæser brugere fra databasen til cache
            LoadCache();

            // Opretter en liste med alle brugere fra cache
            ObservableCollection<User> users = new ObservableCollection<User>(_cache.Values);

            // Opretter en tom liste til samtaler
            Conversations = new ObservableCollection<Conversation>();

            // Opretter FakeConversation-objektet
            FakeConversation = new FakeConversation();

            // Tjekker om der findes mindst 2 brugere i databasen
            if (users.Count < 2)
            {
                // Kaster en fejl hvis der ikke er nok brugere
                throw new Exception("Skal bruge mindst 2 users i DB");
            }
        }

        // Starter fake bots til samtaler
        public void StartFakeBots(ConversationServices conversationServices)
        {
            // Opretter en liste med alle brugere fra cache
            List<User> allUsers = new List<User>(_cache.Values);

            // Starter fake bots med alle brugere, nuværende bruger og ConversationServices
            FakeConversation.StartFakeBots(allUsers, CurrentUser, conversationServices);

            // Lytter efter nye beskeder fra fake conversations
            FakeConversation.OnNewMessage += (conversation) =>
            {
                // Låser FakeConversation, så listen ikke ændres af flere tråde samtidig
                lock (FakeConversation)
                {
                    // Variabel der holder styr på om samtalen allerede findes
                    bool exists = false;

                    // Gennemgår alle samtaler
                    for (int i = 0; i < Conversations.Count; i++)
                    {
                        // Tjekker om samtalen allerede findes i listen
                        if (Conversations[i].Id == conversation.Id)
                        {
                            // Markerer at samtalen findes
                            exists = true;

                            // Stopper loopet
                            break;
                        }
                    }

                    // Tjekker om samtalen ikke allerede findes
                    if (!exists)
                    {
                        // Tilføjer samtalen til listen
                        Conversations.Add(conversation);
                    }
                }
            };
        }

        // Indlæser alle brugere fra databasen til cache
        void LoadCache()
        {
            // Gennemgår alle UserDTO'er fra repository
            foreach (UserDTO dto in _userRepository.GetAllUsers())
            {
                // Mapper DTO'en til User og gemmer den i cache med id som nøgle
                _cache[dto.Id] = Map(dto);
            }
        }

        // Tjekker om en bruger følger en anden bruger
        public bool IsFollowing(User follower, User following)
        {
            // Returnerer resultatet fra repository
            return _userRepository.IsFollowing(follower.Id, following.Id);
        }

        // Henter en bruger ud fra id
        public User? GetById(int id)
        {
            // Prøver at finde brugeren i cache og returnerer den, ellers null
            return _cache.TryGetValue(id, out var user) ? user : null;
        }

        // Henter alle brugere
        public ObservableCollection<User> GetAllUsers()
        {
            // Returnerer alle brugere fra cache som en ObservableCollection
            return new ObservableCollection<User>(_cache.Values);
        }

        // Tilføjer en follower-relation mellem to brugere
        public void AddFollower(User follower, User following)
        {
            // Stopper hvis brugeren prøver at følge sig selv
            if (follower.Id == following.Id)
            {
                return;
            }

            // Stopper hvis brugeren allerede følger den anden bruger
            if (_userRepository.IsFollowing(follower.Id, following.Id))
            {
                return;
            }

            // Tilføjer follower-relationen i databasen
            _userRepository.AddFollower(follower, following);
        }

        // Fjerner en follower-relation mellem to brugere
        public void RemoveFollower(User follower, User following)
        {
            // Fjerner follower-relationen i databasen
            _userRepository.RemoveFollower(follower, following);
        }

        // Henter en bruger direkte fra repository ud fra id
        public User? GetUserById(int id)
        {
            // Henter UserDTO fra repository
            UserDTO? dto = _userRepository.GetUserByID(id);

            // Returnerer null hvis brugeren ikke findes
            if (dto == null)
            {
                return null;
            }

            // Mapper DTO'en til en User og returnerer den
            return Map(dto);
        }

        // Opretter en ny bruger
        public void CreateUser(User user)
        {
            // Tjekker om brugernavn mangler
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                // Kaster en fejl hvis brugernavn mangler
                throw new Exception("Username is required");
            }

            // Tjekker om email mangler
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                // Kaster en fejl hvis email mangler
                throw new Exception("Email is required");
            }

            // Tjekker om adgangskode mangler
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                // Kaster en fejl hvis adgangskode mangler
                throw new Exception("Password is required");
            }

            // Tjekker om brugernavnet allerede findes
            if (_userRepository.GetUserByUsername(user.Username) != null)
            {
                // Kaster en fejl hvis brugernavnet allerede er taget
                throw new Exception("Username already exists");
            }

            // Opretter brugeren i databasen og henter det nye id
            int newId = _userRepository.CreateUser(user);

            // Sætter brugerens nye id
            user.Id = newId;
        }

        // Opdaterer en bruger
        public void UpdateUser(User user)
        {
            // Sender brugeren videre til repository, som opdaterer databasen
            _userRepository.UpdateUser(user);
        }

        // Opretter en ny Message-model
        public Message CreateMessage(string text, Conversation conversation, User sender)
        {
            // Returnerer en ny besked med tekst, samtale, afsender og tidspunkt
            return new Message(text, conversation, sender, DateTime.Now);
        }

        // Logger en bruger ind
        public User? Login(string username, string password)
        {
            // Henter brugerens DTO ud fra brugernavn
            UserDTO? dto = _userRepository.GetUserByUsername(username);

            // Returnerer null hvis brugeren ikke findes
            if (dto == null) return null;

            // Mapper DTO'en til en User-model
            User user = Map(dto);

            // Returnerer null hvis adgangskoden ikke matcher
            if (user.Password != password) return null;

            // Gemmer brugeren som nuværende bruger
            this.CurrentUser = user;

            // Returnerer den indloggede bruger
            return user;
        }

        // Mapper en UserDTO til en User-model
        User Map(UserDTO dto)
        {
            // Opretter og returnerer en User med data fra DTO'en
            return new User
            {
                // Sætter brugerens id
                Id = dto.Id,

                // Sætter brugernavn
                Username = dto.Username,

                // Sætter fornavn
                FirstName = dto.FirstName,

                // Sætter efternavn
                LastName = dto.LastName,

                // Sætter email
                Email = dto.Email,

                // Sætter adgangskode
                Password = dto.Password,

                // Sætter by
                City = dto.City,

                // Sætter profilbillede
                ProfilePicture = dto.ProfilePicture,

                // Sætter telefonnummer
                PhoneNumber = dto.PhoneNumber,

                // Sætter profilbeskrivelse
                Description = dto.Description,

                // Sætter om brugeren er verificeret
                IsVerified = dto.IsVerified,

                // Sætter hvornår brugeren blev oprettet
                CreatedAt = dto.CreatedAt,

                // Sætter hvornår brugeren sidst blev opdateret
                UpdatedAt = dto.UpdatedAt,

                // Henter og sætter antal followers
                FollowersCount = _userRepository.GetUserFollowerCount(dto.Id),

                // Henter og sætter antal brugere som brugeren følger
                FollowingCount = _userRepository.GetUserFollowingCount(dto.Id)
            };
        }
    }
}