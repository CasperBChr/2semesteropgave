using _2SemesterOpgave.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAllArticles();
    }
}
