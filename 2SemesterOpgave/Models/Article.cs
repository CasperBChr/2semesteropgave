using _2SemesterOpgave.Algoritme; // Giver adgang til ItemProfile

namespace _2SemesterOpgave.Models
{
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class Article // Klasse til at repræsentere en artikel, som kan lejes ud på platformen
    {
        public int Id { get; set; } = 0; // Gemmer artikelens id
        public string Title { get; set; } = string.Empty; // Gemmer artikelens titel
        public string Description { get; set; } = string.Empty; // Gemmer artikelens beskrivelse
        public float DailyPrice { get; set; } = 0.0f; // Gemmer dagsprisen
        public float OriginalPrice { get; set; } = 0.0f; // Gemmer den oprindelige pris
        public bool IsRented { get; set; } = false; // Viser om artiklen er udlejet
        public bool IsClean { get; set; } = false; // Viser om artiklen er ren

        // Tilknytning til ItemProfile for anbefalingsalgoritmen
        public ItemProfile? ItemProfile { get; set; }

        public Brand? Brand { get; set; } // Gemmer artikelens brand
        public User? Owner { get; set; } // Gemmer artikelens ejer
        public Category? Category { get; set; } // Gemmer artikelens kategori
        public SubCategory? SubCategory { get; set; } // Gemmer artikelens underkategori
        public Color? Color { get; set; } // Gemmer artikelens farve
        public Size? Size { get; set; } // Gemmer artikelens størrelse

        public DateTime CreatedAt { get; set; } // Gemmer hvornår artiklen blev oprettet
        public DateTime UpdatedAt { get; set; } // Gemmer hvornår artiklen sidst blev opdateret
    }
}