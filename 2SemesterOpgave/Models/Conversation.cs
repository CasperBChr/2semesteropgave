using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Conversation : IReferrer // implementere IReferrer for at kunne modtage notifikationer, når der kommer nye beskeder i samtalen
    {
        public DateTime CreationTime { get; set; } // Property til at gemme tidspunktet for, hvornår samtalen blev oprettet
        public DateTime LastActive { get; set; } 
        public List<User> Participants { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        public void ReceiveNotification(string message)
        {
            return;
        }
        public Conversation() // Constructor: initialiserer en ny instans af Conversation-klassen, hvor Messages og Participants sættes til tomme lister, og CreationTime og LastActive sættes til det aktuelle tidspunkt
        {
            Messages = new ObservableCollection<Message>();
            Participants = new List<User>();
            CreationTime = DateTime.Now;
            LastActive = DateTime.Now;
        }

        public Conversation(List<User> participants) // Constructor: initialiserer en ny instans af Conversation-klassen, hvor Messages sættes til en tom liste, Participants sættes til den angivne liste af deltagere, og CreationTime og LastActive sættes til det aktuelle tidspunkt
        {
            Messages = new ObservableCollection<Message>();
            Participants = participants;
            CreationTime = DateTime.Now;
            LastActive = DateTime.Now;
        }
        public Conversation(List<User> participants, ObservableCollection<Message> messages, DateTime creationTime, DateTime lastActive) // Constructor: initialiserer en ny instans af Conversation-klassen, hvor Messages sættes til den angivne liste af beskeder, Participants sættes til den angivne liste af deltagere, CreationTime sættes til det angivne tidspunkt for oprettelse, og LastActive sættes til det angivne tidspunkt for sidste aktivitet
        {
            Participants = participants;
            Messages = messages;
            CreationTime = creationTime;
            LastActive = lastActive;

        }
        
    }
}
