using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Algoritme
{
	public class Recommendation
	{
		public ItemProfile Item { get; set; }
		public double Score { get; set; }

		public Recommendation(ItemProfile item, double score)
		{
			Item = item;
			Score = score;
		}
	}
}
