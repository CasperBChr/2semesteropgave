using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace _2SemesterOpgave.Algoritme
{
    //Kodet af Camilla
    /// <summary>
    /// Kodet af Camilla med en smule hjælp fra Martin
    /// </summary>
    public class ContentBasedAlgorithm
    {
        //Beregner Cosine Similarity mellem brugerens præferencer og elementets profil
        public static double CosineSimilarity(UserProfile user, ItemProfile item)
        {
            //Field for skalarproduktet for Cosine Similarity
            double dotProduct = 0.0;

            //Field for længden af både brugerprofilen og elementprofilen
            double userMagnitude = 0.0;
            double itemMagnitude = 0.0;

            //Beregner skalarproduktet og længderne for Cosine Similarity
            foreach (KeyValuePair<string, double> feature in item.Features)
            {
                string key = feature.Key;
                double itemValue = feature.Value;
                // Inline if statement. Hvis dictionary indeholder featurens key, så indsæt value fra key, ellers så sæt til 0.0
                double userValue = user.Preferences.ContainsKey(key) ? user.Preferences[key] : 0.0;

                //Beregner skalarproduktet
                dotProduct += userValue * itemValue;

                //Beregner længden for itemprofilen
                itemMagnitude += Math.Pow(itemValue, 2);
            }

            //Beregner længden for brugerprofilen
            foreach (KeyValuePair<string, double> userPref in user.Preferences)
            {
                userMagnitude += Math.Pow(userPref.Value, 2);
            }

            //Undgår division med nul
            if (userMagnitude == 0 || itemMagnitude == 0) return 0.0;

            //Returnerer resultatet for Cosine Similarity
            return dotProduct / (Math.Sqrt(userMagnitude) * Math.Sqrt(itemMagnitude));
        }

        //Finder de bedste 5 anbefalinger for en bruger baseret på Cosine Similarity og sorterer dem i faldende rækkefølge
        public static List<Recommendation> GetRecommendations(UserProfile user, List<ItemProfile> catalog, int topN = 5)
        {
			//Liste til at gemme anbefalinger og deres match-score
			List<Recommendation> recommendations = new List<Recommendation>();

            //Beregner match-score for hvert element i kataloget og tilføjer det til anbefalingslisten
            foreach (ItemProfile item in catalog)
            {
                //Beregner Cosine Similarity mellem brugerprofilen og elementprofilen
                double score = CosineSimilarity(user, item);
                recommendations.Add(new Recommendation(item, score));
            }

            //Returnerer de top 5 anbefalinger sorteret efter match-score i faldende rækkefølge
            return recommendations.OrderByDescending(r => r.Score).Take(topN).ToList();
        }


		//Metode der printer anbefalinger ud fra algoritmen
		static void PrintRecommendations(UserProfile user, List<ItemProfile> catalog)
		{
			//Henter og printer anbefalingerne for brugeren
			List<Recommendation> recommendations = ContentBasedAlgorithm.GetRecommendations(user, catalog);

			//Printer anbefalingerne i konsollen
			foreach (Recommendation recommendation in recommendations)
			{
				//Udskriver anbefalingerne i konsollen
				Debug.WriteLine($"- {recommendation.Item.Name} (Match Score: {recommendation.Score:F2})");
			}

		}
	}
}
