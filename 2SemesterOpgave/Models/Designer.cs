using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Designer // Klasse til at repræsentere en designer, som kan være tilknyttet en kollektion
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
