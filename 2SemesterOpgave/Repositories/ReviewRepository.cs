using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
    public class ReviewRepository
    {
		IDatabaseFactory _db;

        public ReviewRepository(IDatabaseFactory db)
        {
            _db = db;
        }

		public IEnumerable<ReviewDTO> GetAll()
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = @"
                SELECT id, rating, comment, created_at, rental_id, reviewer_id, reviewee_id
                FROM Reviews
                ORDER BY created_at DESC";

			List<ReviewDTO> reviews = new List<ReviewDTO>();

			using IDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				reviews.Add(MapDTO(reader));
			}
			reader.Close();
			return reviews;
		}

		public IEnumerable<ReviewDTO> GetReviewsByRevieweeId(int userId)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = @"
                SELECT id, rating, comment, created_at, rental_id, reviewer_id, reviewee_id
                FROM Reviews
                WHERE reviewee_id = @UserId
                ORDER BY created_at DESC";

			IDbDataParameter parameter = command.CreateParameter();
			parameter.DbType = DbType.Int32;
			parameter.ParameterName = "@UserId";
			parameter.Value = userId;
			command.Parameters.Add(parameter);

			List<ReviewDTO> reviews = new List<ReviewDTO>();

			using IDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				reviews.Add(MapDTO(reader));
			}
			reader.Close();
			return reviews;
		}

		public IEnumerable<ReviewDTO> GetReviewsByReviewerId(int userId)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = @"
                SELECT id, rating, comment, created_at, rental_id, reviewer_id, reviewee_id
                FROM Reviews
                WHERE reviewer_id = @UserId
                ORDER BY created_at DESC";

			IDbDataParameter parameter = command.CreateParameter();
			parameter.DbType = DbType.Int32;
			parameter.ParameterName = "@UserId";
			parameter.Value = userId;
			command.Parameters.Add(parameter);

			List<ReviewDTO> reviews = new List<ReviewDTO>();

			using IDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				reviews.Add(MapDTO(reader));
			}
			reader.Close();
			return reviews;
		}

		public void CreateReview(Review review)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = @"
                INSERT INTO Reviews 
                (rating, comment, rental_id, reviewer_id, reviewee_id)
                VALUES 
                (@Rating, @Comment, @RentalId, @ReviewerId, @RevieweeId)";

			IDbDataParameter ratingParam = command.CreateParameter();
			ratingParam.DbType = DbType.Int32;
			ratingParam.ParameterName = "@Rating";
			ratingParam.Value = review.Rating;
			command.Parameters.Add(ratingParam);

			IDbDataParameter commentParam = command.CreateParameter();
			commentParam.DbType = DbType.String;
			commentParam.ParameterName = "@Comment";
			commentParam.Value = review.Comment;
			command.Parameters.Add(commentParam);

			IDbDataParameter rentalParam = command.CreateParameter();
			rentalParam.DbType = DbType.Int32;
			rentalParam.ParameterName = "@RentalId";
			rentalParam.Value = review.RentalId.HasValue ? review.RentalId : DBNull.Value;
			command.Parameters.Add(rentalParam);

			IDbDataParameter reviewerParam = command.CreateParameter();
			reviewerParam.DbType = DbType.Int32;
			reviewerParam.ParameterName = "@ReviewerId";
			reviewerParam.Value = review.ReviewerId;
			command.Parameters.Add(reviewerParam);

			IDbDataParameter revieweeParam = command.CreateParameter();
			revieweeParam.DbType = DbType.Int32;
			revieweeParam.ParameterName = "@RevieweeId";
			revieweeParam.Value = review.RevieweeId;
			command.Parameters.Add(revieweeParam);

			command.ExecuteNonQuery();
		}


		private static ReviewDTO MapDTO(IDataReader reader)
		{
			return new ReviewDTO
			{
				Id = reader.GetInt32(reader.GetOrdinal("id")),
				Rating = reader.GetInt32(reader.GetOrdinal("rating")),
				Comment = reader.IsDBNull(reader.GetOrdinal("comment"))
					? string.Empty
					: reader.GetString(reader.GetOrdinal("comment")),
				RentalId = reader.IsDBNull(reader.GetOrdinal("rental_id"))
					? null
					: reader.GetInt32(reader.GetOrdinal("rental_id")),
				CreatedAt = DateTime.ParseExact(
					reader.GetString(reader.GetOrdinal("created_at")),
					"yyyy-MM-dd HH:mm:ss", null),
				ReviewerId = reader.GetInt32(reader.GetOrdinal("reviewer_id")),
				RevieweeId = reader.GetInt32(reader.GetOrdinal("reviewee_id"))
			};
		}
	}
}
