using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Repositories
{
	public class SizeRepository
	{
		Database _db;

		public SizeRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<SizeDTO> GetAllSizes()
		{
			List<SizeDTO> dtos = new();

			try
			{
				_db.Open();

				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM Sizes";

				using DbDataReader reader = command.ExecuteReader();

				int id = reader.GetOrdinal("id");
				int name = reader.GetOrdinal("name");
				int created = reader.GetOrdinal("created_at");
				int updated = reader.GetOrdinal("updated_at");

				while (reader.Read())
				{
					dtos.Add(CreateDTO(reader, id, name, created, updated));
				}

				return dtos;
			}
			finally
			{
				_db.Close();
			}
		}

		SizeDTO CreateDTO(DbDataReader reader, int id, int name, int created, int updated)
		{
			return new SizeDTO
			{
				Id = reader.GetInt32(id),
				Name = reader.GetString(name),
				CreatedAt = DateTime.Parse(reader.GetString(created)),
				UpdatedAt = DateTime.Parse(reader.GetString(updated))
			};
		}
	}
}
