using System.Data;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Data
{
	/// <summary>
	/// Factory Pattern brugt til at oprette database connection, på en ensrettet måde
	/// </summary>
	/// <author>Martin</author>
	// Klasse til at håndtere databaseforbindelsen ved hjælp af IDbConnection og interfacen IDatabaseFactory, ioverenstemmelse med Factory Pattern
    public class SqliteDatabaseFactory : IDatabaseFactory 
	{
		readonly string _connectionString;

		public SqliteDatabaseFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection CreateConnection()
		{
			IDbConnection connection = new SqliteConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}
