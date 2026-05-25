using System.Collections.ObjectModel;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave.Services.Interfaces;

namespace _2SemesterOpgave.Services
{
    public class UserServices : IUserService
    {
        IUserRepository _userRepository;

        public ObservableCollection<Conversation> Conversations;
        public ObservableCollection<User> Users;

        public UserServices(Database db)
        {
            _userRepository = new UserRepository(db);
            Conversations = new ObservableCollection<Conversation>();
            Users = GetAllUsers();
            Conversations.Add(new Conversation(new List<User> { Users[0], Users[1] }));
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

            _userRepository.AddUser(user);
        }
    }
}