using System.ComponentModel;

namespace _2SemesterOpgave.Models
{
	// Modelklasse for en bruger
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class User : INotifyPropertyChanged
    {
        // Brugerens id
        public int Id { get; set; }

        // Brugerens login-navn
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Brugerens favoritartikler
        public List<Article> FavoriteArticles { get; set; } = new List<Article>();

        // Bruges til at fortælle UI'et at en property er ændret
        public event PropertyChangedEventHandler? PropertyChanged;

        // Kalder PropertyChanged-eventet
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int _followersCount = 0;

        // Antal følgere
        public int FollowersCount
        {
            get => _followersCount;
            set { _followersCount = value; OnPropertyChanged(nameof(FollowersCount)); }
        }

        private int _followingCount = 0;

        // Antal brugere som brugeren følger
        public int FollowingCount
        {
            get => _followingCount;
            set { _followingCount = value; OnPropertyChanged(nameof(FollowingCount)); }
        }
    }
}