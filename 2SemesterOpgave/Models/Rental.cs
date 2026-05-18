using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Rental
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
        public Rental(User renter, User rentee, Article article, DateOnly startDate, DateOnly endDate, decimal totalPrice, DateTime creationTime, ShippingOption shippingChoice, InsuranceOption insuranceOption )
        {
            Renter = renter;
            Rentee = rentee;
            Article = article;
            StartDate = startDate;
            EndDate = endDate;
            TotalPrice = totalPrice;
            CreationTime = creationTime;
            ShippingChoice = shippingChoice;
            InsuranceOption = insuranceOption;
        }
    }
}
