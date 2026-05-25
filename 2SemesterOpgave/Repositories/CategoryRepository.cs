using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
            return new List<Category>();
        }
    }
}
