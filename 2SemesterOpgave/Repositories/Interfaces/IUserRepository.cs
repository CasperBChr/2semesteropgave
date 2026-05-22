using _2SemesterOpgave.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByID(int id);

        List<User> GetAllUsers();

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);
    }
}
