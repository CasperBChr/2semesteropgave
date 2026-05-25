using _2SemesterOpgave.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _2SemesterOpgave.Services.Interfaces
{
    public interface IUserService
    {
        ObservableCollection<User> GetAllUsers();

        void CreateUser(User user);
    }
}