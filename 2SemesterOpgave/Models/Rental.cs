namespace _2SemesterOpgave.Models
{
    public class Rental // Klasse til at repræsentere en lejeaftale, som indeholder information om lejeperioden, prisen, lejeren, den lejede artikel og andre relevante detaljer
    {
		public int Id { get; set; }
		public bool IsAccepted { get; set; }
		public string Status { get; set; } = string.Empty;
		public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public User Renter { get; set; }
        public User Rentee { get; set; }
        public Article Article  { get; set; }
        public DateTime CreatedAt { get; set; }
        public ShippingOption ShippingChoice { get; set; }
        public InsuranceOption InsuranceChoice { get; set; }
    }
}
