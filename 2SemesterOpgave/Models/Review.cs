using System;

namespace _2SemesterOpgave.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? RentalId { get; set; }

        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }

        // Bruges til visning i UI'et
        public string ReviewerUsername { get; set; }
        public string RevieweeUsername { get; set; }

        public Review()
        {
            Comment = string.Empty;
            ReviewerUsername = string.Empty;
            RevieweeUsername = string.Empty;
            CreatedAt = DateTime.Now;
        }
    }
}