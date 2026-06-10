using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
	public class ColorRepository
	{
		IDatabaseFactory _db;

		public ColorRepository(IDatabaseFactory db)
		{
			_db = db;
		}


		public IEnumerable<ColorDTO> GetAllColors()
		{
			List<ColorDTO> colors = new List<ColorDTO>();
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();

				command.CommandText = "SELECT * FROM Colors";

				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					ColorDTO dto = CreateDTO(reader);
					colors.Add(dto);
				}

				return colors;
		}

		private ColorDTO CreateDTO(DbDataReader reader)
		{
			int id = reader.GetOrdinal("id");
			int name = reader.GetOrdinal("name");

			ColorDTO dto = new ColorDTO
			{
				Id = reader.GetInt32(id),
				Name = reader.GetString(name)
			};

			return dto;
		}
	}
}
