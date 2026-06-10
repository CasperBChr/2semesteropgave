using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class BrandDTO
	{
        //DTO property for Brand
        public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description {  get; set; } = string.Empty;

		public string? LogoPath { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
