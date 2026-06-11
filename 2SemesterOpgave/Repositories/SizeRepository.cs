using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til SizeDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
	// Repositoryklasse der håndterer databasekald for størrelser
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	public class SizeRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public SizeRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Henter alle størrelser fra databasen
        public IEnumerable<SizeDTO> GetAllSizes()
        {
            // Opretter en liste til SizeDTO'er
            List<SizeDTO> dtos = new List<SizeDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle størrelser
            command.CommandText = "SELECT * FROM Sizes";

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Finder placeringen af id-kolonnen
            int id = reader.GetOrdinal("id");

            // Finder placeringen af name-kolonnen
            int name = reader.GetOrdinal("name");

            // Finder placeringen af created_at-kolonnen
            int created = reader.GetOrdinal("created_at");

            // Finder placeringen af updated_at-kolonnen
            int updated = reader.GetOrdinal("updated_at");

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en SizeDTO og tilføjer den til listen
                dtos.Add(CreateDTO(reader, id, name, created, updated));
            }

            // Returnerer listen med størrelser
            return dtos;
        }

        // Omdanner en databaserække til en SizeDTO
        SizeDTO CreateDTO(IDataReader reader, int id, int name, int created, int updated)
        {
            // Opretter og returnerer en SizeDTO med data fra databasen
            return new SizeDTO
            {
                // Sætter størrelsens id
                Id = reader.GetInt32(id),

                // Sætter størrelsens navn
                Name = reader.GetString(name),

                // Konverterer created_at fra tekst til DateTime
                CreatedAt = DateTime.Parse(reader.GetString(created)),

                // Konverterer updated_at fra tekst til DateTime
                UpdatedAt = DateTime.Parse(reader.GetString(updated))
            };
        }
    }
}