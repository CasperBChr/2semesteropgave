namespace _2SemesterOpgave.Models
{
    public class ShippingOption // Klasse til at repræsentere en forsendelsesmulighed, som kan være tilknyttet en artikel og indeholder information om navnet på forsendelsesmuligheden, basisgebyret, leveringstiden i dage og hvilke dage i ugen leveringen kan finde sted
    {
		public int Id { get; set; }
        public string Name { get; set; }
        public float BaseFee { get; set; }
        public byte DeliveryTimeDays { get; set; }
        //public List<DaysOfWeek> DeliveryDays { get; set; }

		public ShippingOption()
		{
			Name = string.Empty;
			BaseFee = 0;
			DeliveryTimeDays = 0;
			//DeliveryDays = new List<DaysOfWeek>();
		}
	}
    public enum DaysOfWeek // Enum til at repræsentere dagene i ugen, som kan bruges til at angive, hvilke dage en forsendelsesmulighed kan levere
    {
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    }
}
