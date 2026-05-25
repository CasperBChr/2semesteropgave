using _2SemesterOpgave.Models;
using System.Collections.Generic;

namespace _2SemesterOpgave.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();

        void CreateUser(User user);
    }
}