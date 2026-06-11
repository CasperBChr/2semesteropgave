using System.Collections.ObjectModel;

namespace _2SemesterOpgave.Models
{
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class Conversation
    {
        public int Id { get; set; } // Gemmer samtalens id
        public DateTime CreationTime { get; set; } // Property til at gemme tidspunktet for, hvornår samtalen blev oprettet
        public DateTime LastActive { get; set; } // Gemmer hvornår samtalen sidst var aktiv
        public List<User> Participants { get; set; } // Gemmer brugerne der deltager i samtalen
        public ObservableCollection<Message> Messages { get; set; } // Gemmer beskederne i samtalen

        public Conversation() // Constructor: initialiserer en ny instans af Conversation-klassen, hvor Messages og Participants sættes til tomme lister, og CreationTime og LastActive sættes til det aktuelle tidspunkt
        {
            Messages = new ObservableCollection<Message>(); // Opretter en tom beskedliste
            Participants = new List<User>(); // Opretter en tom deltagerliste
            CreationTime = DateTime.Now; // Sætter oprettelsestidspunktet til nu
            LastActive = DateTime.Now; // Sætter sidste aktivitet til nu
        }

        public Conversation(List<User> participants) // Constructor: initialiserer en ny instans af Conversation-klassen, hvor Messages sættes til en tom liste, Participants sættes til den angivne liste af deltagere, og CreationTime og LastActive sættes til det aktuelle tidspunkt
        {
            Messages = new ObservableCollection<Message>(); // Opretter en tom beskedliste
            Participants = participants; // Sætter deltagerne til den angivne liste
            CreationTime = DateTime.Now; // Sætter oprettelsestidspunktet til nu
            LastActive = DateTime.Now; // Sætter sidste aktivitet til nu
        }
    }
}