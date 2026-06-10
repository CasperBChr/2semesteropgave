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
	public class ShippingOptionRepository
	{
		IDatabaseFactory _db;

		public ShippingOptionRepository(IDatabaseFactory db)
		{
			_db = db;
		}

		public IEnumerable<ShippingOptionDTO> GetAll()
		{
			List<ShippingOptionDTO> dtos = new();

			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM ShippingOptions";

				using IDataReader reader = command.ExecuteReader();

				int id = reader.GetOrdinal("id");
				int name = reader.GetOrdinal("name");
				int baseFee = reader.GetOrdinal("base_fee");
				int deliveryTimeDays = reader.GetOrdinal("delivery_time_days");

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader, id, name, baseFee, deliveryTimeDays));
				}

				return dtos;
		}

		ShippingOptionDTO CreateDTO(IDataReader reader, int id, int name, int baseFee, int deliveryTimeDays)
		{
			return new ShippingOptionDTO
			{
				Id = reader.GetInt32(id),
				Name = reader.GetString(name),
				BaseFee = reader.GetFloat(baseFee),
				DeliveryTimeDays = reader.GetInt32(deliveryTimeDays)
			};
		}
	}
}
