using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class InsuranceOptionDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public float BaseFees { get; set; }
	}
}
