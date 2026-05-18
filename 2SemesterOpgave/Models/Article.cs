using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using _2SemesterOpgave;
namespace _2SemesterOpgave.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Size size { get; set; }
        //public Collection collection { get; set; }
        public float OriginalPrice { get; set; }
        public float DailyPrice { get; set; }
        //public Brand brand { get; set; }
        public Color Color { get; set; }
        //public Acessibility acessibility { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsRented { get; set; }
        public bool IsSmoked { get; set; }
        public bool IsAnimal { get; set; }
        public bool IsClean { get; set; }
        public TrueToSize TrueToSize { get; set; }
        public Condition Condition { get; set; }
        public Season Season { get; set; }
        public List<ShippingOption> ShippingOptions { get; set; }
        public List<Category> categories { get; set; }
        public List<SubCategory> Subcategories { get; set; }

    }
    public enum TrueToSize 
    { 
      Width, 
      Height 
    }
    public enum Condition
    {
        New,
        LikeNew,
        Used,
        Worn
    }
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
}
