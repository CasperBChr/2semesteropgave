using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Size // Klasse til at repræsentere størrelsen på en artikel, som kan være relevant for både tøj og sko
    {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
