using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
    // DTO-klasse der bruges til at transportere artikeldata fra databasen
    public class ArticleDTO
    {
        // Artikelens id i databasen
        public int Id { get; set; }

        // Artikelens titel
        public string Title { get; set; } = string.Empty;

        // Artikelens beskrivelse
        public string Description { get; set; } = string.Empty;

        // Artikelens dagspris
        public float DailyPrice { get; set; }

        // Artikelens oprindelige pris
        public float OriginalPrice { get; set; }

        // Id på artikelens kategori
        public int? CategoryId { get; set; }

        // Id på artikelens underkategori
        public int? SubcategoryId { get; set; }

        // Id på artikelens brand/mærke
        public int? BrandId { get; set; }

        // Id på artikelens farve
        public int? ColorId { get; set; }

        // Id på artikelens størrelse
        public int? SizeId { get; set; }

        // Fortæller om artiklen er udlejet
        public bool IsRented { get; set; }

        // Fortæller om artiklen er ren
        public bool IsClean { get; set; }

        // Id på brugeren der ejer artiklen
        public int? OwnerId { get; set; }

        // Fortæller om artiklen er markeret som favorit
        public bool IsFavorite { get; set; }

        // Dato og tidspunkt for hvornår artiklen blev oprettet
        public DateTime CreatedAt { get; set; }

        // Dato og tidspunkt for hvornår artiklen sidst blev opdateret
        public DateTime UpdatedAt { get; set; }
    }
}