using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Repositories.Interfaces
{
	public interface IArticleRepository
	{
		IEnumerable<Article> GetAllArticles();
	}
}
