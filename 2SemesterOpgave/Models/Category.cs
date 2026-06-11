namespace _2SemesterOpgave.Models
{
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class Category // Klasse til at repræsentere en kategori, som kan indeholde flere underkategorier
    {
        public int Id { get; set; }
        public string Name { get; set; } // Property: gemmer navnet på kategorien som tekst
        public List<SubCategory> SubCategories { get; set; } // Property: gemmer en liste af underkategorier, som tilhører denne kategori
        public Category() // Constructor: initialiserer en ny instans af Category-klassen uden at sætte nogen værdier
        {
            Name = string.Empty;
            SubCategories = new List<SubCategory>(); // Sætter Name til en tom streng og initialiserer SubCategories som en tom liste
        }
    }
}
