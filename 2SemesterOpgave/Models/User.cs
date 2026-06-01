using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class User : IReferrer // Implementerer IReferrer for at kunne modtage notifikationer, når der sker relevante begivenheder, såsom nye beskeder eller lejeaftaler
    {
        public void ReceiveNotification(string message) // Implementering af ReceiveNotification-metoden fra IReferrer, som udskriver notifikationen til konsollen med brugerens navn
        {
            Console.WriteLine($"Notification for {Username}: {message}"); // Udskriver notifikationen til konsollen, inklusive brugerens navn for at gøre det klart, hvem notifikationen er til
        }

        //Properties 
        public int Id { get; set; } // Property: gemmer brugerens unikke ID som et heltal
        public string Username { get; set; } // Property: gemmer brugernavnet som tekst
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

        // Antal følgere/følger, som kommer direkte fra databasen
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }

        public User() // Default constructor: initialiserer en ny instans af User-klassen, hvor IsVerified sættes til false, RatingScore sættes til 0, SignupTime sættes til det aktuelle tidspunkt, og Followers og Following sættes til tomme lister
        {
            IsVerified = false;
            RatingScore = 0;
            SignupTime = DateTime.Now;

            Followers = new List<User>();
            Following = new List<User>();


            // Standardværdier for tællere, hvis de ikke findes i databasen
            FollowersCount = 0;
            FollowingCount = 0;
        }

        //Contructor 
        public User(
        string username,
        string email,
        string password,
        int id)
        {
            Username = username;
            Email = email;
            Password = password;
            Id = id;

            FirstName = string.Empty;
            LastName = string.Empty;

            IsVerified = false;
            RatingScore = 0;

            PhoneNumber = string.Empty;
            City = string.Empty;
            Description = string.Empty;

            SignupTime = DateTime.Now;

            Followers = new List<User>();
            Following = new List<User>();

            FollowersCount = 0;
            FollowingCount = 0;
        }
    }
}
