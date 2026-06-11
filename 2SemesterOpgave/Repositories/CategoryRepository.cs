using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Category og SubCategory
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
	// Repositoryklasse der håndterer databasekald for kategorier
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	public class CategoryRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;


        // Constructor der modtager database-factory
        public CategoryRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Henter alle kategorier og deres underkategorier fra databasen
        public IEnumerable<Category> GetAllCategories()
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter kategorier og tilhørende underkategorier
            command.CommandText = "SELECT c.id as CategoryId, c.name as CategoryName, s.id as SubId, s.name as SubName FROM Categories c LEFT JOIN Subcategories s ON s.category_id = c.id ORDER BY c.id";

            // Dictionary bruges til at samle underkategorier under den rigtige kategori
            Dictionary<int, Category> categories = new Dictionary<int, Category>();

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Henter kategoriens id fra rækken
                int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));

                // Tjekker om kategorien ikke allerede findes i dictionary
                if (!categories.ContainsKey(categoryId))
                {
                    // Opretter en ny kategori og gemmer den i dictionary
                    categories[categoryId] = new Category
                    {
                        // Sætter kategoriens id
                        Id = categoryId,

                        // Sætter kategoriens navn
                        Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                }

                // Tilføjer underkategorien til den rigtige kategori
                categories[categoryId].SubCategories.Add(new SubCategory
                {
                    // Sætter underkategoriens id
                    Id = reader.GetInt32(reader.GetOrdinal("SubId")),

                    // Sætter underkategoriens navn
                    Name = reader.GetString(reader.GetOrdinal("SubName")),

                    // Sætter hvilken kategori underkategorien hører til
                    Category = categories[categoryId]
                });
            }

            // Returnerer alle kategorierne med deres underkategorier
            return categories.Values;
        }
    }
}