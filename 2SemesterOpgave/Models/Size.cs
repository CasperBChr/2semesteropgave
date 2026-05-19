using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Size // Klasse til at repræsentere størrelsen på en artikel, som kan være relevant for både tøj og sko
    {
        public byte ClothingEUSize { get; set; } // EU-størrelse for tøj, som er en byte (0-255) og kan bruges til at angive størrelsen på en artikel i det europæiske størrelsessystem
        public byte ClothingUSSize { get; set; } // US-størrelse for tøj, som er en byte (0-255) og kan bruges til at angive størrelsen på en artikel i det amerikanske størrelsessystem
        public float ShoeEUSize { get; set; } // EU-størrelse for sko, som er en float og kan bruges til at angive størrelsen på en artikel i det europæiske størrelsessystem for sko. Float-typen tillader decimaler, hvilket kan være nødvendigt for at angive sko størrelser præcist.
        public Size(byte clothingEUSize, byte clothingUSSize, float shoeEUSize) // Constructor: initialiserer en ny instans af Size-klassen med de angivne parametre
        {
            ClothingEUSize = clothingEUSize;
            ClothingUSSize = clothingUSSize;
            ShoeEUSize = shoeEUSize;
        }
        public Size(byte clothingUSSize) // Constructor: initialiserer en ny instans af Size-klassen med kun US-størrelsen, og beregner EU-størrelsen baseret på den angivne US-størrelse
        {
            ClothingUSSize = clothingUSSize; // Sætter ClothingUSSize til den angivne US-størrelse, når en ny Size oprettes med kun US-størrelsen
            ClothingEUSize = CalculateEUSize(clothingUSSize); // Beregner EU-størrelsen baseret på den angivne US-størrelse ved at kalde CalculateEUSize-metoden
        }
        private byte CalculateEUSize(byte clothingUSSize)  // Metode til at beregne EU-størrelsen baseret på den angivne US-størrelse. Implementeringen er en placeholder og skal erstattes med den faktiske logik for størrelseskonvertering.
        {
            
            return clothingUSSize; // Placeholder: returnerer den angivne US-størrelse som EU-størrelse, hvilket ikke er korrekt og skal erstattes med den faktiske logik for størrelseskonvertering
        }
    }
}
