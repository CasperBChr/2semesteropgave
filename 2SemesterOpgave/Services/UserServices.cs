using System.Collections.ObjectModel;
using _2SemesterOpgave.Algoritme;
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
		SessionContext _session;

		public User CurrentUser;
		public User? TargetUser;
		public UserProfile? UserProfile;
		public FakeConversation FakeConversation;
		public ObservableCollection<Conversation> Conversations;

		Dictionary<int, User> _cache = new Dictionary<int, User>();

		public UserServices(UserRepository userRepository, SessionContext session)
		{
			_userRepository = userRepository;
			_session = session;

			LoadCache();

			ObservableCollection<User> users = new ObservableCollection<User>(_cache.Values);
			CurrentUser = users[0];

			Conversations = new ObservableCollection<Conversation>();

			if (users.Count >= 2)
			{
				Conversations.Add(new Conversation(new List<User> { users[0], users[1] }));

				FakeConversation = new FakeConversation();
				FakeConversation.ContinueConversationBot(Conversations[0], Conversations[0].Participants[1]);

				AddFollower(users[0], users[1]);
				AddFollower(users[1], users[0]);

				Conversations[0].Messages.Add(new Message("Suuup", Conversations[0], Conversations[0].Participants[1], DateTime.Now));
				Conversations[0].Messages.Add(new Message("Heeeeeeeeeey", Conversations[0], Conversations[0].Participants[0], DateTime.Now));
			}
			else
			{
				throw new Exception("Skal bruge mindst 2 users i DB");
			}
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
			return _cache.TryGetValue(id, out var user) ? user : null;
		}

		public ObservableCollection<User> GetAllUsers()
		{
			return new ObservableCollection<User>(_cache.Values);
		}

		public void AddFollower(User follower, User following)
		{
			if (follower.Id == following.Id) return;
			if (_userRepository.IsFollowing(follower.Id, following.Id)) return;
			_userRepository.AddFollower(follower, following);
		}

		public void RemoveFollower(User follower, User following)
		{
			_userRepository.RemoveFollower(follower, following);
		}

		public User? GetUserById(int id)
		{
			UserDTO? dto = _userRepository.GetUserByID(id);
			if (dto == null) return null;
			return Map(dto);
		}

		public void CreateUser(User user)
		{
			if (string.IsNullOrWhiteSpace(user.Username))
			{
				throw new Exception("Username is required");
			}

			if (string.IsNullOrWhiteSpace(user.Email))
			{
				throw new Exception("Email is required");
			}

			if (string.IsNullOrWhiteSpace(user.Password))
			{
				throw new Exception("Password is required");
			}

			if (_userRepository.GetUserByUsername(user.Username) != null)
			{
				throw new Exception("Username already exists");
			}

			//_userRepository.CreateUser(user);
			int newId = _userRepository.CreateUser(user);
			user.Id = newId;
		}

		//public void CreateUser(User user)
		//{
		//	if (string.IsNullOrWhiteSpace(user.Email))
		//		throw new Exception("Email is required");
		//	_userRepository.CreateUser(user);
		//}

		public void UpdateUser(User user)
		{
			_userRepository.UpdateUser(user);
		}

		public Message CreateMessage(string text, Conversation conversation, User sender)
		{
			return new Message(text, conversation, sender, DateTime.Now);
		}

		public User? Login(string username, string password)
		{
			UserDTO? dto = _userRepository.GetUserByUsername(username);
			if (dto == null) return null;

			User user = Map(dto);
			if (user.Password != password) return null;

			this.CurrentUser = user;
			return user;
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