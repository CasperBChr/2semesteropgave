using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Message og User

namespace _2SemesterOpgave.Utils.Messages
{
	// UI-model der bruges til at vise en besked i brugerfladen
	/// <summary>
	/// Kodet af Martin
	/// </summary>
	public class MessageUIModel
    {
        // Den originale besked som UI-modellen bygger på
        public Message Message { get; }

        // Bestemmer om beskeden skal stå til venstre eller højre
        public HorizontalAlignment Alignment { get; set; }

        // Bestemmer baggrundsfarven på beskeden
        public SolidColorBrush Background { get; set; }

        // Teksten der vises i beskeden
        public string Text;

        // Navnet på brugeren der har sendt beskeden
        public string Sender;

        // Tidspunktet hvor beskeden blev sendt
        public DateTime Timestamp;

        // Constructor der modtager beskeden og den nuværende bruger
        public MessageUIModel(Message message, User currentUser)
        {
            // Gemmer den originale besked
            Message = message;

            // Henter afsenderens brugernavn
            Sender = Message.Sender.Username;

            // Henter beskedens tekst
            Text = Message.Text;

            // Henter tidspunktet for beskeden
            Timestamp = Message.Timestamp;

            // Tjekker om beskeden er sendt af den nuværende bruger
            bool isOwn = message.Sender.Id == currentUser.Id;

            // Hvis beskeden er sendt af brugeren selv
            if (isOwn)
            {
                // Placerer beskeden til højre
                Alignment = HorizontalAlignment.Right;

                // Giver beskeden en pink baggrund
                Background = Brushes.HotPink;
            }
            else
            {
                // Placerer beskeden til venstre
                Alignment = HorizontalAlignment.Left;

                // Giver beskeden en lyseblå baggrund
                Background = Brushes.LightBlue;
            }
        }
    }
}