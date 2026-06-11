namespace _2SemesterOpgave.Models
{
    public class SubCategory // Klasse til at repræsentere en underkategori, som tilhører en kategori
    {
        // Underkategoriens id
        public int Id { get; set; }

        public string Name { get; set; } // Property: gemmer navnet på underkategorien som tekst
        public Category Category { get; set; } // Property: gemmer en reference til den Category, som underkategorien tilhører, så underkategorien ved, hvilken kategori den er en del af
    }

}