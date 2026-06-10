namespace _2SemesterOpgave.Models
{
    public class Size // Klasse til at repræsentere størrelsen på en artikel, som kan være relevant for både tøj og sko
    {
        // Størrelsens id
        public int Id { get; set; }

        // Størrelsens navn
        public string Name { get; set; } = string.Empty;

        // Hvornår størrelsen blev oprettet
        public DateTime CreatedAt { get; set; }

        // Hvornår størrelsen sidst blev opdateret
        public DateTime UpdatedAt { get; set; }
    }
}