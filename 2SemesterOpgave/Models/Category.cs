using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Category
    {
        public string Name { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public Category()
        {
            Name = string.Empty;
            SubCategories = new List<SubCategory>();
        }
        public Category(string name)
        {
            Name = name;
            SubCategories = new List<SubCategory>();
        }
        public Category(string name, List<SubCategory> subCategories)
        {
            Name = name;
            SubCategories = subCategories;
        }
    }
}
