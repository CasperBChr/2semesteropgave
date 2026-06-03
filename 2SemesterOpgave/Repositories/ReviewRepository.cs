using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Repositories
{
	public class ReviewRepository
	{
		Database _db;

		public ReviewRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<ReviewDTO> GetAll()
		{
			List<ReviewDTO> dtos = new();

			try
			{
				_db.Open();
				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM Reviews";

				using DbDataReader reader = command.ExecuteReader();

				int id = reader.GetOrdinal("id");
				int rating = reader.GetOrdinal("rating");
				int comment = reader.GetOrdinal("comment");
				int createdAt = reader.GetOrdinal("created_at");
				int rentalId = reader.GetOrdinal("rental_id");
				int reviewerId = reader.GetOrdinal("reviewer_id");
				int revieweeId = reader.GetOrdinal("reviewee_id");

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader, id, rating, comment, createdAt, rentalId, reviewerId, revieweeId));
				}

				return dtos;
			}
			finally
			{
				_db.Close();
			}
		}

		public IEnumerable<ReviewDTO> GetByRevieweeId(int revieweeId)
		{
			List<ReviewDTO> dtos = new();

			try
			{
				_db.Open();
				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM Reviews WHERE reviewee_id = @RevieweeId";

				DbParameter param = command.CreateParameter();
				param.ParameterName = "@RevieweeId";
				param.DbType = DbType.Int32;
				param.Value = revieweeId;
				command.Parameters.Add(param);

				using DbDataReader reader = command.ExecuteReader();

				int id = reader.GetOrdinal("id");
				int rating = reader.GetOrdinal("rating");
				int comment = reader.GetOrdinal("comment");
				int createdAt = reader.GetOrdinal("created_at");
				int rentalId = reader.GetOrdinal("rental_id");
				int rId = reader.GetOrdinal("reviewer_id");
				int reeId = reader.GetOrdinal("reviewee_id");

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader, id, rating, comment, createdAt, rentalId, rId, reeId));
				}

				return dtos;
			}
			finally
			{
				_db.Close();
			}
		}

		public void Create(ReviewDTO dto)
		{
			try
			{
				_db.Open();
				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "INSERT INTO Reviews (rating, comment, rental_id, reviewer_id, reviewee_id) VALUES (@Rating, @Comment, @RentalId, @ReviewerId, @RevieweeId)";

				DbParameter rating = command.CreateParameter();
				rating.ParameterName = "@Rating";
				rating.DbType = DbType.Int32;
				rating.Value = dto.Rating;
				command.Parameters.Add(rating);

				DbParameter comment = command.CreateParameter();
				comment.ParameterName = "@Comment";
				comment.DbType = DbType.String;
				comment.Value = dto.Comment;
				command.Parameters.Add(comment);

				DbParameter rentalId = command.CreateParameter();
				rentalId.ParameterName = "@RentalId";
				rentalId.DbType = DbType.Int32;
				rentalId.Value = dto.RentalId;
				command.Parameters.Add(rentalId);

				DbParameter reviewerId = command.CreateParameter();
				reviewerId.ParameterName = "@ReviewerId";
				reviewerId.DbType = DbType.Int32;
				reviewerId.Value = dto.ReviewerId;
				command.Parameters.Add(reviewerId);

				DbParameter revieweeId = command.CreateParameter();
				revieweeId.ParameterName = "@RevieweeId";
				revieweeId.DbType = DbType.Int32;
				revieweeId.Value = dto.RevieweeId;
				command.Parameters.Add(revieweeId);

				command.ExecuteNonQuery();
			}
			finally
			{
				_db.Close();
			}
		}

		ReviewDTO CreateDTO(DbDataReader reader, int id, int rating, int comment, int createdAt, int rentalId, int reviewerId, int revieweeId)
		{
			return new ReviewDTO
			{
				Id = reader.GetInt32(id),
				Rating = reader.GetInt32(rating),
				Comment = reader.IsDBNull(comment) ? string.Empty : reader.GetString(comment),
				CreatedAt = Convert.ToDateTime(reader.GetValue(createdAt)),
				RentalId = reader.IsDBNull(rentalId) ? 0 : reader.GetInt32(rentalId),
				ReviewerId = reader.IsDBNull(reviewerId) ? 0 : reader.GetInt32(reviewerId),
				RevieweeId = reader.IsDBNull(revieweeId) ? 0 : reader.GetInt32(revieweeId)
			};
		}
	}
}
