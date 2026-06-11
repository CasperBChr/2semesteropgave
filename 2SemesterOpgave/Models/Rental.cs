namespace _2SemesterOpgave.Models
{
    public class Rental // Klasse til at repræsentere en lejeaftale, som indeholder information om lejeperioden, prisen, lejeren, den lejede artikel og andre relevante detaljer
    {
        public int Id { get; set; } // Gemmer lejeaftalens unikke id
        public bool IsAccepted { get; set; } // Gemmer om lejeaftalen er accepteret
        public string Status { get; set; } = string.Empty; // Gemmer lejeaftalens status
        public DateOnly StartDate { get; set; } // Gemmer startdatoen for lejeperioden
        public DateOnly EndDate { get; set; } // Gemmer slutdatoen for lejeperioden
        public decimal TotalPrice { get; set; } // Gemmer den samlede pris for lejeaftalen
        public User Renter { get; set; } // Gemmer brugeren der lejer artiklen
        public User Rentee { get; set; } // Gemmer brugeren der udlejer artiklen
        public Article Article { get; set; } // Gemmer den artikel der bliver lejet
        public DateTime CreatedAt { get; set; } // Gemmer hvornår lejeaftalen blev oprettet
        public ShippingOption ShippingChoice { get; set; } // Gemmer den valgte forsendelsesmulighed
        public InsuranceOption InsuranceChoice { get; set; } // Gemmer den valgte forsikringsmulighed
    }
}