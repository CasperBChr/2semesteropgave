using _2SemesterOpgave.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Algoritme
{
    //Kodet af Camilla
    public class ItemProfile
    {
        public int ArticleID { get; set; }
        public Article? Article { get; set; }
        public string Name { get; set; }

        //Dictionary der gemmer elementets egenskaber og deres score
        public Dictionary<string, double> Features { get; set; }

        //Constructor for ItemProfile
        public ItemProfile(int articleId, string name, Dictionary<string, double> features)
        {
            ArticleID = articleId;
            Name = name;
            Features = features;
        }
        public ItemProfile()
        {
            // Parameterless constructor for deserialization or manual property setting
        }
    }
}
