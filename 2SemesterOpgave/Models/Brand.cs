using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Brand // Klasse til at repræsentere et brand, som kan være tilknyttet en kollektion
    {
        public string Name { get; set; } // Property: gemmer navnet på brandet, som kan bruges til at identificere det og vise det til brugerne   
        public string Description { get; set; } // Property: gemmer en beskrivelse af brandet, som kan bruges til at give brugerne mere information om brandet
        public string LogoPath { get; set; } // Property
        public Brand(string name, string description, string logopath) // Constructor: initialiserer en ny instans af Brand-klassen med et navn, en beskrivelse og en sti til logoet
        {
            Name = name;
            Description = description;
            LogoPath = logopath;
        }

    }
}
