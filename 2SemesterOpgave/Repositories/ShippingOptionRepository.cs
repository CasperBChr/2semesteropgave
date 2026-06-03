using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Repositories
{
	public class ShippingOptionRepository
	{
		Database _db;

		public ShippingOptionRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<ShippingOptionDTO> GetAll()
		{
			List<ShippingOptionDTO> dtos = new();

			try
			{
				_db.Open();
				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM ShippingOptions";

				using DbDataReader reader = command.ExecuteReader();

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
			finally
			{
				_db.Close();
			}
		}

		ShippingOptionDTO CreateDTO(DbDataReader reader, int id, int name, int baseFee, int deliveryTimeDays)
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
