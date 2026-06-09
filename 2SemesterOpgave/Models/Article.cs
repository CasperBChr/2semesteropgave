using _2SemesterOpgave.Algoritme;
namespace _2SemesterOpgave.Models
{
    public class Article // Klasse til at repræsentere en artikel, som kan lejes ud på platformen
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float DailyPrice { get; set; } = 0.0f;
        public float OriginalPrice { get; set; } = 0.0f;
		public bool IsRented { get; set; } = false;
		public bool IsClean { get; set; } = false;

        // Tilknytning til ItemProfile for anbefalingsalgoritmen
        public ItemProfile? ItemProfile { get; set; } 

        public Brand? Brand { get; set; }
        public User? Owner { get; set; }
        public Category? Category { get; set; }
        public SubCategory? SubCategory { get; set; }
        public Color? Color { get; set; }
        public Size? Size { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

    }
}
