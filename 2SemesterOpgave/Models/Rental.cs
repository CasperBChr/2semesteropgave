using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Rental // Klasse til at repræsentere en lejeaftale, som indeholder information om lejeperioden, prisen, lejeren, den lejede artikel og andre relevante detaljer
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public User Renter { get; set; }
        public User Rentee { get; set; }
        public Article Article  { get; set; }
        public DateTime CreationTime { get; set; }
        public ShippingOption ShippingChoice { get; set; }
        public InsuranceOption InsuranceOption { get; set; }
        public Rental(User renter, User rentee, Article article, DateOnly startDate, DateOnly endDate, decimal totalPrice, DateTime creationTime, ShippingOption shippingChoice, InsuranceOption insuranceOption) // Constructor: initialiserer en ny instans af Rental-klassen, hvor Renter sættes til den angivne bruger, Rentee sættes til den angivne bruger, Article sættes til den angivne artikel, StartDate og EndDate sættes til de angivne datoer for lejeperioden, TotalPrice sættes til den angivne pris for lejeaftalen, CreationTime sættes til det angivne tidspunkt for oprettelse, ShippingChoice sættes til den angivne forsendelsesmulighed, og InsuranceOption sættes til den angivne forsikringsmulighed
        {
            Renter = renter; // Sætter Renter til den angivne bruger, når en ny Rental oprettes
            Rentee = rentee; // Sætter Rentee til den angivne bruger, når en ny Rental oprettes
            Article = article; // Sætter Article til den angivne artikel, når en ny Rental oprettes
            StartDate = startDate; // Sætter StartDate til den angivne startdato for lejeperioden, når en ny Rental oprettes
            EndDate = endDate; // Sætter End
            TotalPrice = totalPrice; // Sætter TotalPrice til den angivne pris for lejeaftalen, når en ny Rental oprettes
            CreationTime = creationTime; // Sætter CreationTime til det angivne tidspunkt for oprettelse, når en ny Rental oprettes
            ShippingChoice = shippingChoice; // Sætter ShippingChoice til den angivne forsendelsesmulighed, når en ny Rental oprettes
            InsuranceOption = insuranceOption; // Sætter InsuranceOption til den angivne forsikringsmulighed, når en ny Rental oprettes
        }
    }
}
