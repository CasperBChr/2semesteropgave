using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Category // Klasse til at repræsentere en kategori, som kan indeholde flere underkategorier
    {
        public string Name { get; set; } // Property: gemmer navnet på kategorien som tekst
        public List<SubCategory> SubCategories { get; set; } // Property: gemmer en liste af underkategorier, som tilhører denne kategori
        public Category() // Constructor: initialiserer en ny instans af Category-klassen uden at sætte nogen værdier
        {
            Name = string.Empty;
            SubCategories = new List<SubCategory>(); // Sætter Name til en tom streng og initialiserer SubCategories som en tom liste
        }
        public Category(string name) // Constructor: initialiserer en ny instans af Category-klassen med et navn og en tom liste af underkategorier
        {
            Name = name;
            SubCategories = new List<SubCategory>();
        }
        public Category(string name, List<SubCategory> subCategories) // Constructor: initialiserer en ny instans af Category-klassen med et navn og en liste af underkategorier
        {
        {
            Name = name;
            SubCategories = subCategories;
        }
    }
}
