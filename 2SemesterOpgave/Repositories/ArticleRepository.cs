using _2SemesterOpgave;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;
using _2SemesterOpgave.Services;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;

namespace _2SemesterOpgave.Repositories
{
    
    public class ArticleRepository
	{
		IDatabaseFactory _db;
		public ArticleRepository(IDatabaseFactory db)
		{
			_db = db;
		}

		public void DeleteArticle(Article article)
		{
			try
			{
				using IDbConnection connection = _db.CreateConnection();
				using IDbCommand command = connection.CreateCommand();

				command.CommandText = "DELETE FROM Articles WHERE id = @ArticleId;";

				IDbDataParameter articleParam = command.CreateParameter();
				articleParam.ParameterName = "@ArticleId";
				articleParam.Value = article.Id;
				command.Parameters.Add(articleParam);

				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		public void UpdateArticle(Article article) 
		{
			try {
				using IDbConnection connection = _db.CreateConnection();
				using IDbCommand command = connection.CreateCommand();

				command.CommandText = @"
					UPDATE Articles SET
					name = @name, description = @description, category_id = @categoryId, subcategory_id = @subcategoryId, brand_id = @brandId, color_id = @colorId, size_id = @sizeId, daily_price = @dailyPrice, original_price = @originalPrice, is_rented = @isRented 
					WHERE id = @ArticleId;";

				IDbDataParameter articleParam = command.CreateParameter();
				articleParam.ParameterName = "@ArticleId";
				articleParam.Value = article.Id;
				command.Parameters.Add(articleParam);

				IDbDataParameter nameParam = command.CreateParameter();
				nameParam.ParameterName = "@name";
				nameParam.Value = article.Title;
				command.Parameters.Add(nameParam);

				IDbDataParameter descriptionParam = command.CreateParameter();
				descriptionParam.ParameterName = "@description";
				descriptionParam.Value = article.Description;
				command.Parameters.Add(descriptionParam);

				IDbDataParameter categoryParam = command.CreateParameter();
				categoryParam.ParameterName = "@categoryId";
				categoryParam.Value = article.Category.Id;
				command.Parameters.Add(categoryParam);

				IDbDataParameter subcategoryParam = command.CreateParameter();
				subcategoryParam.ParameterName = "@subcategoryId";
				subcategoryParam.Value = article.SubCategory.Id;
				command.Parameters.Add(subcategoryParam);

				IDbDataParameter brandParam = command.CreateParameter();
				brandParam.ParameterName = "@brandId";
				brandParam.Value = article.Brand.Id;
				command.Parameters.Add(brandParam);

				IDbDataParameter colorParam = command.CreateParameter();
				colorParam.ParameterName = "@colorId";
				colorParam.Value = article.Color.Id;
				command.Parameters.Add(colorParam);

				IDbDataParameter sizeParam = command.CreateParameter();
				sizeParam.ParameterName = "@sizeId";
				sizeParam.Value = article.Size.Id;
				command.Parameters.Add(sizeParam);

				IDbDataParameter dailyPriceParam = command.CreateParameter();
				dailyPriceParam.ParameterName = "@dailyPrice";
				dailyPriceParam.Value = article.DailyPrice;
				command.Parameters.Add(dailyPriceParam);

				IDbDataParameter originalPriceParam = command.CreateParameter();
				originalPriceParam.ParameterName = "@originalPrice";
				originalPriceParam.Value = article.OriginalPrice;
				command.Parameters.Add(originalPriceParam);

				IDbDataParameter isRentedParam = command.CreateParameter();
				isRentedParam.ParameterName = "@isRented";
				isRentedParam.Value = article.IsRented;
				command.Parameters.Add(isRentedParam);

				command.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		public IEnumerable<ArticleDTO> GetAllArticles() 
		{
			List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Articles";
			using IDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				ArticleDTO articleDTO = CreateDTO(reader);
				articleDTOs.Add(articleDTO);
			}

			reader.Close();
			return articleDTOs;
		}


        public IEnumerable<ArticleDTO> GetNewestArticles()
        {
			List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Articles ORDER BY created_at DESC LIMIT 10";
			List<Article> articles = new List<Article>();
			using IDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				ArticleDTO articleDTO = CreateDTO(reader);
				articleDTOs.Add(articleDTO);
			}

			reader.Close();
			return articleDTOs;
        }

		public IEnumerable<ArticleDTO> GetAllArticlesByOwner(int ownerId)
		{
			List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Articles WHERE owner_id = @OwnerId";

			IDbDataParameter ownerParameter = command.CreateParameter();
			ownerParameter.ParameterName = "@OwnerId";
			ownerParameter.DbType = DbType.Int32;
			ownerParameter.Value = ownerId;
			command.Parameters.Add(ownerParameter);

			using IDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				ArticleDTO articleDTO = CreateDTO(reader);
				articleDTOs.Add(articleDTO);
			}

			reader.Close();
			return articleDTOs;
		}

		public IEnumerable<ArticleDTO> GetAllFavoritedArticlesByUser(int userId)
		{
			List<ArticleDTO> articleDTOs = new();

			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = @"
				SELECT a.*
				FROM Articles a
				INNER JOIN UserFavorites uf
					ON uf.article_id = a.id
				WHERE uf.user_id = @UserId";

			IDbDataParameter parameter = command.CreateParameter();
			parameter.ParameterName = "@UserId";
			parameter.DbType = DbType.Int32;
			parameter.Value = userId;

			command.Parameters.Add(parameter);

			using IDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				articleDTOs.Add(CreateDTO(reader));
			}

			return articleDTOs;
		}

		public void AddFavorite(int userId, int articleId)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = @"
				INSERT INTO UserFavorites(user_id, article_id)
				VALUES(@UserId, @ArticleId)";

			IDbDataParameter userParam = command.CreateParameter();
			userParam.ParameterName = "@UserId";
			userParam.Value = userId;
			command.Parameters.Add(userParam);

			IDbDataParameter articleParam = command.CreateParameter();
			articleParam.ParameterName = "@ArticleId";
			articleParam.Value = articleId;
			command.Parameters.Add(articleParam);

			command.ExecuteNonQuery();
		}

		public void RemoveFavorite(int userId, int articleId)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = @"
				DELETE FROM UserFavorites
				WHERE user_id = @UserId
				AND article_id = @ArticleId";

			IDbDataParameter userParam = command.CreateParameter();
			userParam.ParameterName = "@UserId";
			userParam.Value = userId;
			command.Parameters.Add(userParam);

			IDbDataParameter articleParam = command.CreateParameter();
			articleParam.ParameterName = "@ArticleId";
			articleParam.Value = articleId;
			command.Parameters.Add(articleParam);

			command.ExecuteNonQuery();
		}

		public bool IsFavorite(int userId, int articleId)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = @"
				SELECT COUNT(*)
				FROM UserFavorites
				WHERE user_id = @UserId
				AND article_id = @ArticleId";

			IDbDataParameter userParam = command.CreateParameter();
			userParam.ParameterName = "@UserId";
			userParam.Value = userId;
			command.Parameters.Add(userParam);

			IDbDataParameter articleParam = command.CreateParameter();
			articleParam.ParameterName = "@ArticleId";
			articleParam.Value = articleId;
			command.Parameters.Add(articleParam);

			return Convert.ToInt32(command.ExecuteScalar()) > 0;
		}


		public IEnumerable<ArticleDTO> GetFilteredArticles(FilterCriteria filter)
		{
			List<ArticleDTO> articleDTOs = new List<ArticleDTO>();
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			StringBuilder sql = new StringBuilder("SELECT * FROM Articles WHERE 1=1");

				if (!string.IsNullOrWhiteSpace(filter.SearchText))
				{
					sql.Append(" AND (name LIKE @search OR description LIKE @search)");
					IDbDataParameter parameter = command.CreateParameter();
					parameter.ParameterName = "@search";
					parameter.Value = $"%{filter.SearchText}%";
					command.Parameters.Add(parameter);
				}

				command.CommandText = sql.ToString();
				IDataReader reader = command.ExecuteReader();
				List<Article> articles = new List<Article>();

				while (reader.Read())
				{
					ArticleDTO articleDTO = CreateDTO(reader);
					articleDTOs.Add(articleDTO);
				}
				return articleDTOs;

		}

        public void CreateArticle(Article article, User owner)
        {
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = "INSERT INTO Articles (name, description, category_id, subcategory_id, brand_id, color_id, size_id, daily_price, original_price, is_rented, owner_id) VALUES (@name, @description, @categoryId, @subcategoryId, @brandId, @colorId, @sizeId, @dailyPrice, @originalPrice, @isRented, @ownerId)";

			IDbDataParameter nameParam = command.CreateParameter();
            nameParam.ParameterName = "@name";
            nameParam.DbType = DbType.String;
            nameParam.Value = article.Title;
            command.Parameters.Add(nameParam);

            IDbDataParameter descriptionParam = command.CreateParameter();
            descriptionParam.DbType = DbType.String;
            descriptionParam.ParameterName = "@description";
            descriptionParam.Value = article.Description;
            command.Parameters.Add(descriptionParam);

            IDbDataParameter categoryIdParam = command.CreateParameter();
            categoryIdParam.DbType = DbType.Int32;
            categoryIdParam.ParameterName = "@categoryId";
            categoryIdParam.Value = article.Category.Id;
            command.Parameters.Add(categoryIdParam);

            IDbDataParameter subcategoryIdParam = command.CreateParameter();
            subcategoryIdParam.DbType = DbType.Int32;
            subcategoryIdParam.ParameterName = "@subcategoryId";
            subcategoryIdParam.Value = article.SubCategory.Id;
            command.Parameters.Add(subcategoryIdParam);

            IDbDataParameter brandIdParam = command.CreateParameter();
            brandIdParam.DbType = DbType.Int32;
            brandIdParam.ParameterName = "@brandId";
            brandIdParam.Value = article.Brand.Id;
            command.Parameters.Add(brandIdParam);

            IDbDataParameter colorIdParam = command.CreateParameter();
            colorIdParam.DbType = DbType.Int32;
            colorIdParam.ParameterName = "@colorId";
            colorIdParam.Value = article.Color.Id;
            command.Parameters.Add(colorIdParam);

            IDbDataParameter sizeIdParam = command.CreateParameter();
            sizeIdParam.DbType = DbType.Int32;
            sizeIdParam.ParameterName = "@sizeId";
            sizeIdParam.Value = article.Size.Id;
            command.Parameters.Add(sizeIdParam);

            IDbDataParameter dailyPriceParam = command.CreateParameter();
            dailyPriceParam.DbType = DbType.Single;
            dailyPriceParam.ParameterName = "@dailyPrice";
            dailyPriceParam.Value = article.DailyPrice;
            command.Parameters.Add(dailyPriceParam);

            IDbDataParameter originalPriceParam = command.CreateParameter();
            originalPriceParam.DbType = DbType.Single;
            originalPriceParam.ParameterName = "@originalPrice";
            originalPriceParam.Value = article.OriginalPrice;
            command.Parameters.Add(originalPriceParam);

            IDbDataParameter isRentedParam = command.CreateParameter();
            isRentedParam.DbType = DbType.Int32;
            isRentedParam.ParameterName = "@isRented";
            isRentedParam.Value = article.IsRented ? 1 : 0;
            command.Parameters.Add(isRentedParam);
            
			IDbDataParameter ownerParam = command.CreateParameter();
            ownerParam.DbType = DbType.Int32;
            ownerParam.ParameterName = "@ownerId";
            ownerParam.Value = owner.Id;
            command.Parameters.Add(ownerParam);

            command.ExecuteNonQuery();
        }


        ArticleDTO CreateDTO(IDataReader reader)
		{
			int id = reader.GetOrdinal("id");
			int name = reader.GetOrdinal("name");
			int description = reader.GetOrdinal("description");

			int category = reader.GetOrdinal("category_id");
			int subcategory = reader.GetOrdinal("subcategory_id");

			int brand = reader.GetOrdinal("brand_id");

			int color = reader.GetOrdinal("color_id");
			int size = reader.GetOrdinal("size_id");
			int owner = reader.GetOrdinal("owner_id");

			int daily = reader.GetOrdinal("daily_price");
			int original = reader.GetOrdinal("original_price");

			int rented = reader.GetOrdinal("is_rented");
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
