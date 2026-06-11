using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til InsuranceOptionDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
	// Repositoryklasse der håndterer databasekald for forsikringsmuligheder
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	public class InsuranceOptionRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public InsuranceOptionRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Henter alle forsikringsmuligheder fra databasen
        public IEnumerable<InsuranceOptionDTO> GetAll()
        {
            // Opretter en liste til forsikrings-DTO'er
            List<InsuranceOptionDTO> dtos = new List<InsuranceOptionDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle forsikringsmuligheder
            command.CommandText = "SELECT * FROM InsuranceOptions";

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Finder placeringen af id-kolonnen
            int id = reader.GetOrdinal("id");

            // Finder placeringen af name-kolonnen
            int name = reader.GetOrdinal("name");

            // Finder placeringen af base_fees-kolonnen
            int baseFees = reader.GetOrdinal("base_fees");

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en InsuranceOptionDTO og tilføjer den til listen
                dtos.Add(CreateDTO(reader, id, name, baseFees));
            }

            // Returnerer listen med forsikringsmuligheder
            return dtos;
        }

        // Omdanner en databaserække til en InsuranceOptionDTO
        InsuranceOptionDTO CreateDTO(IDataReader reader, int id, int name, int baseFees)
        {
            // Opretter og returnerer en InsuranceOptionDTO med data fra databasen
            return new InsuranceOptionDTO
            {
                // Sætter forsikringsmulighedens id
                Id = reader.GetInt32(id),

                // Sætter forsikringsmulighedens navn
                Name = reader.GetString(name),

                // Sætter grundprisen for forsikringen
                BaseFees = reader.GetFloat(baseFees)
            };
        }
    }
}