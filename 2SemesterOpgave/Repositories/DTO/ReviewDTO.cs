using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
    // DTO-klasse der bruges til at transportere review-data fra databasen
    public class ReviewDTO
    {
        // Reviewets id i databasen
        public int Id { get; set; }

        // Ratingen som brugeren har givet
        public int Rating { get; set; }

        // Kommentaren til reviewet
        public string Comment { get; set; } = string.Empty;

        // Dato og tidspunkt for hvornår reviewet blev oprettet
        public DateTime CreatedAt { get; set; }

        // Id på lejeaftalen som reviewet eventuelt hører til
        public int? RentalId { get; set; }

        // Id på brugeren der har skrevet reviewet
        public int ReviewerId { get; set; }

        // Id på brugeren der er blevet vurderet
        public int RevieweeId { get; set; }
    }
}
