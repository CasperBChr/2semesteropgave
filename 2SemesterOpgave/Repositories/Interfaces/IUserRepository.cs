using _2SemesterOpgave.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByID(int id);

        IEnumerable<User> GetAllUsers();

        void CreateUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);
    }
}
