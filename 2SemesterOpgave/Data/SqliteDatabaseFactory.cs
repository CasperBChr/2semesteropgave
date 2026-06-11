using System.Data;
using Microsoft.Data.Sqlite; // Giver adgang til SQLite-forbindelse

namespace _2SemesterOpgave.Data
{
    /// <summary>
    /// Factory Pattern brugt til at oprette database connection, på en ensrettet måde
    /// Kodet af Martin
    /// </summary>
    /// <author>Martin</author>
    // Klasse til at håndtere databaseforbindelsen ved hjælp af IDbConnection og interfacen IDatabaseFactory, ioverenstemmelse med Factory Pattern
    public class SqliteDatabaseFactory : IDatabaseFactory
    {
        readonly string _connectionString; // Gemmer forbindelsesstrengen til databasen

        public SqliteDatabaseFactory(string connectionString) // Constructor der modtager forbindelsesstrengen
        {
            _connectionString = connectionString; // Gemmer forbindelsesstrengen i feltet
        }

        public IDbConnection CreateConnection() // Opretter og returnerer en databaseforbindelse
        {
            IDbConnection connection = new SqliteConnection(_connectionString); // Opretter en ny SQLite-forbindelse
            connection.Open(); // Åbner forbindelsen til databasen
            return connection; // Returnerer den åbne forbindelse
        }
    }
}