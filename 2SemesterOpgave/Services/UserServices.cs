using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave.Services.Interfaces;
using _2SemesterOpgave.Utils;
using System.Collections.ObjectModel;

namespace _2SemesterOpgave.Services
{
    public class UserServices : IUserService
    {
        IUserRepository _userRepository;

        public ObservableCollection<Conversation> Conversations;
        public ObservableCollection<User> Users;
        public FakeConversation FakeConversation;
        public User CurrentUser;
        public User? TargetUser;
        public UserServices(Database db)
        {
            _userRepository = new UserRepository(db);
            Conversations = new ObservableCollection<Conversation>();
            Users = GetAllUsers();
            CurrentUser = Users[0];

            Conversations.Add(new Conversation(new List<User> { Users[0], Users[1] }));

            FakeConversation = new FakeConversation();
            FakeConversation.ContinueConversationBot(Conversations[0], Conversations[0].Participants[1]);

            Conversations[0].Messages.Add(new Message("Suuup", Conversations[0], Conversations[0].Participants[1], DateTime.Now));
            Conversations[0].Messages.Add(new Message("Heeeeeeeeeey", Conversations[0], Conversations[0].Participants[0], DateTime.Now));
        }

		public ObservableCollection<User> GetAllUsers()
		{
			IEnumerable<User> users = _userRepository.GetAllUsers();
			ObservableCollection<User> uiUsers = new ObservableCollection<User>();
			foreach (User user in users)
			{
				uiUsers.Add(user);
			}
			return uiUsers;
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


    }
}