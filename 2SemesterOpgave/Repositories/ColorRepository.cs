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
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

				command.CommandText = "SELECT * FROM Colors";

				using IDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					ColorDTO dto = CreateDTO(reader);
					colors.Add(dto);
				}

				return colors;
		}

		private ColorDTO CreateDTO(IDataReader reader)
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
