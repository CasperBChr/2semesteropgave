using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class CategoryDTO
	{
        //DTO property for Category
        public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int? SubId { get; set; }
		public string? SubName { get; set; }
	}
}
