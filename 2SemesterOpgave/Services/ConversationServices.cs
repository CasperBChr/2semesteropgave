using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Conversation, Message og User
using _2SemesterOpgave.Repositories; // Giver adgang til ConversationRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ConversationDTO og MessageDTO

namespace _2SemesterOpgave.Services
{
    // Serviceklasse der håndterer logik for samtaler og beskeder
    public class ConversationServices
    {
        // Repository der bruges til databasekald for samtaler og beskeder
        private readonly ConversationRepository _conversationRepository;

        // Service der bruges til at hente brugere
        private readonly UserServices _userServices;

        // Cache sikrer samme instance bruges i hele UI
        // Dictionary der gemmer samtaler, så samme samtale-objekt kan genbruges
        private readonly Dictionary<int, Conversation> _conversationCache = new();

        // Track read state pr user + conversation
        // Dictionary der gemmer hvornår en samtale sidst blev læst
        private readonly Dictionary<int, DateTime> _lastReadAt = new();

        // Gemmer den nuværende samtale
        public Conversation CurrentConversation { get; set; }

        // Constructor der modtager repository og user service
        public ConversationServices(
            ConversationRepository conversationRepository,
            UserServices userServices)
        {
            // Gemmer ConversationRepository, så den kan bruges i metoderne
            _conversationRepository = conversationRepository;

            // Gemmer UserServices, så brugere kan hentes
            _userServices = userServices;
        }

        // Henter en eksisterende samtale mellem to brugere eller opretter en ny
        public Conversation GetOrCreateConversation(User userA, User userB)
        {
            // Prøver at hente en eksisterende samtale mellem de to brugere
            ConversationDTO? existing =
                _conversationRepository.GetConversationBetween(userA.Id, userB.Id);

            // Hvis samtalen findes, bliver den mappet og returneret
            if (existing != null)
                return Map(existing);

            // Opretter en ny samtale og gemmer dens id
            int conversationId = _conversationRepository.CreateConversation();

            // Tilføjer den første bruger som deltager
            _conversationRepository.AddParticipant(conversationId, userA.Id);

            // Tilføjer den anden bruger som deltager
            _conversationRepository.AddParticipant(conversationId, userB.Id);

            // Henter den nyoprettede samtale fra databasen
            ConversationDTO? created =
                _conversationRepository.GetConversationById(conversationId);

            // Mapper og returnerer den oprettede samtale
            return Map(created!);
        }


        // Henter alle samtaler for en bestemt bruger
        public ObservableCollection<Conversation> GetConversationsForUser(User user)
        {
            // Henter samtale-DTO'er fra repository ud fra brugerens id
            IEnumerable<ConversationDTO> dtos =
                _conversationRepository.GetConversationsByUserId(user.Id);

            // Opretter en ObservableCollection, så UI kan vise samtalerne
            ObservableCollection<Conversation> conversations = new();

            // Gennemgår alle DTO'er
            foreach (ConversationDTO dto in dtos)
            {
                // Mapper DTO'en til en Conversation og tilføjer den til listen
                conversations.Add(Map(dto));
            }

            // Returnerer samtalerne
            return conversations;
        }

        // Sender en besked i en samtale
        public void SendMessage(Conversation conversation, User sender, string text)
        {
            // Stopper metoden hvis beskedteksten er tom
            if (string.IsNullOrWhiteSpace(text))
                return;

            // Opretter en MessageDTO med beskedens data
            MessageDTO dto = new()
            {
                // Sætter beskedens tekst
                Text = text,

                // Sætter id på brugeren der sender beskeden
                SenderId = sender.Id,

                // Sætter id på samtalen beskeden hører til
                ConversationId = conversation.Id
            };

            // Gemmer beskeden i databasen
            _conversationRepository.AddMessage(dto);

            // Opretter en Message-model til UI'et
            Message message = new(
                text,
                conversation,
                sender,
                DateTime.Now);

            // Tilføjer beskeden til samtalens beskedliste
            conversation.Messages.Add(message);

            // Opdaterer hvornår samtalen sidst var aktiv
            conversation.LastActive = DateTime.Now;
        }

        // Henter antal samtaler med ulæste beskeder for en bruger
        public int GetUnreadConversationCount(User user)
        {
            // Tæller hvor mange samtaler der har ulæste beskeder
            int count = 0;

            // Gennemgår alle brugerens samtaler
            foreach (Conversation conv in _userServices.Conversations)
            {
                // Finder den seneste besked i samtalen, som ikke er sendt af brugeren selv
                var lastOther = conv.Messages
                    .LastOrDefault(m => m.Sender.Id != user.Id);

                // Springer samtalen over hvis der ikke findes beskeder fra andre
                if (lastOther == null)
                    continue;

                // Henter tidspunktet for hvornår samtalen sidst blev læst
                DateTime lastRead = GetLastReadTime(conv.Id, user);

                // Tjekker om den seneste besked er nyere end sidste læsetidspunkt
                if (lastOther.Timestamp > lastRead)
                    count++;
            }

            // Returnerer antal ulæste samtaler
            return count;
        }

        // Marker en samtale som læst for en bruger
        public void MarkConversationAsRead(Conversation conversation, User user)
        {
            // Gemmer tidspunktet for hvornår samtalen blev læst
            _lastReadAt[conversation.Id] = DateTime.Now;

            // Tjekker om samtalen har et gyldigt id
            if (conversation.Id > 0)
            {
                // Gemmer læsestatus i databasen
                _conversationRepository.MarkAsRead(user.Id, conversation.Id);
            }
        }

        // Henter tidspunktet for hvornår en samtale sidst blev læst
        public DateTime GetLastReadTime(int conversationId, User user)
        {
            // Tjekker om samtalen findes i læse-cache
            if (_lastReadAt.TryGetValue(conversationId, out DateTime time))
                return time;

            // Returnerer minimumsdato hvis samtalen ikke er markeret som læst
            return DateTime.MinValue;
        }

        // Mapper en ConversationDTO til en Conversation-model
        private Conversation Map(ConversationDTO dto)
        {
            // Tjekker om samtalen allerede findes i cache
            if (_conversationCache.TryGetValue(dto.Id, out Conversation cached))
                return cached;

            // Henter alle deltagere ud fra deres id'er
            List<User> participants = dto.ParticipantIds
                .Select(id => _userServices.GetById(id))
                .Where(u => u != null)
                .Select(u => u!)
                .ToList();

            // Opretter en beskedliste til samtalen
            ObservableCollection<Message> messages = new();

            // Opretter en Conversation-model med data fra DTO'en
            Conversation conversation = new()
            {
                // Sætter samtalens id
                Id = dto.Id,

                // Sætter hvornår samtalen blev oprettet
                CreationTime = dto.CreatedAt,

                // Sætter sidste aktivitet til oprettelsestidspunktet som standard
                LastActive = dto.CreatedAt,

                // Sætter samtalens deltagere
                Participants = participants,

                // Sætter samtalens beskeder
                Messages = messages
            };

            // Gennemgår alle besked-DTO'er i samtalen
            foreach (MessageDTO m in dto.Messages)
            {
                // Henter afsenderen ud fra beskedens SenderId
                User? sender = _userServices.GetById(m.SenderId);

                // Springer beskeden over hvis afsenderen ikke findes
                if (sender == null)
                    continue;

                // Opretter en Message-model og tilføjer den til samtalen
                messages.Add(new Message(
                    m.Text,
                    conversation,
                    sender,
                    m.CreatedAt));
            }

            // Gemmer samtalen i cache
            _conversationCache[dto.Id] = conversation;

            // Returnerer den færdige Conversation-model
            return conversation;
        }
    }
}