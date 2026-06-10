using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Data
{
	/// <summary>
	/// Factory Pattern brugt til at oprette database connection, på en ensrettet måde
	/// </summary>
	/// <author>Martin</author>
    public class SqliteDatabaseFactory : IDatabaseFactory // Klasse til at håndtere databaseforbindelsen ved hjælp af SqliteConnection og interfacen IDatabaseFactory, ioverenstemmelse med Factory Pattern
	{
		readonly string _connectionString;

		public SqliteDatabaseFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public SqliteConnection CreateConnection()
		{
			SqliteConnection connection = new SqliteConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}
