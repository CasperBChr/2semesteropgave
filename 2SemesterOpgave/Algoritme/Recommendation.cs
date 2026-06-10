using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Algoritme
{
    public class Recommendation // Klasse til at repræsentere en anbefaling med et item og en score
    {
        public ItemProfile Item { get; set; } // Gemmer det item der anbefales
        public double Score { get; set; } // Gemmer hvor relevant anbefalingen er

        public Recommendation(ItemProfile item, double score) // Constructor der opretter en anbefaling med item og score
        {
            Item = item; // Sætter det anbefalede item
            Score = score; // Sætter anbefalingens score
        }
    }
}