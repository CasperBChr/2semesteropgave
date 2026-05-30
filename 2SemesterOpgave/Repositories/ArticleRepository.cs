using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave;

namespace _2SemesterOpgave.Repositories
{
	public class ArticleRepository : IArticleRepository
	{
		Database _db;
		public ArticleRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<Article> GetAllArticles() 
		{
			_db.Open();
			DbCommand command = _db.Connection.CreateCommand();
			command.CommandText = "SELECT * FROM Articles";
			List<Article> articles = new List<Article>();
			DbDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				articles.Add(new Article(title: reader.GetString(reader.GetOrdinal("name")), description: reader.GetString(reader.GetOrdinal("description")), category: reader.GetOrdinal("category"), subcategory: reader.GetOrdinal("subcategory"), size: reader.GetOrdinal("size"), originalPrice: 20000.0f, dailyPrice: 150.0f, color: reader.GetOrdinal("color"), brand: reader.GetOrdinal("brand"), isRented: true, isSmoked: true, isAnimal: true, isClean: true, owner: reader.GetOrdinal("owner_id")));
				//users.Add(new User(username: reader["Username"].ToString(), email: reader["Email"].ToString(), password: reader["Password"].ToString(), id: Convert.ToInt32(reader["ID"])));
			}

			reader.Close();
			_db.Close();
			return articles;
		}


        public IEnumerable<Article> GetNewestArticles()
        {
            _db.Open();
            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText = "SELECT * FROM Articles ORDER BY created_at DESC LIMIT 10";
            List<Article> articles = new List<Article>();
            DbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                articles.Add(new Article(title: reader.GetString(reader.GetOrdinal("name")), description: reader.GetString(reader.GetOrdinal("description")), originalPrice: 20000.0f, dailyPrice: 150.0f, isRented: true, isSmoked: true, isAnimal: true, isClean: true));
                //users.Add(new User(username: reader["Username"].ToString(), email: reader["Email"].ToString(), password: reader["Password"].ToString(), id: Convert.ToInt32(reader["ID"])));
            }

            reader.Close();
            _db.Close();
            return articles;
        }


        public IEnumerable<Article> GetFilteredArticles(FilterCriteria filter)
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
				articles.Add(new Article(title: reader.GetString(reader.GetOrdinal("name")), description: reader.GetString(reader.GetOrdinal("description")), originalPrice: 20000.0f, dailyPrice: 150.0f, isRented: true, isSmoked: true, isAnimal: true, isClean: true));
				//users.Add(new User(username: reader["Username"].ToString(), email: reader["Email"].ToString(), password: reader["Password"].ToString(), id: Convert.ToInt32(reader["ID"])));
			}

			reader.Close();
			_db.Close();
			return articles;
		}

	}
}
