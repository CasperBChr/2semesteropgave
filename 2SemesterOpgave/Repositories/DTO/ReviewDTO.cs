using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class ReviewDTO
	{
		public int Id { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public int? RentalId { get; set; }
		public int ReviewerId { get; set; }
		public int RevieweeId { get; set; }
	}
}
