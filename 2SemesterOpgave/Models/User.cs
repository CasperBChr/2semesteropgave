using System.ComponentModel;

namespace _2SemesterOpgave.Models
{
    public class User : INotifyPropertyChanged
	{

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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Article> FavoriteArticles { get; set; } = new List<Article>();

		public event PropertyChangedEventHandler? PropertyChanged;

		private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int _followersCount = 0;
		public int FollowersCount
		{
			get => _followersCount;
			set { _followersCount = value; OnPropertyChanged(nameof(FollowersCount)); }
		}

		private int _followingCount = 0;
		public int FollowingCount
		{
			get => _followingCount;
			set { _followingCount = value; OnPropertyChanged(nameof(FollowingCount)); }
		}
    }
}
