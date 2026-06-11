using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ShippingOptionDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
    // Repositoryklasse der håndterer databasekald for fragtmuligheder
    public class ShippingOptionRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public ShippingOptionRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Henter alle fragtmuligheder fra databasen
        public IEnumerable<ShippingOptionDTO> GetAll()
        {
            // Opretter en liste til ShippingOptionDTO'er
            List<ShippingOptionDTO> dtos = new();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle fragtmuligheder
            command.CommandText = "SELECT * FROM ShippingOptions";

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Finder placeringen af id-kolonnen
            int id = reader.GetOrdinal("id");

            // Finder placeringen af name-kolonnen
            int name = reader.GetOrdinal("name");

            // Finder placeringen af base_fee-kolonnen
            int baseFee = reader.GetOrdinal("base_fee");

            // Finder placeringen af delivery_time_days-kolonnen
            int deliveryTimeDays = reader.GetOrdinal("delivery_time_days");

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ShippingOptionDTO og tilføjer den til listen
                dtos.Add(CreateDTO(reader, id, name, baseFee, deliveryTimeDays));
            }

            // Returnerer listen med fragtmuligheder
            return dtos;
        }

        // Omdanner en databaserække til en ShippingOptionDTO
        ShippingOptionDTO CreateDTO(IDataReader reader, int id, int name, int baseFee, int deliveryTimeDays)
        {
            // Opretter og returnerer en ShippingOptionDTO med data fra databasen
            return new ShippingOptionDTO
            {
                // Sætter fragtmulighedens id
                Id = reader.GetInt32(id),

                // Sætter fragtmulighedens navn
                Name = reader.GetString(name),

                // Sætter grundprisen for fragten
                BaseFee = reader.GetFloat(baseFee),

                // Sætter antal dage leveringen tager
                DeliveryTimeDays = reader.GetInt32(deliveryTimeDays)
            };
        }
    }
}