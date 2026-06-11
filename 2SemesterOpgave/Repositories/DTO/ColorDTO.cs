using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	// DTO-klasse der bruges til at transportere farve-data fra databasen
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class ColorDTO
    {
        // Farvens id i databasen
        public int Id { get; set; }

        // Farvens navn
        public string Name { get; set; } = string.Empty;
    }
}
