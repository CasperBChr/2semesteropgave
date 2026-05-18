using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Collection
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Brand Brand { get; set; }
        public Designer? Designer { get; set; }
        public List<Article> Articles { get; set; }

        public Collection(string name, string description, Brand brand, Designer? designer, List<Article> articles)
        {
            Name = name;
            Description = description;
            Brand = brand;
            Designer = designer;
            Articles = articles;
        }
    }
}
