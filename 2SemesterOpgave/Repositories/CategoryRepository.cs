using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		Database _db;	


		public CategoryRepository(Database db)
		{
			_db = db;
		}

		public IEnumerable<Category> GetAllCategories() 
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT c.id as CategoryId, c.name as CategoryName, s.id as SubId, s.name as SubName FROM Categories c LEFT JOIN Subcategories s ON s.category_id = c.id ORDER BY c.id";
			Dictionary<int, Category> categories = new Dictionary<int, Category>();
			using DbDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));

				//if (!categories.ContainsKey(categoryId))
				//{
				//	categories[categoryId] = new Category(reader.GetString(reader.GetOrdinal("CategoryName")));
				//}

				if (!categories.ContainsKey(categoryId))
				{
					categories[categoryId] = new Category
					{
						Id = categoryId,
						Name = reader.GetString(reader.GetOrdinal("CategoryName"))
					};
				}

				categories[categoryId].SubCategories.Add(new SubCategory
				{
					Id = reader.GetInt32(reader.GetOrdinal("SubId")),
					Name = reader.GetString(reader.GetOrdinal("SubName")),
					Category = categories[categoryId]
				});

				//if (!reader.IsDBNull(reader.GetOrdinal("SubId")))
				//{
				//	categories[categoryId].SubCategories.Add(new SubCategory(reader.GetString(reader.GetOrdinal("SubName")), categories[categoryId]));
				//}
			}
			return categories.Values;
		}
	}
}
