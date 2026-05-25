using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		public Article SelectedArticle { get; set; }

		public ArticleServices(Database db) 
		{
			_articleRepository = new ArticleRepository(db);
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
