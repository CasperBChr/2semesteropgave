using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
    // DTO-klasse der bruges til at transportere forsikringsmulighed-data fra databasen
    public class InsuranceOptionDTO
    {
        // Forsikringsmulighedens id i databasen
        public int Id { get; set; }

        // Forsikringsmulighedens navn
        public string Name { get; set; } = string.Empty;

        // Grundprisen for forsikringen
        public float BaseFees { get; set; }
    }
}