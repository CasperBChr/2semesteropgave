using System.Collections.ObjectModel;

namespace _2SemesterOpgave.Models
{
    public class Conversation
    {
		public int Id { get; set; }
		public DateTime CreationTime { get; set; } // Property til at gemme tidspunktet for, hvornår samtalen blev oprettet
        public DateTime LastActive { get; set; } 
        public List<User> Participants { get; set; }
        public ObservableCollection<Message> Messages { get; set; }

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
    }
}
