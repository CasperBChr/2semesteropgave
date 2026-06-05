using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Xml.Linq;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
	public class BrandRepository
	{
		Database _db;	

		public BrandRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<BrandDTO> GetAll()
		{
			List<BrandDTO> brands = new List<BrandDTO>();

				using SqliteConnection connection = _db.CreateConnection();
				using DbCommand command = connection.CreateCommand();
				command.CommandText = "SELECT * FROM Brands";
				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					BrandDTO dto = CreateDTO(reader);

					brands.Add(dto);
				}

				return brands;
		}

		public BrandDTO? GetById(int idValue)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
				command.CommandText = "SELECT * FROM Brands WHERE id = @id";

				var param = command.CreateParameter();
				param.ParameterName = "@id";
				param.Value = idValue;
				command.Parameters.Add(param);

				using DbDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					return CreateDTO(reader);
				}

				return null;

		}


		BrandDTO CreateDTO(DbDataReader reader)
		{
			int id = reader.GetOrdinal("id");
			int name = reader.GetOrdinal("name");
			int description = reader.GetOrdinal("name");
			int logo = reader.GetOrdinal("logopath");
			int created = reader.GetOrdinal("created_at");
			int updated = reader.GetOrdinal("updated_at");

			string? logoString;
			if (reader.IsDBNull(logo))
			{
				logoString = null;
			}
			else
			{
				logoString = reader.GetString(logo);
			}

			return new BrandDTO
			{
				Id = reader.GetInt32(id),
				Name = reader.GetString(name),
				Description = reader.GetString(description),
				LogoPath = logoString,
				CreatedAt = DateTime.Parse(reader.GetString(created)),
				UpdatedAt = DateTime.Parse(reader.GetString(updated))
			};
		}
	}
}
