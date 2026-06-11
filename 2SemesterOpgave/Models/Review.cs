namespace _2SemesterOpgave.Models
{
    //Klasse for Review
    public class Review
    {
        //Property for Review
        // Reviewets id
        public int Id { get; set; }

        // Rating/score
        public int Rating { get; set; }

        // Kommentar
        public string Comment { get; set; } = string.Empty;

        // Oprettelsesdato
        public DateTime CreatedAt { get; set; }

        // Id på lejeaftale
        public int? RentalId { get; set; }

        // Backup-id'er hvis User-objekterne ikke er sat
        private int _reviewerId;
        private int _revieweeId;

        // Brugeren der skriver reviewet
        public User Reviewer { get; set; }

        // Brugeren der modtager reviewet
        public User Reviewee { get; set; }

        // Id på reviewer
        public int ReviewerId
        {
            get => Reviewer?.Id ?? _reviewerId;
            set => _reviewerId = value;
        }

        // Id på reviewee
        public int RevieweeId
        {
            get => Reviewee?.Id ?? _revieweeId;
            set => _revieweeId = value;
        }

        // Reviewers brugernavn
        public string ReviewerUsername => Reviewer?.Username ?? string.Empty;

        // Reviewees brugernavn
        public string RevieweeUsername => Reviewee?.Username ?? string.Empty;
    }
}