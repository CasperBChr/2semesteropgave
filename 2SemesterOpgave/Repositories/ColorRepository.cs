using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ColorDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
    // Repositoryklasse der håndterer databasekald for farver
    public class ColorRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public ColorRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }


        // Henter alle farver fra databasen
        public IEnumerable<ColorDTO> GetAllColors()
        {
            // Opretter en liste til farve-DTO'er
            List<ColorDTO> colors = new List<ColorDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle farver
            command.CommandText = "SELECT * FROM Colors";

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ColorDTO
                ColorDTO dto = CreateDTO(reader);

                // Tilføjer DTO'en til listen
                colors.Add(dto);
            }

            // Returnerer listen med farver
            return colors;
        }

        // Omdanner en databaserække til en ColorDTO
        private ColorDTO CreateDTO(IDataReader reader)
        {
            // Finder placeringen af id-kolonnen
            int id = reader.GetOrdinal("id");

            // Finder placeringen af name-kolonnen
            int name = reader.GetOrdinal("name");

            // Opretter en ColorDTO med data fra databasen
            ColorDTO dto = new ColorDTO
            {
                // Sætter farvens id
                Id = reader.GetInt32(id),

                // Sætter farvens navn
                Name = reader.GetString(name)
            };

            // Returnerer den færdige DTO
            return dto;
        }
    }
}