using System; 
using System.Collections.Generic; 
using System.Collections.ObjectModel; 
using System.Data; 
using System.Data.Common; 
using _2SemesterOpgave.Data; // Giver adgang til Database-klassen
using _2SemesterOpgave.Models; // Giver adgang til Review-modellen
using _2SemesterOpgave.Repositories.Interfaces; // Giver adgang til IReviewRepository-interfacet
using Microsoft.Data.Sqlite; // Giver adgang til SqliteConnection

namespace _2SemesterOpgave.Repositories
{
    public class ReviewRepository : IReviewRepository // Klassen håndterer databasekald for reviews og implementerer IReviewRepository
    {
        Database _db; // Gemmer database-objektet, så repository kan oprette forbindelse til databasen

        public ReviewRepository(Database db) // Constructor der modtager database-objektet
        {
            _db = db; // Gemmer database-objektet i feltet _db
        }

        private static Review MapReview(DbDataReader reader) // Metode der omdanner en række fra databasen til et Review-objekt
        {
            int rentalIdOrdinal = reader.GetOrdinal("rental_id"); // Finder placeringen af kolonnen rental_id
            int createdAtOrdinal = reader.GetOrdinal("created_at"); // Finder placeringen af kolonnen created_at
            int reviewerIdOrdinal = reader.GetOrdinal("reviewer_id"); // Finder placeringen af kolonnen reviewer_id
            int revieweeIdOrdinal = reader.GetOrdinal("reviewee_id"); // Finder placeringen af kolonnen reviewee_id
            string comment = string.Empty; // Starter med en tom kommentar, hvis databasen ikke har en kommentar

            if (!reader.IsDBNull(reader.GetOrdinal("comment"))) // Tjekker om comment-kolonnen ikke er tom/null
            {
                comment = reader.GetString(reader.GetOrdinal("comment")); // Henter kommentaren fra databasen
            }

            Review review = new Review // Opretter et nyt Review-objekt
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")), // Henter reviewets id fra databasen
                Rating = reader.GetInt32(reader.GetOrdinal("rating")), // Henter ratingen fra databasen
                Comment = comment, // Sætter kommentaren på reviewet
                ReviewerId = reader.GetInt32(reviewerIdOrdinal), // Henter id på brugeren der har skrevet reviewet
                RevieweeId = reader.GetInt32(revieweeIdOrdinal) // Henter id på brugeren der bliver vurderet
            };

            review.RentalId = reader.GetInt32(rentalIdOrdinal); // Henter id på den lejeaftale reviewet hører til
            review.CreatedAt = DateTime.ParseExact(reader.GetString(createdAtOrdinal), "yyyy-MM-dd HH:mm:ss", null); // Konverterer datoen fra tekst til DateTime

            return review; // Returnerer det færdige Review-objekt
        }

        public IEnumerable<Review> GetAll() // Henter alle reviews fra databasen
        {
            using SqliteConnection connection = _db.CreateConnection(); // Opretter en forbindelse til databasen
            using DbCommand command = connection.CreateCommand(); // Opretter en SQL-kommando på forbindelsen

            command.CommandText = @" 
                    SELECT r.id, r.rating, r.comment, r.created_at, r.rental_id, r.reviewer_id, r.reviewee_id,
                    reviewer.Username AS ReviewerUsername,
                    reviewee.Username AS RevieweeUsername
                    FROM Reviews r
                    LEFT JOIN Users reviewer ON reviewer.ID = r.reviewer_id
                    LEFT JOIN Users reviewee ON reviewee.ID = r.reviewee_id
                    ORDER BY r.created_at DESC"; // SQL der henter alle reviews og sorterer de nyeste først

            List<Review> reviews = new List<Review>(); // Opretter en liste til de reviews der bliver hentet

            using DbDataReader reader = command.ExecuteReader(); // Kører SQL-kommandoen og læser resultatet
            while (reader.Read()) // Kører så længe der er flere rækker i resultatet
            {
                reviews.Add(MapReview(reader)); // Omdanner databaserækken til et Review-objekt og tilføjer det til listen
            }

            reader.Close(); // Lukker readeren
            return reviews; // Returnerer listen med reviews
        }

        public IEnumerable<Review> GetReviewsByRevieweeId(int userId) // Henter reviews hvor en bestemt bruger er blevet vurderet
        {
            using SqliteConnection connection = _db.CreateConnection(); // Opretter en forbindelse til databasen
            using DbCommand command = connection.CreateCommand(); // Opretter en SQL-kommando på forbindelsen

            command.CommandText = @"
                    SELECT r.id, r.rating, r.comment, r.created_at, r.rental_id, r.reviewer_id, r.reviewee_id,
                    reviewer.Username AS ReviewerUsername,
                    reviewee.Username AS RevieweeUsername
                    FROM Reviews r
                    LEFT JOIN Users reviewer ON reviewer.ID = r.reviewer_id
                    LEFT JOIN Users reviewee ON reviewee.ID = r.reviewee_id
                    WHERE r.reviewee_id = @UserId
                    ORDER BY r.created_at DESC"; // SQL der henter reviews for den bruger der bliver vurderet

            DbParameter parameter = command.CreateParameter(); // Opretter en parameter til SQL-kommandoen
            parameter.DbType = DbType.Int32; // Fortæller at parameteren er et heltal
            parameter.ParameterName = "@UserId"; // Navnet på parameteren i SQL'en
            parameter.Value = userId; // Sætter parameterens værdi til det userId metoden modtager
            command.Parameters.Add(parameter); // Tilføjer parameteren til SQL-kommandoen

            List<Review> reviews = new List<Review>(); // Opretter en liste til de reviews der bliver hentet

            using DbDataReader reader = command.ExecuteReader(); // Kører SQL-kommandoen og læser resultatet
            while (reader.Read()) // Kører så længe der er flere rækker
            {
                reviews.Add(MapReview(reader)); // Mapper rækken til et Review-objekt og tilføjer det til listen
            }

            reader.Close(); // Lukker readeren
            return reviews; // Returnerer listen med reviews
        }

        public IEnumerable<Review> GetReviewsByReviewerId(int userId) // Henter reviews som en bestemt bruger har skrevet
        {
            using SqliteConnection connection = _db.CreateConnection(); // Opretter en forbindelse til databasen
            using DbCommand command = connection.CreateCommand(); // Opretter en SQL-kommando på forbindelsen

            command.CommandText = @"
                    SELECT r.id, r.rating, r.comment, r.created_at, r.rental_id, r.reviewer_id, r.reviewee_id,
                    reviewer.Username AS ReviewerUsername,
                    reviewee.Username AS RevieweeUsername
                    FROM Reviews r
                    LEFT JOIN Users reviewer ON reviewer.ID = r.reviewer_id
                    LEFT JOIN Users reviewee ON reviewee.ID = r.reviewee_id
                    WHERE r.reviewer_id = @UserId
                    ORDER BY r.created_at DESC"; // SQL der henter reviews skrevet af en bestemt bruger

            DbParameter parameter = command.CreateParameter(); // Opretter en parameter til SQL-kommandoen
            parameter.DbType = DbType.Int32; // Fortæller at parameteren er et heltal
            parameter.ParameterName = "@UserId"; // Navnet på parameteren i SQL'en
            parameter.Value = userId; // Sætter parameterens værdi til det userId metoden modtager
            command.Parameters.Add(parameter); // Tilføjer parameteren til SQL-kommandoen

            List<Review> reviews = new List<Review>(); // Opretter en liste til de reviews der bliver hentet

            using DbDataReader reader = command.ExecuteReader(); // Kører SQL-kommandoen og læser resultatet
            while (reader.Read()) // Kører så længe der er flere rækker
            {
                reviews.Add(MapReview(reader)); // Mapper rækken til et Review-objekt og tilføjer det til listen
            }

            reader.Close(); // Lukker readeren
            return reviews; // Returnerer listen med reviews
        }

        public void CreateReview(Review review) // Opretter et nyt review i databasen
        {
            using SqliteConnection connection = _db.CreateConnection(); // Opretter en forbindelse til databasen
            using DbCommand command = connection.CreateCommand(); // Opretter en SQL-kommando på forbindelsen

            command.CommandText = @"
                    INSERT INTO Reviews 
                    (rating, comment, rental_id, reviewer_id, reviewee_id)
                    VALUES 
                    (@Rating, @Comment, @RentalId, @ReviewerId, @RevieweeId)"; // SQL der indsætter et nyt review i databasen

            DbParameter ratingParam = command.CreateParameter(); // Opretter parameter til rating
            ratingParam.DbType = DbType.Int32; // Fortæller at rating er et heltal
            ratingParam.ParameterName = "@Rating"; // Navnet på rating-parameteren i SQL'en
            ratingParam.Value = review.Rating; // Sætter værdien til reviewets rating
            command.Parameters.Add(ratingParam); // Tilføjer rating-parameteren til kommandoen

            DbParameter commentParam = command.CreateParameter(); // Opretter parameter til kommentar
            commentParam.DbType = DbType.String; // Fortæller at kommentaren er tekst
            commentParam.ParameterName = "@Comment"; // Navnet på comment-parameteren i SQL'en
            commentParam.Value = review.Comment; // Sætter værdien til reviewets kommentar
            command.Parameters.Add(commentParam); // Tilføjer comment-parameteren til kommandoen

            DbParameter rentalParam = command.CreateParameter(); // Opretter parameter til rental_id
            rentalParam.DbType = DbType.Int32; // Fortæller at rental_id er et heltal
            rentalParam.ParameterName = "@RentalId"; // Navnet på rental-parameteren i SQL'en

            if (review.RentalId.HasValue) // Tjekker om reviewet har en rental id
            {
                rentalParam.Value = review.RentalId; // Sætter rental id som værdi
            }
            else // Hvis reviewet ikke har en rental id
            {
                rentalParam.Value = DBNull.Value; // Sætter værdien til database-null
            }

            command.Parameters.Add(rentalParam); // Tilføjer rental-parameteren til kommandoen

            DbParameter reviewerParam = command.CreateParameter(); // Opretter parameter til reviewer_id
            reviewerParam.DbType = DbType.Int32; // Fortæller at reviewer_id er et heltal
            reviewerParam.ParameterName = "@ReviewerId"; // Navnet på reviewer-parameteren i SQL'en
            reviewerParam.Value = review.ReviewerId; // Sætter værdien til id'et på brugeren der skriver reviewet
            command.Parameters.Add(reviewerParam); // Tilføjer reviewer-parameteren til kommandoen

            DbParameter revieweeParam = command.CreateParameter(); // Opretter parameter til reviewee_id
            revieweeParam.DbType = DbType.Int32; // Fortæller at reviewee_id er et heltal
            revieweeParam.ParameterName = "@RevieweeId"; // Navnet på reviewee-parameteren i SQL'en
            revieweeParam.Value = review.RevieweeId; // Sætter værdien til id'et på brugeren der bliver vurderet
            command.Parameters.Add(revieweeParam); // Tilføjer reviewee-parameteren til kommandoen

            command.ExecuteNonQuery(); // Kører SQL-kommandoen og gemmer reviewet i databasen
        }
    }
}