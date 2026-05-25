using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave
{
	public class FilterCriteria
	{
		public Category? Category { get; set; }
		public SubCategory? SubCategory { get; set; }
		public string? SearchText { get; set; }
		public string? Color { get; set; }
		public string? Size { get; set; }
		public float? MinPrice { get; set; }
		public float? MaxPrice { get; set; }
	}
}
