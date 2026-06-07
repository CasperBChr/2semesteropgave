using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
	public class RentalRepository
	{
		Database _db;

		public RentalRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<RentalDTO> GetAll()
		{
			List<RentalDTO> dtos = new List<RentalDTO>();

			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Rentals";

				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader));
				}
				reader.Close();
				return dtos;

		}

		public IEnumerable<RentalDTO> GetByRenterId(int renterId)
		{
			List<RentalDTO> dtos = new List<RentalDTO>();

			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Rentals WHERE renter_id = @renterId";

				DbParameter param = command.CreateParameter();
				param.ParameterName = "@renterId";
				param.DbType = DbType.Int32;
				param.Value = renterId;
				command.Parameters.Add(param);

				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader));
				}
				reader.Close();
				return dtos;
		}

		public IEnumerable<RentalDTO> GetByRenteeId(int renteeId)
		{
			List<RentalDTO> dtos = new List<RentalDTO>();

			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Rentals WHERE rentee_id = @renteeId";

				DbParameter param = command.CreateParameter();
				param.ParameterName = "@renteeId";
				param.DbType = DbType.Int32;
				param.Value = renteeId;
				command.Parameters.Add(param);

				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader));
				}
				reader.Close();
				return dtos;
		}

		public IEnumerable<(DateOnly Start, DateOnly End)> GetBookedDateRangesForArticle(int articleId)
		{
			List<(DateOnly, DateOnly)> ranges = new();

			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = @"
				SELECT start_date, end_date FROM Rentals
				WHERE article_id = @articleId
				AND status != 'cancelled'";

			AddParameter(command, "@articleId", DbType.Int32, articleId);

			using DbDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				DateOnly start = DateOnly.ParseExact(reader.GetString(0), "yyyy-MM-dd");
				DateOnly end = DateOnly.ParseExact(reader.GetString(1), "yyyy-MM-dd");
				ranges.Add((start, end));
			}
			return ranges;
		}

		public void Create(RentalDTO dto)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = @"
					INSERT INTO Rentals 
                    (start_date, end_date, total_price, is_accepted, renter_id, rentee_id, article_id, status, shipping_option_id, insurance_option_id)
                    VALUES 
                    (@startDate, @endDate, @totalPrice, @isAccepted, @renterId, @renteeId, @articleId, @status, @shippingOptionId, @insuranceOptionId)";


				if (!DateOnly.TryParseExact(dto.StartDate, "yyyy-MM-dd", out _)) 
				{
					throw new Exception($"Invalid StartDate format: {dto.StartDate}");
				}
				if (!DateOnly.TryParseExact(dto.EndDate, "yyyy-MM-dd", out _))
				{
					throw new Exception($"Invalid EndDate format: {dto.EndDate}");
				}
				AddParameter(command, "@startDate", DbType.String, dto.StartDate);
				AddParameter(command, "@endDate", DbType.String, dto.EndDate);
				AddParameter(command, "@totalPrice", DbType.Double, dto.TotalPrice);
				AddParameter(command, "@isAccepted", DbType.Int32, dto.IsAccepted ? 1 : 0);
				AddParameter(command, "@renterId", DbType.Int32, dto.RenterId);
				AddParameter(command, "@renteeId", DbType.Int32, dto.RenteeId);
				AddParameter(command, "@articleId", DbType.Int32, dto.ArticleId);
				AddParameter(command, "@status", DbType.String, dto.Status);
				AddParameter(command, "@shippingOptionId", DbType.Int32, dto.ShippingOptionId.HasValue ? dto.ShippingOptionId.Value : DBNull.Value);
				AddParameter(command, "@insuranceOptionId", DbType.Int32, dto.InsuranceOptionId.HasValue ? dto.InsuranceOptionId.Value : DBNull.Value);

				command.ExecuteNonQuery();
		}

		public void UpdateStatus(int id, string status)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "UPDATE Rentals SET status = @status WHERE id = @id";

				AddParameter(command, "@status", DbType.String, status);
				AddParameter(command, "@id", DbType.Int32, id);

				command.ExecuteNonQuery();
		}

		public void SetAccepted(int id, bool isAccepted)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "UPDATE Rentals SET is_accepted = @isAccepted WHERE id = @id";

				AddParameter(command, "@isAccepted", DbType.Int32, isAccepted ? 1 : 0);
				AddParameter(command, "@id", DbType.Int32, id);

				command.ExecuteNonQuery();
		}

		// Hjælpemetode så vi undgår at gentage parameter-oprettelse
		void AddParameter(DbCommand command, string name, DbType type, object value)
		{
			DbParameter param = command.CreateParameter();
			param.ParameterName = name;
			param.DbType = type;
			param.Value = value;
			command.Parameters.Add(param);
		}

		RentalDTO CreateDTO(DbDataReader reader)
		{
			int id = reader.GetOrdinal("id");
			int startDate = reader.GetOrdinal("start_date");
			int endDate = reader.GetOrdinal("end_date");
			int totalPrice = reader.GetOrdinal("total_price");
			int isAccepted = reader.GetOrdinal("is_accepted");
			int status = reader.GetOrdinal("status");
			int renterId = reader.GetOrdinal("renter_id");
			int renteeId = reader.GetOrdinal("rentee_id");
			int articleId = reader.GetOrdinal("article_id");
			int shippingOptionId = reader.GetOrdinal("shipping_option_id");
			int insuranceOptionId = reader.GetOrdinal("insurance_option_id");
			int createdAt = reader.GetOrdinal("created_at");

			return new RentalDTO
			{
				Id = reader.GetInt32(id),
				StartDate = reader.GetString(startDate),
				EndDate = reader.GetString(endDate),
				TotalPrice = reader.GetFloat(totalPrice),
				IsAccepted = reader.GetInt32(isAccepted) == 1,
				Status = reader.GetString(status),
				RenterId = reader.GetInt32(renterId),
				RenteeId = reader.GetInt32(renteeId),
				ArticleId = reader.GetInt32(articleId),
				ShippingOptionId = reader.GetInt32(shippingOptionId),
				InsuranceOptionId = reader.GetInt32(insuranceOptionId),
				CreatedAt = DateTime.Parse(reader.GetString(createdAt))
			};
		}
	}
}
