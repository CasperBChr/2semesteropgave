using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Utils
{
	public class SessionContext
	{
		public User? CurrentUser { get; set; }

		public bool IsAuthenticated => CurrentUser != null;

		public void Clear()
		{
			CurrentUser = null;
		}
	}
}
