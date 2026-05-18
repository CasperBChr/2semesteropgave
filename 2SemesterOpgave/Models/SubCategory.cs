using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class SubCategory
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public SubCategory(string name, Category category)
        {
            Name = name;
            Category = category;
        }
    }
    
}
