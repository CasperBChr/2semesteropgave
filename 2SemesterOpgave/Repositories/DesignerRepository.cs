using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
	public class DesignerRepository
	{
		Database _db;

		public DesignerRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<DesignerDTO> GetAllDesigners()
		{
			List<DesignerDTO> dtos = new List<DesignerDTO>();

			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Designers";

				using DbDataReader reader = command.ExecuteReader();

				// CACHED ORDINALS (performance + konsistens)
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

		DesignerDTO CreateDTO(DbDataReader reader, int id, int name, int created, int updated)
		{
			return new DesignerDTO
			{
				Id = reader.GetInt32(id),
				Name = reader.GetString(name),
				CreatedAt = DateTime.Parse(reader.GetString(created)),
				UpdatedAt = DateTime.Parse(reader.GetString(updated))
			};
		}
	}
}
