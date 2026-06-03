using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class RentalDTO
	{
		public int Id { get; set; }

		public string StartDate { get; set; } = string.Empty;
		public string EndDate { get; set; } = string.Empty;
		public float TotalPrice { get; set; }
		public bool IsAccepted { get; set; }
		public string Status { get; set; } = string.Empty;

		public int RenterId { get; set; }
		public int RenteeId { get; set; }
		public int ArticleId { get; set; }

		public int? ShippingOptionId { get; set; }
		public int? InsuranceOptionId { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}
