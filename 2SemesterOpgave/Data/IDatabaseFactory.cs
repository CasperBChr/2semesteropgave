using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Data
{
	public interface IDatabaseFactory
	{
		SqliteConnection CreateConnection();
	}
}
