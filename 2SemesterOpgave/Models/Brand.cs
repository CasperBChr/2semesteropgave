using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Brand // Klasse til at repræsentere et brand, som kan være tilknyttet en kollektion
    {
		public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Property: gemmer navnet på brandet, som kan bruges til at identificere det og vise det til brugerne   
        public string Description { get; set; } = string.Empty; // Property: gemmer en beskrivelse af brandet, som kan bruges til at give brugerne mere information om brandet
        public string LogoPath { get; set; } = string.Empty; // Property
        public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		//public Brand(string name, string description, string logopath) // Constructor: initialiserer en ny instans af Brand-klassen med et navn, en beskrivelse og en sti til logoet
		//{
		//    Name = name;
		//    Description = description;
		//    LogoPath = logopath;
		//}

	}
}
