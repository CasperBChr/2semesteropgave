using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class CollectionDTO
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int? BrandId { get; set; }
		public int? DesignerId { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
