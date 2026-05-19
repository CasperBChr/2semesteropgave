using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class ShippingOption // Klasse til at repræsentere en forsendelsesmulighed, som kan være tilknyttet en artikel og indeholder information om navnet på forsendelsesmuligheden, basisgebyret, leveringstiden i dage og hvilke dage i ugen leveringen kan finde sted
    {
        public string Name { get; set; }
        public float BaseFee { get; set; }
        public byte DeliveryTimeDays { get; set; }
        public List<DaysOfWeek> DeliveryDays { get; set; }
        public ShippingOption() //Åben contructor for at kunne oprette en shipping option uden at skulle angive alle parametre
        {
            DeliveryDays = new List<DaysOfWeek>(); // Initialiserer DeliveryDays som en tom liste, så den ikke er null, når en ny ShippingOption oprettes uden at angive nogen parametre
            Name = string.Empty;
            BaseFee = 0;
            DeliveryTimeDays = 0;
        }
        public ShippingOption(string name, float baseFee, byte deliveryTimeDays, List<DaysOfWeek> deliveryDays) // Constructor: initialiserer en ny instans af ShippingOption-klassen med de angivne parametre
        {
            Name = name;
            BaseFee = baseFee;
            DeliveryTimeDays = deliveryTimeDays;
            DeliveryDays = deliveryDays;
        }
    }
    public enum DaysOfWeek // Enum til at repræsentere dagene i ugen, som kan bruges til at angive, hvilke dage en forsendelsesmulighed kan levere
    {
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    }
}
