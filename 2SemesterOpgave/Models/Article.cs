using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using _2SemesterOpgave;
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
        public bool IsSmoked { get; set; } = false;
		public bool IsAnimal { get; set; } = false;
		public bool IsClean { get; set; } = false;

        //Tilknytning til ItemProfile for anbefalingsalgoritmen
        public ItemProfile? ItemProfile { get; set; } 

        public Brand? Brand { get; set; }
        public User? Owner { get; set; }
        public Category? Category { get; set; }
        public SubCategory? SubCategory { get; set; }
		public string? Color { get; set; }
		public Size? Size { get; set; }
		public Collection? collection { get; set; }
		public string? ImagePath { get; set; }

		public List<ShippingOption>? ShippingOptions { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		//public Color Color { get; set; }

		//public Acessibility acessibility { get; set; }
		//public DateTime CreationTime { get; set; }

		//public TrueToSizeEnum TrueToSize { get; set; }
		//public ConditionEnum Condition { get; set; }
		//public SeasonEnum Season { get; set; }







		////Constructor: initialiserer en ny instans af Article-klassen med de angivne parametre
		//public Article(string title, string description, Category category, SubCategory subCategory, Size size, float dailyPrice, string color, Brand brand, bool isRented, float originalPrice, bool isSmoked, bool isAnimal, bool isClean, User owner)
		//{
		//    Title = title; // Sætter Title til det angivne titel, når en ny Article oprettes
		//    Description = description; // Sætter Description til den angivne beskrivelse, når en ny Article oprettes
		//    Category = category; // Sætter categories til den angivne liste af kategorier, når en ny Article oprettes
		//    SubCategory = subCategory; // Sætter Subcategories til den angivne liste af underkategorier, når en ny Article oprettes
		//    Size = size; // Sætter size til den angivne størrelse, når en ny Article oprettes        

		//}
		//public Article(string title, string description, Brand brand, float originalPrice, float dailyPrice, bool isRented, bool isSmoked, bool isAnimal, bool isClean) // Constructor: initialiserer en ny instans af Article-klassen med de angivne parametre
		//{
		//    Title = title; // Sætter Title til det angivne titel, når en ny Article oprettes
		//    Description = description; // Sætter Description til den angivne beskrivelse, når en ny Article oprettes
		//    Brand = brand; // Sætter Brand til den angivne brand, når en ny Article oprettes
		//    //this.size = size; // Sætter size til den angivne størrelse, når en ny Article oprettes
		//    OriginalPrice = originalPrice; // Sætter OriginalPrice til den angivne oprindelige pris, når en ny Article oprettes

		//    DailyPrice = dailyPrice; // Sætter DailyPrice til den angivne daglige pris, når en ny Article oprettes
		//    Color = "Black"; // Sætter Color til den angivne farve, når en ny Article oprettes
		//    Brand = brand; // Sætter Brand til den angivne mærke, når en ny Article oprettes
		//    IsRented = isRented; // Sætter IsRented til den angivne værdi for at indikere om artiklen er udlejet eller ej
		//    //CreationTime = creationTime; // Sætter CreationTime til det angivne oprettelsestidspunkt, når en ny Article oprettes
		//    OriginalPrice = originalPrice; // Sætter OriginalPrice til den angivne oprindelige pris, når en ny Article oprettes
		//    IsSmoked = isSmoked; // Sætter IsSmoked til den angivne værdi for at indikere om artiklen er røget eller ej
		//    IsAnimal = isAnimal; // Sætter IsAnimal til den angivne værdi for at indikere om artiklen har været i kontakt med dyr eller ej
		//    IsClean = isClean; // Sætter IsClean til den angivne værdi for at indikere om artiklen er ren eller ej
		//    Owner = new User();
		//    //TrueToSize = trueToSize; // Sætter TrueToSizeEnum til den angivne værdi for at indikere om artiklen er true to size, mindre eller større end forventet
		//    //Condition = condition; // Sætter ConditionEnum til den angivne værdi for at indikere tilstanden på artiklen
		//    //Season = season; // Sætter SeasonEnum til den angivne værdi for

		//}

		//public Article(string title, string description, float originalPrice, float dailyPrice, bool isRented, bool isSmoked, bool isAnimal, bool isClean)
		//{
		//    Title = title;
		//    Description = description;
		//    OriginalPrice = originalPrice;
		//    DailyPrice = dailyPrice;
		//    IsRented = isRented;
		//    IsSmoked = isSmoked;
		//    IsAnimal = isAnimal;
		//    IsClean = isClean;
		//}

		////public Article(string title, string description, int category, int subcategory, int size, float originalPrice, float dailyPrice, int color, int brand, bool isRented, bool isSmoked, bool isAnimal, bool isClean, User owner)
		////{
		////    Title = title;
		////    Description = description;
		////    OriginalPrice = originalPrice;
		////    DailyPrice = dailyPrice;
		////    IsRented = isRented;
		////    IsSmoked = isSmoked;
		////    IsAnimal = isAnimal;
		////    IsClean = isClean;
		////    Owner = owner;
		////}

		//public Article(string title, string description, int category, int subcategory, int size, float originalPrice, float dailyPrice, int color, int brand, bool isRented, bool isSmoked, bool isAnimal, bool isClean, int owner)
		//{
		//    Title = title;
		//    Description = description;
		//    OriginalPrice = originalPrice;
		//    DailyPrice = dailyPrice;
		//    IsRented = isRented;
		//    IsSmoked = isSmoked;
		//    IsAnimal = isAnimal;
		//    IsClean = isClean;
		//}

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
