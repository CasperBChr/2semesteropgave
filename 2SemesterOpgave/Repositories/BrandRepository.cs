using System;
using System.Collections.Generic;
using System.Data;
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
		IDatabaseFactory _db;	

		public BrandRepository(IDatabaseFactory db)
		{
			_db = db;
		}

		public IEnumerable<BrandDTO> GetAll()
		{
			List<BrandDTO> brands = new List<BrandDTO>();

				using IDbConnection connection = _db.CreateConnection();
				using IDbCommand command = connection.CreateCommand();
				command.CommandText = "SELECT * FROM Brands";
				using IDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					BrandDTO dto = CreateDTO(reader);

					brands.Add(dto);
				}

				return brands;
		}

		BrandDTO CreateDTO(IDataReader reader)
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
