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

		List<Category> _categories;
		Dictionary<int, Category> _categoryLookup = new Dictionary<int, Category>();
		Dictionary<int, SubCategory> _subCategoryLookup = new Dictionary<int, SubCategory>();

		public CategoryServices(CategoryRepository categoryRepository) 
		{
			_categoryRepository = categoryRepository;
			InitializeCache();
		}

		//public IEnumerable<Category> GetAllCategories() 
		//{
		//	return _categoryRepository.GetAllCategories();
		//}

		void InitializeCache()
		{
			IEnumerable<Category> categories = _categoryRepository.GetAllCategories();
			foreach (Category category in categories)
			{
				_categoryLookup[category.Id] = category;
				foreach (SubCategory sub in category.SubCategories)
				{
					_subCategoryLookup[sub.Id] = sub;
				}
			}
		}

		public IEnumerable<Category> GetAllCategories()
		{
			return _categoryLookup.Values;
		}

		public Category? GetCategoryById(int id)
		{
			_categoryLookup.TryGetValue(id, out Category? category);
			return category;
		}

		public SubCategory? GetSubCategoryById(int id)
		{
			_subCategoryLookup.TryGetValue(id, out SubCategory? subCategory);
			return subCategory;
		}

		//public Category? GetCategoryById(int id)
		//{
		//	foreach (Category category in _categories)
		//	{
		//		if (category.Id == id)
		//		{
		//			return category;
		//		}
		//	}
		//	return null;
		//}

		//public SubCategory? GetSubCategoryById(int id)
		//{
		//	foreach (Category category in _categories)
		//	{
		//		foreach (SubCategory subCategory in category.SubCategories)
		//		{
		//			if (subCategory.Id == id)
		//			{
		//				return subCategory;
		//			}
		//		}
		//	}
		//	return null;
		//}
	}
}
