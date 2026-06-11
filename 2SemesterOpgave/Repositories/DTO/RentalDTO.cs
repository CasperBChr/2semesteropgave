using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	// DTO-klasse der bruges til at transportere lejeaftale-data fra databasen
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class RentalDTO
    {
        // Lejeaftalens id i databasen
        public int Id { get; set; }

        // Startdato for lejeperioden
        public string StartDate { get; set; } = string.Empty;

        // Slutdato for lejeperioden
        public string EndDate { get; set; } = string.Empty;

        // Den samlede pris for lejeaftalen
        public float TotalPrice { get; set; }

        // Fortæller om lejeaftalen er accepteret
        public bool IsAccepted { get; set; }

        // Status på lejeaftalen, fx aktiv, afventer eller afsluttet
        public string Status { get; set; } = string.Empty;

        // Id på brugeren der lejer artiklen
        public int RenterId { get; set; }

        // Id på brugeren der ejer/udlejer artiklen
        public int RenteeId { get; set; }

        // Id på artiklen der bliver lejet
        public int ArticleId { get; set; }

        // Id på den valgte fragtmulighed, hvis der er valgt en
        public int? ShippingOptionId { get; set; }

        // Id på den valgte forsikringsmulighed, hvis der er valgt en
        public int? InsuranceOptionId { get; set; }

        // Dato og tidspunkt for hvornår lejeaftalen blev oprettet
        public DateTime CreatedAt { get; set; }
    }
}