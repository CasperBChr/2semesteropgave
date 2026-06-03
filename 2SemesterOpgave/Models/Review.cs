using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
	public class Review
	{
		public int Id { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public int RentalId { get; set; }
		public User? Reviewer { get; set; }
		public User? Reviewee { get; set; }
	}
}
