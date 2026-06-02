using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Designer // Klasse til at repræsentere en designer, som kan være tilknyttet en kollektion
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Property: gemmer navnet på designeren som tekst

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		//public Designer(string name) // Constructor: initialiserer en ny instans af Designer-klassen med et navn
		//{
		//    Name = name; // Sætter Name til det angivne navn, når en ny Designer oprettes
		//}
	}
}
