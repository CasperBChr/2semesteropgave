using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Data
{
    public class Database // Klasse til at håndtere databaseforbindelsen ved hjælp af SqliteConnection
    {
        public SqliteConnection Connection; // Privat felt til at gemme instansen af SqliteConnection, som repræsenterer forbindelsen til databasen

        public Database(string connectionString) // Constructor: initialiserer en ny instans af DbConnection-klassen med en forbindelse til databasen baseret på den angivne connectionString
        {
            Connection = new SqliteConnection(connectionString); 
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}
