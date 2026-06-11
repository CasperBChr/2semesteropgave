using _2SemesterOpgave.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Algoritme
{
    /// <summary>
    /// Kodet af Camilla
    /// </summary>
    public class UserProfile
    {
        //Kodet af Camilla
        public string UserID { get; set; }
        public User? User { get; set; }
        
        //Dictionary der gemmer brugerens præferencer for forskellige features
        public Dictionary<string, double> Preferences { get; set; }

        //Constructor for UserProfile
        public UserProfile(string userId, List<string> allFeatures)
        {
            UserID = userId;
            Preferences = new Dictionary<string, double>();

            //Initialiserer præferencer til 0, så der ingen præference er i starten
            foreach (string feature in allFeatures)
            {
                Preferences[feature] = 0.0;
            }
        }
        //Funktion som opdaterer brugerprofilen, når de kigger på et element
        public void UpdateUserProfileView(ItemProfile viewedItem, double learningRate = 0.1)
        {
            if (viewedItem == null)
            {
                return;
            }
			foreach (KeyValuePair<string, double> feature in viewedItem.Features)
            {
                //Opdaterer brugerens præferencer baseret på det sete element
                if (Preferences.ContainsKey(feature.Key))
                {
                    //Vægten øges baseret på elementets egenskab og den angivne "learning rate"
                    Preferences[feature.Key] += feature.Value * learningRate;
                }
            }
        }

    }
}
