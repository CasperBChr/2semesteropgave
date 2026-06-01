using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;

namespace _2SemesterOpgave.Services
{
	public class ArticleServices
	{
		ArticleRepository _articleRepository;
		BrandServices _brandServices;
		public Article? SelectedArticle { get; set; }

		public ArticleServices(ArticleRepository articleRepository, BrandServices brandService) 
		{
			_articleRepository = articleRepository;
			_brandServices = brandService;
		}

		Article MapArticle(DbDataReader reader)
		{
			return new Article(
				title: reader.GetString(reader.GetOrdinal("name")),
				description: reader.GetString(reader.GetOrdinal("description")),
				dailyPrice: reader.GetFloat(reader.GetOrdinal("daily_price")),
				originalPrice: reader.GetFloat(reader.GetOrdinal("original_price")),
				isRented: reader.GetBoolean(reader.GetOrdinal("is_rented")),
				isSmoked: reader.GetBoolean(reader.GetOrdinal("is_smoked")),
				isAnimal: reader.GetBoolean(reader.GetOrdinal("is_animal")),
				isClean: reader.GetBoolean(reader.GetOrdinal("is_clean"))
			);
		}


		public ObservableCollection<Article> GetAllArticles()
		{
			IEnumerable<Article> articles = _articleRepository.GetAllArticles();
			ObservableCollection<Article> uiArticles = new ObservableCollection<Article>();
			foreach (Article article in articles)
			{
				uiArticles.Add(article);
			}
			return uiArticles;
		}

        public ObservableCollection<Article> GetNewestArticles()
        {
            IEnumerable<Article> articles = _articleRepository.GetNewestArticles();
            ObservableCollection<Article> uiArticles = new ObservableCollection<Article>();
            foreach (Article article in articles)
            {
                uiArticles.Add(article);
            }
            return uiArticles;
        }



        public ObservableCollection<Article> GetFilteredArticles(FilterCriteria filter)
		{
			IEnumerable<Article> articles = _articleRepository.GetFilteredArticles(filter);
			ObservableCollection<Article> uiArticles = new ObservableCollection<Article>();
			foreach (Article a in articles)
			{
				uiArticles.Add(a);
			}
			return uiArticles;
		}

	}
}
