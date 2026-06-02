using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Repositories
{
	public class ColorRepository
	{
		Database _db;

		public ColorRepository(Database db)
		{
			_db = db;
		}


		public IEnumerable<ColorDTO> GetAllColors()
		{
			List<ColorDTO> colors = new List<ColorDTO>();

			try
			{
				_db.Open();

				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM Colors";

				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					ColorDTO dto = CreateDTO(reader);
					colors.Add(dto);
				}

				return colors;
			}
			finally
			{
				_db.Close();
			}
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
