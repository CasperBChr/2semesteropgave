using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User

namespace _2SemesterOpgave.Utils
{
    // Klasse der holder styr på den nuværende session
    public class SessionContext
    {
        // Gemmer den bruger der er logget ind
        public User? CurrentUser { get; set; }

        // Returnerer true hvis der er en bruger logget ind
        public bool IsAuthenticated => CurrentUser != null;

        // Rydder sessionen og logger brugeren ud
        public void Clear()
        {
            // Fjerner den nuværende bruger
            CurrentUser = null;
        }
    }
}