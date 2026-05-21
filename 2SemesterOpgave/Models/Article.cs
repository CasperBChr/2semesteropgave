using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using _2SemesterOpgave;
namespace _2SemesterOpgave.Models
{
    public class Article // Klasse til at repræsentere en artikel, som kan lejes ud på platformen
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public Size size { get; set; }
        //public Collection collection { get; set; }
        public float OriginalPrice { get; set; }
        public float DailyPrice { get; set; }
        //public Brand brand { get; set; }
        //public Color Color { get; set; }
        //public Acessibility acessibility { get; set; }
        //public DateTime CreationTime { get; set; }
        public bool IsRented { get; set; }
        public bool IsSmoked { get; set; }
        public bool IsAnimal { get; set; }
        public bool IsClean { get; set; }
        //public TrueToSizeEnum TrueToSize { get; set; }
        //public ConditionEnum Condition { get; set; }
        //public SeasonEnum Season { get; set; }
        //public List<ShippingOption> ShippingOptions { get; set; }
        //public List<Category> categories { get; set; }
        //public List<SubCategory> Subcategories { get; set; }

        public Article(string title, string description, float originalPrice, float dailyPrice, bool isRented, bool isSmoked, bool isAnimal, bool isClean) // Constructor: initialiserer en ny instans af Article-klassen med de angivne parametre
        {
            Title = title; // Sætter Title til det angivne titel, når en ny Article oprettes
            Description = description; // Sætter Description til den angivne beskrivelse, når en ny Article oprettes
            //this.size = size; // Sætter size til den angivne størrelse, når en ny Article oprettes
            OriginalPrice = originalPrice; // Sætter OriginalPrice til den angivne oprindelige pris, når en ny Article oprettes
            DailyPrice = dailyPrice; // Sætter DailyPrice til den angivne daglige pris, når en ny Article oprettes
            //Color = color; // Sætter Color til den angivne farve, når en ny Article oprettes
            //CreationTime = creationTime; // Sætter CreationTime til det angivne oprettelsestidspunkt, når en ny Article oprettes
            IsRented = isRented; // Sætter IsRented til den angivne værdi for at indikere om artiklen er udlejet eller ej
            IsSmoked = isSmoked; // Sætter IsSmoked til den angivne værdi for at indikere om artiklen er røget eller ej
            IsAnimal = isAnimal; // Sætter IsAnimal til den angivne værdi for at indikere om artiklen har været i kontakt med dyr eller ej
            IsClean = isClean; // Sætter IsClean til den angivne værdi for at indikere om artiklen er ren eller ej
            //TrueToSize = trueToSize; // Sætter TrueToSizeEnum til den angivne værdi for at indikere om artiklen er true to size, mindre eller større end forventet
            //Condition = condition; // Sætter ConditionEnum til den angivne værdi for at indikere tilstanden på artiklen
            //Season = season; // Sætter SeasonEnum til den angivne værdi for

        }
        public enum TrueToSizeEnum // Enum til at repræsentere, om en artikel er true to size, mindre eller større end forventet
        {
            TrueToSize,
            Smaller,
            Larger
        }
        public enum Size2 // Enum til at repræsentere størrelsen på en artikel, som kan være relevant for både tøj og sko
        {
            Width,
            Height
        }
        public enum ConditionEnum // Enum til at repræsentere tilstanden på en artikel, som kan være relevant for både tøj og sko
        {
            New,
            LikeNew,
            Used,
            Worn
        }
        public enum SeasonEnum // Enum til at repræsentere sæsonen for en artikel, som kan være relevant for både tøj og sko
        {
            Spring,
            Summer,
            Autumn,
            Winter
        }
    }
}
