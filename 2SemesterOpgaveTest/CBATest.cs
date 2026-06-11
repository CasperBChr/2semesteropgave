using _2SemesterOpgave;
using _2SemesterOpgave.Algoritme;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Pages;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace _2SemesterOpgaveTest
{
    //Kodet af Camilla
	[TestClass]
	public sealed class CBATest
	{
        //Unit test for at teste Top-N anbefalingsfunktionen i ContentBasedAlgorithm klassen
        [TestMethod]
		public void GetRecommendations()
		{
			//Arrange
			ContentBasedAlgorithm recommender = new ContentBasedAlgorithm();

            // Opret test-items med kendte features (f.eks. [Action, Komedie])
            ItemProfile first1 = new ItemProfile { ArticleID = 1, Name = "Kjoler", Features = new Dictionary<string, double> { { "Kjoler", 1.0 } } };
            ItemProfile first2 = new ItemProfile { ArticleID = 2, Name = "Overdele", Features = new Dictionary<string, double> { { "Overdele", 0.0 } } };
            List<ItemProfile> allItems = new List<ItemProfile> { first1, first2 };

            //Det forventede resultat
            string expected = "Kjoler";

            //Brugerprofil med præference for "Kjoler"
            UserProfile userProfile = new UserProfile("user1", new List<string> { "Overdele", "Kjoler" });

            //Act
            //Metoden der returnerer anbefalinger baseret på brugerprofilen og ItemProfile
            List<Recommendation> recommendations = ContentBasedAlgorithm.GetRecommendations(userProfile, allItems, 1);

            //Assert
            //Recommendations skal indeholde det item, der matcher brugerprofilen bedst (i dette tilfælde "Kjoler")
            Assert.IsNotNull(recommendations);

            //Det første item i anbefalingerne skal være "Kjoler", da det har den højeste match-score
            Assert.AreEqual(expected, recommendations.First().Item.Name);
        }

        //Unit test for at teste Cosine Similarity funktionen i forhold til identiske profiler
        [TestMethod]
        public void CosineSimilarityMatch()
        {
            //Arrange
            UserProfile user = new UserProfile("user1", new List<string> { "Kjole" });
            user.Preferences["Kjole"] = 1.0;
            ItemProfile item = new ItemProfile { ArticleID = 1, Name = "Match", Features = new Dictionary<string, double> { { "Kjole", 1.0 } } };

            //Det forventede resultat
            float expected = 1.0f;

            //Act
            //Beregner cosine similarity mellem brugerprofilen og itemprofilen
            double similarity = ContentBasedAlgorithm.CosineSimilarity(user, item);

            //Assert
            //Forventet cosine similarity er 1.0 for identiske profiler
            Assert.AreEqual(expected, similarity, 0.0001); // Forventet cosine similarity er 1.0 for identiske profiler
        }

        //Unit test for at teste Cosine Similarity funktionen i forhold til forskellige profiler
        [TestMethod]
        public void CosineSimilarityNoMatch()
        {
            //Arrange
            UserProfile user = new UserProfile("user1", new List<string> { "Overdele" });
            user.Preferences["Overdele"] = 0.0;
            ItemProfile noMatch = new ItemProfile { ArticleID = 1, Name = "No Match", Features = new Dictionary<string, double> { { "Overdele", 0.0 } } };

            //Det forventede resultat
            float expected = 0.0f;

            //Act
            //Beregner cosine similarity mellem brugerprofilen og itemprofilen
            double similarity = ContentBasedAlgorithm.CosineSimilarity(user, noMatch);

            //Assert
            //Forventet cosine similarity er 0.0 for ikke-matchende profiler
            Assert.AreEqual(expected, similarity, 0.0001); // Forventet cosine similarity er 0.0 for ikke-matchende profiler
        }
    }
}
