using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.Interfaces;

namespace _2SemesterOpgave.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly Database _db;

        public ReviewRepository(Database db)
        {
            _db = db;
        }

        private static Review MapReview(DbDataReader reader)
        {
            int rentalIdOrdinal = reader.GetOrdinal("rental_id");
            int createdAtOrdinal = reader.GetOrdinal("created_at");
            int reviewerIdOrdinal = reader.GetOrdinal("reviewer_id");
            int revieweeIdOrdinal = reader.GetOrdinal("reviewee_id");

            Review review = new Review
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Rating = reader.GetInt32(reader.GetOrdinal("rating")),
                Comment = reader["comment"]?.ToString() ?? string.Empty,

                ReviewerId = reader.IsDBNull(reviewerIdOrdinal)
                    ? 0
                    : reader.GetInt32(reviewerIdOrdinal),

                RevieweeId = reader.IsDBNull(revieweeIdOrdinal)
                    ? 0
                    : reader.GetInt32(revieweeIdOrdinal),

                ReviewerUsername = reader["ReviewerUsername"]?.ToString() ?? string.Empty,
                RevieweeUsername = reader["RevieweeUsername"]?.ToString() ?? string.Empty
            };

            review.RentalId = reader.IsDBNull(rentalIdOrdinal)
                ? null
                : reader.GetInt32(rentalIdOrdinal);

            if (!reader.IsDBNull(createdAtOrdinal) &&
                DateTime.TryParse(reader["created_at"].ToString(), out DateTime createdAt))
            {
                review.CreatedAt = createdAt;
            }

            return review;
        }

        public IEnumerable<Review> GetAll()
        {
            _db.Open();

            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText =
                @"SELECT r.id, r.rating, r.comment, r.created_at, r.rental_id, r.reviewer_id, r.reviewee_id,
                         reviewer.Username AS ReviewerUsername,
                         reviewee.Username AS RevieweeUsername
                  FROM Reviews r
                  LEFT JOIN Users reviewer ON reviewer.ID = r.reviewer_id
                  LEFT JOIN Users reviewee ON reviewee.ID = r.reviewee_id
                  ORDER BY r.created_at DESC";

            List<Review> reviews = new List<Review>();

            using DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                reviews.Add(MapReview(reader));
            }

            _db.Close();

            return reviews;
        }

        public IEnumerable<Review> GetReviewsByRevieweeId(int userId)
        {
            _db.Open();

            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText =
                @"SELECT r.id, r.rating, r.comment, r.created_at, r.rental_id, r.reviewer_id, r.reviewee_id,
                         reviewer.Username AS ReviewerUsername,
                         reviewee.Username AS RevieweeUsername
                  FROM Reviews r
                  LEFT JOIN Users reviewer ON reviewer.ID = r.reviewer_id
                  LEFT JOIN Users reviewee ON reviewee.ID = r.reviewee_id
                  WHERE r.reviewee_id = @UserId
                  ORDER BY r.created_at DESC";

            DbParameter param = command.CreateParameter();
            param.DbType = DbType.Int32;
            param.ParameterName = "@UserId";
            param.Value = userId;
            command.Parameters.Add(param);

            List<Review> reviews = new List<Review>();

            using DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                reviews.Add(MapReview(reader));
            }

            _db.Close();

            return reviews;
        }

        public IEnumerable<Review> GetReviewsByReviewerId(int userId)
        {
            _db.Open();

            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText =
                @"SELECT r.id, r.rating, r.comment, r.created_at, r.rental_id, r.reviewer_id, r.reviewee_id,
                         reviewer.Username AS ReviewerUsername,
                         reviewee.Username AS RevieweeUsername
                  FROM Reviews r
                  LEFT JOIN Users reviewer ON reviewer.ID = r.reviewer_id
                  LEFT JOIN Users reviewee ON reviewee.ID = r.reviewee_id
                  WHERE r.reviewer_id = @UserId
                  ORDER BY r.created_at DESC";

            DbParameter param = command.CreateParameter();
            param.DbType = DbType.Int32;
            param.ParameterName = "@UserId";
            param.Value = userId;
            command.Parameters.Add(param);

            List<Review> reviews = new List<Review>();

            using DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                reviews.Add(MapReview(reader));
            }

            _db.Close();

            return reviews;
        }

        public void CreateReview(Review review)
        {
            _db.Open();

            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO Reviews 
                    (rating, comment, rental_id, reviewer_id, reviewee_id)
                  VALUES 
                    (@Rating, @Comment, @RentalId, @ReviewerId, @RevieweeId)";

            DbParameter ratingParam = command.CreateParameter();
            ratingParam.DbType = DbType.Int32;
            ratingParam.ParameterName = "@Rating";
            ratingParam.Value = review.Rating;
            command.Parameters.Add(ratingParam);

            DbParameter commentParam = command.CreateParameter();
            commentParam.DbType = DbType.String;
            commentParam.ParameterName = "@Comment";
            commentParam.Value = review.Comment ?? string.Empty;
            command.Parameters.Add(commentParam);

            DbParameter rentalParam = command.CreateParameter();
            rentalParam.DbType = DbType.Int32;
            rentalParam.ParameterName = "@RentalId";
            rentalParam.Value = review.RentalId.HasValue
                ? review.RentalId.Value
                : DBNull.Value;
            command.Parameters.Add(rentalParam);

            DbParameter reviewerParam = command.CreateParameter();
            reviewerParam.DbType = DbType.Int32;
            reviewerParam.ParameterName = "@ReviewerId";
            reviewerParam.Value = review.ReviewerId;
            command.Parameters.Add(reviewerParam);

            DbParameter revieweeParam = command.CreateParameter();
            revieweeParam.DbType = DbType.Int32;
            revieweeParam.ParameterName = "@RevieweeId";
            revieweeParam.Value = review.RevieweeId;
            command.Parameters.Add(revieweeParam);

            command.ExecuteNonQuery();

            _db.Close();
        }
    }
}
