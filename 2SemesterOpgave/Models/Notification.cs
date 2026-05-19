using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Notification // Klasse til at repræsentere en notifikation, som indeholder en besked og en reference til den bruger, der modtager notifikationen
    {
        public string Message { get; set; } = string.Empty; 
        public IReferrer Referrer { get; set; }
        public Notification(string message, IReferrer referrer) // Constructor: initialiserer en ny instans af Notification-klassen, hvor Message sættes til den angivne besked, og Referrer sættes til den angivne reference til den bruger, der modtager notifikationen
        {
            Message = message;
            Referrer = referrer;
        }

        public void ReceiveNotification(string message) // Metode til at modtage notifikationen, som kalder ReceiveNotification-metoden på den angivne Referrer med den angivne besked
        {
            Console.WriteLine($"Notification: {message}"); // Udskriver notifikationen til konsollen
        }
    }
}
