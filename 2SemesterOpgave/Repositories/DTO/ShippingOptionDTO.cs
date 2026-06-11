using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class ShippingOptionDTO
	{
        //DTO property for ShippingOption
        public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public float BaseFee { get; set; }
		public int DeliveryTimeDays { get; set; }
	}
}
