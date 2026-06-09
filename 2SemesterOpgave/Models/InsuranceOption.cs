namespace _2SemesterOpgave.Models
{
    public class InsuranceOption
    {
		public int Id { get; set; }
        public string Name { get; set; } = string.Empty; //Property: gemmer navnet på forsikringsmuligheden som tekst
        public float BaseFees { get; set; } // Property: gemmer de grundlæggende omkostninger for forsikringsmuligheden som et flydende tal
    }
}
