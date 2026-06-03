using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class InsuranceOption
    {
		public int Id { get; set; }
        public string Name { get; set; } = string.Empty; //Property: gemmer navnet på forsikringsmuligheden som tekst
        public float BaseFees { get; set; } // Property: gemmer de grundlæggende omkostninger for forsikringsmuligheden som et flydende tal
        //public InsuranceOption() // Constructor: initialiserer en ny instans af InsuranceOption-klassen med standardværdier
        //{
        //    Name = string.Empty; // Sætter Name til en tom streng
        //    BaseFees = 0; // Sætter BaseFees til 0
        //}
    }
}
