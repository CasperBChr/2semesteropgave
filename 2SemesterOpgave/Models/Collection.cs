using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Collection // Klasse til at repræsentere en kollektion, som indeholder flere artikler og er tilknyttet et brand og eventuelt en designer
    {
		public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
		public Brand? Brand { get; set; }
		public Designer? Designer { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		//Constructor 
		//public Collection(string name, string description, Brand brand, Designer? designer, List<Article> articles) // Initialiserer en ny instans af Collection-klassen med de angivne parametre
		//{
		//    Name = name; // Sætter Name til det angivne navn, når en ny Collection oprettes
		//    Description = description; // Sætter Description til den angivne beskrivelse, når en ny Collection oprettes
		//    Brand = brand; // Sætter Brand til det angivne brand, når en ny Collection oprettes
		//    Designer = designer; // Sætter Designer til den angivne designer, når en ny Collection oprettes (kan være null)
		//    Articles = articles; // Sætter Articles til den angivne liste af artikler, når en ny Collection oprettes
		//}
	}
}
