using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave;
using _2SemesterOpgave.Services;
using _2SemesterOpgave.Repositories.DTO;
using System.Diagnostics;

namespace _2SemesterOpgave.Repositories
{
	public class ArticleRepository
	{
		Database _db;
		public ArticleRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<ArticleDTO> GetAllArticles() 
		{
			List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

			try 
			{
				_db.Open();
				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM Articles";
				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					ArticleDTO articleDTO = CreateDTO(reader);
					articleDTOs.Add(articleDTO);
				}

				reader.Close();
				return articleDTOs;
			}
			finally
			{
				_db.Close();
			}
		}


        public IEnumerable<ArticleDTO> GetNewestArticles()
        {
			List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

			try 
			{
				_db.Open();
				using DbCommand command = _db.Connection.CreateCommand();
				command.CommandText = "SELECT * FROM Articles ORDER BY created_at DESC LIMIT 10";
				List<Article> articles = new List<Article>();
				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					ArticleDTO articleDTO = CreateDTO(reader);
					articleDTOs.Add(articleDTO);
				}

				reader.Close();
				return articleDTOs;
			}
			finally 
			{
				_db.Close();
			}
        }


        public IEnumerable<ArticleDTO> GetFilteredArticles(FilterCriteria filter)
		{
			List<ArticleDTO> articleDTOs = new List<ArticleDTO>();
			try
			{
				_db.Open();
				DbCommand command = _db.Connection.CreateCommand();

				StringBuilder sql = new StringBuilder("SELECT * FROM Articles WHERE 1=1");

				//if (filter.Category != null)
				//{
				//	sql.Append(" AND category_id = @categoryId");
				//	DbParameter parameter = command.CreateParameter();
				//	parameter.ParameterName = "@categoryId";
				//	parameter.Value = filter.Category.Id;
				//	command.Parameters.Add(parameter);
				//}

				//if (filter.SubCategory != null)
				//{
				//	sql.Append(" AND subcategory_id = @subId");
				//	DbParameter parameter = command.CreateParameter();
				//	parameter.ParameterName = "@subId";
				//	parameter.Value = filter.SubCategory.Id;
				//	command.Parameters.Add(parameter);
				//}

				if (!string.IsNullOrWhiteSpace(filter.SearchText))
				{
					sql.Append(" AND (name LIKE @search OR description LIKE @search)");
					DbParameter parameter = command.CreateParameter();
					parameter.ParameterName = "@search";
					parameter.Value = $"%{filter.SearchText}%";
					command.Parameters.Add(parameter);
				}

				if (filter.MinPrice.HasValue)
				{
					sql.Append(" AND daily_price >= @minPrice");
					DbParameter parameter = command.CreateParameter();
					parameter.ParameterName = "@minPrice";
					parameter.Value = filter.MinPrice.Value;
					command.Parameters.Add(parameter);
				}

				if (filter.MaxPrice.HasValue)
				{
					sql.Append(" AND daily_price <= @maxPrice");
					DbParameter parameter = command.CreateParameter();
					parameter.ParameterName = "@maxPrice";
					parameter.Value = filter.MaxPrice.Value;
					command.Parameters.Add(parameter);
				}

				command.CommandText = sql.ToString();
				DbDataReader reader = command.ExecuteReader();
				List<Article> articles = new List<Article>();

				while (reader.Read())
				{
					ArticleDTO articleDTO = CreateDTO(reader);
					articleDTOs.Add(articleDTO);
				}

				reader.Close();
				_db.Close();
				return articleDTOs;
			}
			finally
			{
				_db.Close();
			}
		}





		ArticleDTO CreateDTO(DbDataReader reader)
		{
			int id = reader.GetOrdinal("id");
			int name = reader.GetOrdinal("name");
			int description = reader.GetOrdinal("description");

			int category = reader.GetOrdinal("category_id");
			int subcategory = reader.GetOrdinal("subcategory_id");
			int brand = reader.GetOrdinal("brand_id");
			int collection = reader.GetOrdinal("collection_id");
			int color = reader.GetOrdinal("color_id");
			int size = reader.GetOrdinal("size_id");
			int owner = reader.GetOrdinal("owner_id");

			int daily = reader.GetOrdinal("daily_price");
			int original = reader.GetOrdinal("original_price");

			int rented = reader.GetOrdinal("is_rented");
			int smoked = reader.GetOrdinal("is_smoked");
			int animal = reader.GetOrdinal("is_animal");
			int clean = reader.GetOrdinal("is_clean");

			int created = reader.GetOrdinal("created_at");
			int updated = reader.GetOrdinal("updated_at");

			ArticleDTO dto = new ArticleDTO();

			dto.Id = reader.GetInt32(id);
			dto.Title = reader.GetString(name);

			if (!reader.IsDBNull(description))
			{
				dto.Description = reader.GetString(description);
			}
			else
			{
				dto.Description = string.Empty;
			}

			if (!reader.IsDBNull(category))
			{
				dto.CategoryId = reader.GetInt32(category);
			}

			if (!reader.IsDBNull(subcategory))
			{
				dto.SubcategoryId = reader.GetInt32(subcategory);
			}

			if (!reader.IsDBNull(brand))
			{
				dto.BrandId = reader.GetInt32(brand);
			}

			if (!reader.IsDBNull(collection))
			{
				dto.CollectionId = reader.GetInt32(collection);
			}

			if (!reader.IsDBNull(color))
			{
				dto.ColorId = reader.GetInt32(color);
			}

			if (!reader.IsDBNull(size))
			{
				dto.SizeId = reader.GetInt32(size);
			}

			if (!reader.IsDBNull(daily))
			{
				Debug.WriteLine($"DTO DailyPrice: {dto.DailyPrice}");
				Debug.WriteLine($"reader DailyPrice: {reader.GetFloat(daily)}");
				dto.DailyPrice = reader.GetFloat(daily);
			}
			else
			{
				dto.DailyPrice = 0f;
			}

			if (!reader.IsDBNull(original))
			{
				dto.OriginalPrice = reader.GetFloat(original);
			}
			else
			{
				dto.OriginalPrice = 0f;
			}

			dto.IsRented = reader.GetInt32(rented) != 0;
			dto.IsSmoked = reader.GetInt32(smoked) != 0;
			dto.IsAnimal = reader.GetInt32(animal) != 0;
			dto.IsClean = reader.GetInt32(clean) != 0;

			if (!reader.IsDBNull(owner))
			{
				dto.OwnerId = reader.GetInt32(owner);
			}

			dto.CreatedAt = DateTime.Parse(reader.GetString(created));
			dto.UpdatedAt = DateTime.Parse(reader.GetString(updated));

			return dto;
		}
	}
}
