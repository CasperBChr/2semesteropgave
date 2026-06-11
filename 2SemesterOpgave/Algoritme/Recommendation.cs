using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Algoritme
{
    //Recommendation klasse til at holde styr på de anbefalede artikler og deres score, som udregnes i ContentBasedAlgorithm klassen
    public class Recommendation
	{
		public ItemProfile Item { get; set; }
		public double Score { get; set; }

        //Constructor
        public Recommendation(ItemProfile item, double score)
		{
			Item = item;
			Score = score;
		}
	}
}
