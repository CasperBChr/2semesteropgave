using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	// DTO-klasse der bruges til at transportere brand-data fra databasen
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class BrandDTO
    {
        // Brandets id i databasen
        public int Id { get; set; }

        // Brandets navn
        public string Name { get; set; } = string.Empty;

        // Brandets beskrivelse
        public string Description { get; set; } = string.Empty;

        // Sti til brandets logo, hvis der findes et
        public string? LogoPath { get; set; }

        // Dato og tidspunkt for hvornår brandet blev oprettet
        public DateTime CreatedAt { get; set; }

        // Dato og tidspunkt for hvornår brandet sidst blev opdateret
        public DateTime UpdatedAt { get; set; }
    }
}