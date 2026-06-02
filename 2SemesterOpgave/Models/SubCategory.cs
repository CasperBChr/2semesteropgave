using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class SubCategory // Klasse til at repræsentere en underkategori, som tilhører en kategori
    {
		public int Id { get; set; }
		public string Name { get; set; } // Property: gemmer navnet på underkategorien som tekst
        public Category Category { get; set; } // Property: gemmer en reference til den Category, som underkategorien tilhører, så underkategorien ved, hvilken kategori den er en del af
        //public SubCategory(string name, Category category) // Constructor: initialiserer en ny instans af SubCategory-klassen med et navn og en reference til den kategori, som underkategorien tilhører
        //{
        //    Name = name;
        //    Category = category;
        //}
    }
    
}
