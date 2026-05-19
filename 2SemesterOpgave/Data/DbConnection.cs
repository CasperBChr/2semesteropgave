using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Data
{
    public class DbConnection // Klasse til at håndtere databaseforbindelsen ved hjælp af SqliteConnection
    {
        private SqliteConnection _connection; // Privat felt til at gemme instansen af SqliteConnection, som repræsenterer forbindelsen til databasen

        public DbConnection(string connectionString) // Constructor: initialiserer en ny instans af DbConnection-klassen med en forbindelse til databasen baseret på den angivne connectionString
        {
            _connection = new SqliteConnection(connectionString); 
        }

        public void Open()
        {
            _connection.Open();
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
