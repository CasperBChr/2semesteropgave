namespace _2SemesterOpgave.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? RentalId { get; set; }

		private int _reviewerId;
		private int _revieweeId;

		public User Reviewer { get; set; }
		public User Reviewee { get; set; }

		public int ReviewerId
		{
			get => Reviewer?.Id ?? _reviewerId;
			set => _reviewerId = value;
		}

		public int RevieweeId
		{
			get => Reviewee?.Id ?? _revieweeId;
			set => _revieweeId = value;
		}
		public string ReviewerUsername => Reviewer?.Username ?? string.Empty;
		public string RevieweeUsername => Reviewee?.Username ?? string.Empty;
    }
}
