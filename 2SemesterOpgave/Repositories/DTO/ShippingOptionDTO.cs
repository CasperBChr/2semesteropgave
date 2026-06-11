using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	// DTO-klasse der bruges til at transportere fragtmulighed-data fra databasen
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class ShippingOptionDTO
    {
        // Fragtmulighedens id i databasen
        public int Id { get; set; }

        // Fragtmulighedens navn
        public string Name { get; set; } = string.Empty;

        // Grundprisen for fragten
        public float BaseFee { get; set; }

        // Antal dage leveringen cirka tager
        public int DeliveryTimeDays { get; set; }
    }
}
