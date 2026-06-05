using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Algoritme;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave.Services.Interfaces;

namespace _2SemesterOpgave.Services
{
	public class ArticleServices
	{
		ArticleRepository _articleRepository;
		UserServices _userServices;
		BrandServices _brandServices;
		CategoryServices _categoryServices;
		CollectionServices _collectionServices;
		ColorServices _colorServices;
		SizeServices _sizeServices;
		public Article? SelectedArticle { get; set; }

		public ArticleServices(ArticleRepository articleRepository, UserServices userService, BrandServices brandService, CategoryServices categoryServices, CollectionServices collectionServices, ColorServices colorServices, SizeServices sizeServices) 
		{
			_articleRepository = articleRepository;
			_brandServices = brandService;
			_categoryServices = categoryServices;
			_collectionServices = collectionServices;
			_colorServices = colorServices;
			_sizeServices = sizeServices;
			_userServices = userService;
		}

		//Article MapArticle(DbDataReader reader)
		//{
		//	return new Article(
		//		title: reader.GetString(reader.GetOrdinal("name")),
		//		description: reader.GetString(reader.GetOrdinal("description")),
		//		dailyPrice: reader.GetFloat(reader.GetOrdinal("daily_price")),
		//		originalPrice: reader.GetFloat(reader.GetOrdinal("original_price")),
		//		isRented: reader.GetBoolean(reader.GetOrdinal("is_rented")),
		//		isSmoked: reader.GetBoolean(reader.GetOrdinal("is_smoked")),
		//		isAnimal: reader.GetBoolean(reader.GetOrdinal("is_animal")),
		//		isClean: reader.GetBoolean(reader.GetOrdinal("is_clean"))
		//	);
		//}


		public IEnumerable<Article> GetAllArticles()
		{
			IEnumerable<ArticleDTO> dtos = _articleRepository.GetAllArticles();
			ObservableCollection<Article> articles = new ObservableCollection<Article>();
			foreach (ArticleDTO dto in dtos)
			{
				articles.Add(MapToArticle(dto));
			}
			return articles;
		}

		public IEnumerable<Article> GetNewestArticles()
		{
			IEnumerable<ArticleDTO> dtos = _articleRepository.GetNewestArticles();
			ObservableCollection<Article> articles = new ObservableCollection<Article>();
			foreach (ArticleDTO dto in dtos)
			{
				articles.Add(MapToArticle(dto));
			}
			return articles;
		}

		public IEnumerable<Article> GetRandomArticles(int amount)
		{
			List<Article> articles = new List<Article>(GetAllArticles());
			HashSet<Article> randomArticles = new HashSet<Article>();
			Random random = new Random();
			if(articles.Count < amount)
			{
				for (int i = 0; i < amount; i++)
				{
					randomArticles.Add(articles[random.Next(0, articles.Count)]);
				}
			}
			else
			{
				for(int i = 0; randomArticles.Count < amount; i++)
				{
					randomArticles.Add(articles[random.Next(0, articles.Count)]);
				}
			}
			return randomArticles;
		}

		public IEnumerable<Article> GetFilteredArticles(FilterCriteria filter)
		{
			IEnumerable<ArticleDTO> dtos = _articleRepository.GetFilteredArticles(filter);
			ObservableCollection<Article> articles = new ObservableCollection<Article>();
			foreach (ArticleDTO dto in dtos)
			{
				articles.Add(MapToArticle(dto));
			}
			return articles;
		}

		public IEnumerable<Article> GetArticlesByCategory(int categoryId)
		{
			IEnumerable<Article> allArticles = GetAllArticles();
			List<Article> filteredArticles = new List<Article>();
			foreach (Article article in allArticles)
			{
				if (article.Category != null && article.Category.Id == categoryId)
				{
					filteredArticles.Add(article);
				}
			}
			return filteredArticles;
		}
		
		public IEnumerable<Article> GetArticlesByOwner(int ownerId)
		{
			IEnumerable<Article> allArticles = GetAllArticles();
			List<Article> filteredArticles = new List<Article>();
			foreach (Article article in allArticles)
			{
				if (article.Owner != null && article.Owner.Id == ownerId)
				{
					filteredArticles.Add(article);
				}
			}
			return filteredArticles;
		}


		Article MapToArticle(ArticleDTO dto)
		{
			Article article = new Article
			{
				Id = dto.Id,
				Title = dto.Title,
				Description = dto.Description,
				DailyPrice = dto.DailyPrice,
				OriginalPrice = dto.OriginalPrice,

				IsRented = dto.IsRented,
				IsSmoked = dto.IsSmoked,
				IsAnimal = dto.IsAnimal,
				IsClean = dto.IsClean,

				CreatedAt = dto.CreatedAt,
				UpdatedAt = dto.UpdatedAt
			};

			if (dto.BrandId.HasValue)
			{
				article.Brand = _brandServices.GetById(dto.BrandId.Value);
			}

			if (dto.CategoryId.HasValue)
			{
				article.Category = _categoryServices.GetCategoryById(dto.CategoryId.Value);
			}

			if (dto.SubcategoryId.HasValue)
			{
				article.SubCategory = _categoryServices.GetSubCategoryById(dto.SubcategoryId.Value);
			}

			if (dto.CollectionId.HasValue)
			{
				article.collection = _collectionServices.GetById(dto.CollectionId.Value);
			}

			if (dto.ColorId.HasValue)
			{
				article.Color = _colorServices.GetNameById(dto.ColorId.Value);
			}

			if (dto.SizeId.HasValue)
			{
				article.Size = _sizeServices.GetById(dto.SizeId.Value);
			}

			if (dto.OwnerId.HasValue)
			{
				article.Owner = _userServices.GetById(dto.OwnerId.Value);
			}

			// SKAL BRUGES TIL CONTENT BASES ALGORITHM, FOR AT ALLE FÅR DET PÅ!
			if (article.Category != null)
			{
				article.ItemProfile = new ItemProfile
				{
					ArticleID = article.Id,
					Article = article,
					Features = new Dictionary<string, double>
					{
						{ article.Category.Name, 1.0 }
					}
				};
			}

			return article;
		}
	}
}
