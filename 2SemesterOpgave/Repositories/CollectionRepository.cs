using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Repositories
{
	public class CollectionRepository
	{
		Database _db;

		public CollectionRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<CollectionDTO> GetAllCollections()
		{
			List<CollectionDTO> collections = new List<CollectionDTO>();

			try
			{
				_db.Open();

				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM Collections";

				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					CollectionDTO dto = CreateDTO(reader);
					collections.Add(dto);
				}

				return collections;
			}
			finally
			{
				_db.Close();
			}
		}

		CollectionDTO CreateDTO(DbDataReader reader)
		{
			int id = reader.GetOrdinal("id");
			int name = reader.GetOrdinal("name");
			int brandId = reader.GetOrdinal("brand_id");
			int designerId = reader.GetOrdinal("designer_id");
			int createdAt = reader.GetOrdinal("created_at");
			int updatedAt = reader.GetOrdinal("updated_at");

			CollectionDTO dto = new CollectionDTO();
			dto.Id = reader.GetInt32(id);
			dto.Name = reader.GetString(name);
			if (!reader.IsDBNull(brandId))
			{
				dto.BrandId = reader.GetInt32(brandId);
			}
			if (!reader.IsDBNull(designerId))
			{
				dto.DesignerId = reader.GetInt32(designerId);
			}

			dto.CreatedAt = DateTime.Parse(reader.GetString(createdAt));
			dto.UpdatedAt = DateTime.Parse(reader.GetString(updatedAt));

			return dto;
		}
	}
}
