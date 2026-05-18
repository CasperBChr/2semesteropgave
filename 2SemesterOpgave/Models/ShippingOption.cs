using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class ShippingOption
    {
        public string Name { get; set; }
        public float BaseFee { get; set; }
        public byte DeliveryTimeDays { get; set; }
        public List<DaysOfWeek> DeliveryDays { get; set; }
        public ShippingOption() //Åben contructor for at kunne oprette en shipping option uden at skulle angive alle parametre
        {
            DeliveryDays = new List<DaysOfWeek>();
            Name = string.Empty;
            BaseFee = 0;
            DeliveryTimeDays = 0;
        }
        public ShippingOption(string name, float baseFee, byte deliveryTimeDays, List<DaysOfWeek> deliveryDays)
        {
            Name = name;
            BaseFee = baseFee;
            DeliveryTimeDays = deliveryTimeDays;
            DeliveryDays = deliveryDays;
        }
    }
    public enum DaysOfWeek
    {
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    }
}
