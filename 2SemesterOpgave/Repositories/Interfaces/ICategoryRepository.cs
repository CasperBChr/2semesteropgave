using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Repositories.Interfaces
{
	public interface ICategoryRepository
	{
		//Category? GetCategoryByID(int id);

		IEnumerable<Category> GetAllCategories();

		//void AddCategory(User user);

		//void UpdateCategory(User user);

		//void DeleteCategory(int id);
	}
}
