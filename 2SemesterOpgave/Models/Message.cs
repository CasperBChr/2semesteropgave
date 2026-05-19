using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Message // Klasse til at repræsentere en besked i en samtale, som indeholder tekst og en reference til den samtale, som beskeden tilhører
    {
        public string Text { get; set; } // Property: gemmer teksten i beskeden som tekst
        public Conversation Conversation { get; set; } //   Property: gemmer en reference til den Conversation, som beskeden tilhører, så

        public Message(string text, Conversation conversation) //  Constructor: initialiserer en ny instans af Message-klassen med tekst og en reference til en Conversation
        {
            Text = text; // Sætter Text til den angivne tekst, når en ny Message oprettes
            Conversation = conversation; // Sætter Conversation til den angivne Conversation, når en ny Message oprettes, så beskeden ved, hvilken samtale den tilhører
        }
    }
    
}
