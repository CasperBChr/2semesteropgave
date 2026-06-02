using System.Collections.ObjectModel;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave.Services.Interfaces;
using _2SemesterOpgave.Utils;

namespace _2SemesterOpgave.Services
{
    public class UserServices
    {
        UserRepository _userRepository;

        public ObservableCollection<Conversation> Conversations;
        public ObservableCollection<User> Users;
        public FakeConversation FakeConversation;
        public User CurrentUser;
        public User? TargetUser;
		Dictionary<int, User> _cache = new Dictionary<int, User>();
		//public UserServices(UserRepository userRepository)
		//      {
		//          _userRepository = userRepository;
		//          Conversations = new ObservableCollection<Conversation>();
		//          Users = GetAllUsers();
		//	LoadCache();

		//	Conversations.Add(new Conversation(new List<User> { Users[0], Users[1] }));

		//          FakeConversation = new FakeConversation();
		//          FakeConversation.ContinueConversationBot(Conversations[0], Conversations[0].Participants[1]);

		//          this.AddFollower(Users[0], Users[1]);
		//          CurrentUser = Users[0];

		//          Conversations[0].Messages.Add(new Message("Suuup", Conversations[0], Conversations[0].Participants[1], DateTime.Now));
		//          Conversations[0].Messages.Add(new Message("Heeeeeeeeeey", Conversations[0], Conversations[0].Participants[0], DateTime.Now));
		//      }

		public UserServices(UserRepository userRepository)
		{
			_userRepository = userRepository;

			Conversations = new ObservableCollection<Conversation>();

			LoadCache(); // 1. FILL CACHE FIRST

			Users = new ObservableCollection<User>(_cache.Values); // 2. NOW SAFE

			if (Users.Count < 2) 
			{
				throw new Exception("Skal bruge mindst to 2 users i DB");
			}

			CurrentUser = Users[0];

			Conversations.Add(new Conversation(new List<User> { Users[0], Users[1] }));

			FakeConversation = new FakeConversation();
			FakeConversation.ContinueConversationBot(Conversations[0], Conversations[0].Participants[1]);

			AddFollower(Users[0], Users[1]);

			Conversations[0].Messages.Add(new Message("Suuup", Conversations[0], Conversations[0].Participants[1], DateTime.Now));
			//Conversations[0].Messages.Add(new Message("Heeeeeeeeeey", Conversations[0].Participants[0], DateTime.Now));
			Conversations[0].Messages.Add(new Message("Heeeeeeeeeey", Conversations[0], Conversations[0].Participants[0], DateTime.Now));
		}

		void LoadCache()
		{
			foreach (UserDTO dto in _userRepository.GetAllUsers())
			{
				_cache[dto.Id] = Map(dto);
			}
		}

		public User? GetById(int id)
		{
			return _cache.TryGetValue(id, out var user)
				? user
				: null;
		}

		public ObservableCollection<User> GetAllUsers()
		{
			return new ObservableCollection<User>(_cache.Values);
		}

		//public ObservableCollection<User> GetAllUsers()
		//{
		//	IEnumerable<User> users = _userRepository.GetAllUsers();
		//	ObservableCollection<User> uiUsers = new ObservableCollection<User>();
		//	foreach (User user in users)
		//	{
		//		uiUsers.Add(user);
		//	}
		//	return uiUsers;
		//}

		public void AddFollower(User follower, User following)
        {
			if (follower.Id == following.Id) 
            {
                return;
            }

			if (_userRepository.IsFollowing(follower.Id, following.Id)) 
            {
                return;
            }

			_userRepository.AddFollower(follower, following);
        }

        public void RemoveFollower(User follower, User following) 
        {
			_userRepository.RemoveFollower(follower, following);
		}

		//public User? GetUserById(int id) 
		//{
		//    return _userRepository.GetUserByID(id);
		//}

		public User? GetUserById(int id)
		{
			UserDTO? dto = _userRepository.GetUserByID(id);
			if (dto == null)
			{
				return null;
			}
			return Map(dto);
		}

		public void CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new Exception("Email is required");
            }

            _userRepository.CreateUser(user);
        }

        // Gem funktion til MyAccount siden, så den kan opdatere både i databasen og i UI'et
        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public Message CreateMessage(string text, Conversation conversation, User sender)
        {
            return new Message(text, conversation, sender, DateTime.Now);
        }

		User Map(UserDTO dto)
		{
			return new User
			{
				Id = dto.Id,
				Username = dto.Username,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = dto.Email,
				Password = dto.Password,
				City = dto.City,
				ProfilePicture = dto.ProfilePicture,
				PhoneNumber = dto.PhoneNumber,
				Description = dto.Description,
				IsVerified = dto.IsVerified,
				RatingScore = dto.RatingScore,
				CreatedAt = dto.CreatedAt,
				UpdatedAt = dto.UpdatedAt,
				FollowersCount = dto.FollowersCount,
				FollowingCount = dto.FollowingCount
			};
		}
	}
}