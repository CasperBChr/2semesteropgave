using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Xml.Linq;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til BrandDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
	// Repositoryklasse der håndterer databasekald for brands
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	public class BrandRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public BrandRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Henter alle brands fra databasen
        public IEnumerable<BrandDTO> GetAll()
        {
            // Opretter en liste til brand-DTO'er
            List<BrandDTO> brands = new List<BrandDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle brands
            command.CommandText = "SELECT * FROM Brands";

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en BrandDTO
                BrandDTO dto = CreateDTO(reader);

                // Tilføjer DTO'en til listen
                brands.Add(dto);
            }

            // Returnerer listen med brands
            return brands;
        }

        // Omdanner en databaserække til en BrandDTO
        BrandDTO CreateDTO(IDataReader reader)
        {
            // Finder placeringen af id-kolonnen
            int id = reader.GetOrdinal("id");

            // Finder placeringen af name-kolonnen
            int name = reader.GetOrdinal("name");

            // Finder placeringen af description-kolonnen
            int description = reader.GetOrdinal("name");        ///////////// Burde være "description" i stedet for "name", men den crasher efter login /////////////

            // Finder placeringen af logopath-kolonnen
            int logo = reader.GetOrdinal("logopath");

            // Finder placeringen af created_at-kolonnen
            int created = reader.GetOrdinal("created_at");

            // Finder placeringen af updated_at-kolonnen
            int updated = reader.GetOrdinal("updated_at");

            // Variabel til logo-stien
            string? logoString;

            // Tjekker om logo-feltet er null i databasen
            if (reader.IsDBNull(logo))
            {
                // Sætter logoString til null, hvis der ikke findes et logo
                logoString = null;
            }
            else
            {
                // Henter logo-stien fra databasen
                logoString = reader.GetString(logo);
            }

            // Opretter og returnerer en BrandDTO med data fra databasen
            return new BrandDTO
            {
                // Sætter brandets id
                Id = reader.GetInt32(id),

                // Sætter brandets navn
                Name = reader.GetString(name),

                // Sætter brandets beskrivelse
                Description = reader.GetString(description),

                // Sætter sti til logo
                LogoPath = logoString,

                // Konverterer created_at fra tekst til DateTime
                CreatedAt = DateTime.Parse(reader.GetString(created)),

                // Konverterer updated_at fra tekst til DateTime
                UpdatedAt = DateTime.Parse(reader.GetString(updated))
            };
        }
    }
}