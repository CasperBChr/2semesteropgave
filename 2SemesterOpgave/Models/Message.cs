namespace _2SemesterOpgave.Models
{
    public class Message // Klasse til at repræsentere en besked i en samtale, som indeholder tekst og en reference til den samtale, som beskeden tilhører
    {
        public string Text { get; set; } // Property: gemmer teksten i beskeden som tekst
        public Conversation Conversation { get; set; } // Property: gemmer en reference til den Conversation, som beskeden tilhører

        public DateTime Timestamp { get; set; } // Gemmer tidspunktet for beskeden
        public User Sender { get; set; } // Gemmer brugeren der sendte beskeden

        public Message(string text, Conversation conversation, User sender, DateTime timestamp) // Constructor: initialiserer en ny instans af Message-klassen med tekst, samtale, afsender og tidspunkt
        {
            Text = text; // Sætter Text til den angivne tekst, når en ny Message oprettes
            Conversation = conversation; // Sætter Conversation til den angivne Conversation, når en ny Message oprettes
            Sender = sender; // Sætter Sender til den bruger der har sendt beskeden
            Timestamp = timestamp; // Sætter Timestamp til tidspunktet for beskeden
        }
    }
}