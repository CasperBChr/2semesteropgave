using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
    // DTO-klasse der bruges til at transportere størrelse-data fra databasen
    public class SizeDTO
    {
        // Størrelsens id i databasen
        public int Id { get; set; }

        // Størrelsens navn
        public string Name { get; set; } = string.Empty;

        // Dato og tidspunkt for hvornår størrelsen blev oprettet
        public DateTime CreatedAt { get; set; }

        // Dato og tidspunkt for hvornår størrelsen sidst blev opdateret
        public DateTime UpdatedAt { get; set; }
    }
}