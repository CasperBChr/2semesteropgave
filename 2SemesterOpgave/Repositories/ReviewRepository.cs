using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
// Giver adgang til database-factory, som kan oprette databaseforbindelser
using _2SemesterOpgave.Data;
// Giver adgang til vores modelklasser, fx Review
using _2SemesterOpgave.Models;
// Giver adgang til ReviewDTO, som bruges til data fra databasen
using _2SemesterOpgave.Repositories.DTO;
// Giver adgang til SQLite-forbindelsen
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{

    /// <summary>
    /// Interaction logic for ReviewRepository.xaml === Kodet af Daniel
    /// </summary>


    // Repositoryklasse der håndterer databasekald for reviews
    public class ReviewRepository
    {
        // Bruges til at oprette forbindelse til databasen
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public ReviewRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Henter alle reviews fra databasen
        public IEnumerable<ReviewDTO> GetAll()
        {
            // Opretter en forbindelse til databasen
			using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
			using IDbCommand command = connection.CreateCommand();
            // SQL der henter alle reviews og sorterer de nyeste først
            command.CommandText = @"
                SELECT id, rating, comment, created_at, rental_id, reviewer_id, reviewee_id
                FROM Reviews
                ORDER BY created_at DESC";

            // Opretter en liste til de reviews der bliver hentet
            List<ReviewDTO> reviews = new List<ReviewDTO>();

            // Kører SQL-kommandoen og læser resultatet
			using IDataReader reader = command.ExecuteReader();

            // Kører så længe der er flere rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ReviewDTO og tilføjer den til listen
                reviews.Add(MapDTO(reader));
            }

            // Lukker readeren
            reader.Close();

            // Returnerer listen med reviews
            return reviews;
        }

        // Henter reviews som en bestemt bruger har modtaget
        public IEnumerable<ReviewDTO> GetReviewsByRevieweeId(int userId)
        {
            // Opretter en forbindelse til databasen
			using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
			using IDbCommand command = connection.CreateCommand();

            // SQL der henter reviews hvor brugeren er den der bliver vurderet
            command.CommandText = @"
                SELECT id, rating, comment, created_at, rental_id, reviewer_id, reviewee_id
                FROM Reviews
                WHERE reviewee_id = @UserId
                ORDER BY created_at DESC";

            // Opretter en parameter til SQL-kommandoen
			IDbDataParameter parameter = command.CreateParameter();

            // Fortæller at parameteren er et heltal
            parameter.DbType = DbType.Int32;

            // Navnet på parameteren i SQL'en
            parameter.ParameterName = "@UserId";

            // Sætter parameterens værdi til det userId metoden modtager
            parameter.Value = userId;

            // Tilføjer parameteren til SQL-kommandoen
            command.Parameters.Add(parameter);

            // Opretter en liste til de reviews der bliver hentet
            List<ReviewDTO> reviews = new List<ReviewDTO>();

            // Kører SQL-kommandoen og læser resultatet
			using IDataReader reader = command.ExecuteReader();

            // Kører så længe der er flere rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ReviewDTO og tilføjer den til listen
                reviews.Add(MapDTO(reader));
            }

            // Lukker readeren
            reader.Close();

            // Returnerer listen med reviews
            return reviews;
        }

        // Henter reviews som en bestemt bruger har skrevet
        public IEnumerable<ReviewDTO> GetReviewsByReviewerId(int userId)
        {
            // Opretter en forbindelse til databasen
			using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
			using IDbCommand command = connection.CreateCommand();

            // SQL der henter reviews hvor brugeren er den der har skrevet reviewet
            command.CommandText = @"
                SELECT id, rating, comment, created_at, rental_id, reviewer_id, reviewee_id
                FROM Reviews
                WHERE reviewer_id = @UserId
                ORDER BY created_at DESC";

            // Opretter en parameter til SQL-kommandoen
			IDbDataParameter parameter = command.CreateParameter();

            // Fortæller at parameteren er et heltal
            parameter.DbType = DbType.Int32;

            // Navnet på parameteren i SQL'en
            parameter.ParameterName = "@UserId";

            // Sætter parameterens værdi til det userId metoden modtager
            parameter.Value = userId;

            // Tilføjer parameteren til SQL-kommandoen
            command.Parameters.Add(parameter);

            // Opretter en liste til de reviews der bliver hentet
            List<ReviewDTO> reviews = new List<ReviewDTO>();

            // Kører SQL-kommandoen og læser resultatet
			using IDataReader reader = command.ExecuteReader();

            // Kører så længe der er flere rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ReviewDTO og tilføjer den til listen
                reviews.Add(MapDTO(reader));
            }

            // Lukker readeren
            reader.Close();

            // Returnerer listen med reviews
            return reviews;
        }

        // Opretter et nyt review i databasen
        public void CreateReview(Review review)
        {
            // Opretter en forbindelse til databasen
			using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
			using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter et nyt review i databasen
            command.CommandText = @"
                INSERT INTO Reviews 
                (rating, comment, rental_id, reviewer_id, reviewee_id)
                VALUES 
                (@Rating, @Comment, @RentalId, @ReviewerId, @RevieweeId)";

            // Opretter parameter til rating
			IDbDataParameter ratingParam = command.CreateParameter();

            // Fortæller at rating er et heltal
            ratingParam.DbType = DbType.Int32;

            // Navnet på rating-parameteren i SQL'en
            ratingParam.ParameterName = "@Rating";

            // Sætter værdien til reviewets rating
            ratingParam.Value = review.Rating;

            // Tilføjer rating-parameteren til kommandoen
            command.Parameters.Add(ratingParam);

			// Opretter parameter til kommentar
			IDbDataParameter commentParam = command.CreateParameter();

            // Fortæller at kommentaren er tekst
            commentParam.DbType = DbType.String;

            // Navnet på comment-parameteren i SQL'en
            commentParam.ParameterName = "@Comment";

            // Sætter værdien til reviewets kommentar
            commentParam.Value = review.Comment;

            // Tilføjer comment-parameteren til kommandoen
            command.Parameters.Add(commentParam);

			// Opretter parameter til rental_id
			IDbDataParameter rentalParam = command.CreateParameter();

            // Fortæller at rental_id er et heltal
            rentalParam.DbType = DbType.Int32;

            // Navnet på rental-parameteren i SQL'en
            rentalParam.ParameterName = "@RentalId";

            // Sætter rental id, eller database-null hvis der ikke er et
            rentalParam.Value = review.RentalId.HasValue ? review.RentalId : DBNull.Value;

            // Tilføjer rental-parameteren til kommandoen
            command.Parameters.Add(rentalParam);

			// Opretter parameter til reviewer_id
			IDbDataParameter reviewerParam = command.CreateParameter();

            // Fortæller at reviewer_id er et heltal
            reviewerParam.DbType = DbType.Int32;

            // Navnet på reviewer-parameteren i SQL'en
            reviewerParam.ParameterName = "@ReviewerId";

            // Sætter værdien til id'et på brugeren der skriver reviewet
            reviewerParam.Value = review.ReviewerId;

            // Tilføjer reviewer-parameteren til kommandoen
            command.Parameters.Add(reviewerParam);

            // Opretter parameter til reviewee_id
            IDbDataParameter revieweeParam = command.CreateParameter();

            // Fortæller at reviewee_id er et heltal
            revieweeParam.DbType = DbType.Int32;

            // Navnet på reviewee-parameteren i SQL'en
            revieweeParam.ParameterName = "@RevieweeId";

            // Sætter værdien til id'et på brugeren der bliver vurderet
            revieweeParam.Value = review.RevieweeId;

            // Tilføjer reviewee-parameteren til kommandoen
            command.Parameters.Add(revieweeParam);

            // Kører SQL-kommandoen og gemmer reviewet i databasen
            command.ExecuteNonQuery();
        }


        // Omdanner en række fra databasen til en ReviewDTO
        private static ReviewDTO MapDTO(IDataReader reader)
        {
            return new ReviewDTO
            {
                // Henter reviewets id
                Id = reader.GetInt32(reader.GetOrdinal("id")),

                // Henter reviewets rating
                Rating = reader.GetInt32(reader.GetOrdinal("rating")),

                // Henter kommentaren, eller tom tekst hvis feltet er null
                Comment = reader.IsDBNull(reader.GetOrdinal("comment"))
                    ? string.Empty
                    : reader.GetString(reader.GetOrdinal("comment")),

                // Henter rental id, eller null hvis feltet er tomt
                RentalId = reader.IsDBNull(reader.GetOrdinal("rental_id"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("rental_id")),

                // Konverterer dato fra tekst til DateTime
                CreatedAt = DateTime.ParseExact(
                    reader.GetString(reader.GetOrdinal("created_at")),
                    "yyyy-MM-dd HH:mm:ss", null),

                // Henter id på brugeren der skrev reviewet
                ReviewerId = reader.GetInt32(reader.GetOrdinal("reviewer_id")),

                // Henter id på brugeren der blev vurderet
                RevieweeId = reader.GetInt32(reader.GetOrdinal("reviewee_id"))
            };
        }
    }
}
