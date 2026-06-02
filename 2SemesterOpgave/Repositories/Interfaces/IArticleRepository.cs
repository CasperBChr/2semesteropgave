using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Repositories.Interfaces
{
	public interface IArticleRepository
	{
		IEnumerable<ArticleDTO> GetAllArticles();
	}
}
