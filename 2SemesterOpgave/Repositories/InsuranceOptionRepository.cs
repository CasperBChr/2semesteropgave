using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
	public class InsuranceOptionRepository
	{
		Database _db;

		public InsuranceOptionRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<InsuranceOptionDTO> GetAll()
		{
			List<InsuranceOptionDTO> dtos = new();

			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM InsuranceOptions";

				using DbDataReader reader = command.ExecuteReader();

				int id = reader.GetOrdinal("id");
				int name = reader.GetOrdinal("name");
				int baseFees = reader.GetOrdinal("base_fees");

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader, id, name, baseFees));
				}

				return dtos;

		}

		InsuranceOptionDTO CreateDTO(DbDataReader reader, int id, int name, int baseFees)
		{
			return new InsuranceOptionDTO
			{
				Id = reader.GetInt32(id),
				Name = reader.GetString(name),
				BaseFees = reader.GetFloat(baseFees)
			};
		}
	}
}
