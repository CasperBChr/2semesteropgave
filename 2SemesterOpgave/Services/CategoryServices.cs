using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.Interfaces;

namespace _2SemesterOpgave.Services
{
	public class CategoryServices
	{
		ICategoryRepository _categoryRepository;

		public CategoryServices(Database db) 
		{
			_categoryRepository = new CategoryRepository(db);
		}

		public ObservableCollection<Category> GetAllCategories() 
		{
			IEnumerable<Category> categories = _categoryRepository.GetAllCategories();
			ObservableCollection<Category> uiCategories = new ObservableCollection<Category>();
			foreach(Category category in categories)  
			{
				uiCategories.Add(category);
			}
			return uiCategories;
		}

	}
}
