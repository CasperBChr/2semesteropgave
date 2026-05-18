using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class User : IReferrer
    {
        public void ReceiveNotification(string message)
        {
            Console.WriteLine($"Notification for {Username}: {message}");
        }

        //Properties 
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public bool IsVerified { get; set; }
        public float RatingScore { get; set; }
        public DateTime SignupTime { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }

        public User()
        {
            IsVerified = false;
            RatingScore = 0;
            SignupTime = DateTime.Now;

            Followers = new List<User>();
            Following = new List<User>();
        }

        //Contructor 
        public User(
        string username,
        string email,
        string password)
        {
            Username = username;
            Email = email;
            Password = password;

            FirstName = string.Empty;
            LastName = string.Empty;

            IsVerified = false;
            RatingScore = 0;

            PhoneNumber = string.Empty;
            City = string.Empty;
            Description = string.Empty;

            SignupTime = DateTime.Now;
        }
    }
}
