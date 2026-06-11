namespace _2SemesterOpgave.Models
{
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class ShippingOption // Klasse til at repræsentere en forsendelsesmulighed, som kan være tilknyttet en artikel og indeholder information om navnet på forsendelsesmuligheden, basisgebyret, leveringstiden i dage og hvilke dage i ugen leveringen kan finde sted
    {
        // Forsendelsesmulighedens id
        public int Id { get; set; }

        // Forsendelsesmulighedens navn
        public string Name { get; set; }

        // Grundgebyr for forsendelsen
        public float BaseFee { get; set; }

        // Leveringstid i antal dage
        public byte DeliveryTimeDays { get; set; }

        //public List<DaysOfWeek> DeliveryDays { get; set; }

        // Constructor der sætter standardværdier
        public ShippingOption()
        {
            // Sætter navn til tom tekst
            Name = string.Empty;

            // Sætter basisgebyr til 0
            BaseFee = 0;

            // Sætter leveringstid til 0 dage
            DeliveryTimeDays = 0;

            //DeliveryDays = new List<DaysOfWeek>();
        }
    }

    public enum DaysOfWeek // Enum til at repræsentere dagene i ugen, som kan bruges til at angive, hvilke dage en forsendelsesmulighed kan levere
    {
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    }
}