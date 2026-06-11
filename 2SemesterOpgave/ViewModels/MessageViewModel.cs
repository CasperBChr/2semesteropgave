using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Message og User

namespace _2SemesterOpgave.ViewModels
{
    // ViewModel der bruges til at vise en besked i UI'et
    public class MessageViewModel
    {
        // Den originale besked som ViewModel'en bygger på
        public Message Message { get; }

        // Returnerer beskedens tekst
        public string Text => Message.Text;

        // Returnerer navnet på afsenderen
        public string SenderName => Message.Sender.Username;

        // Returnerer tidspunktet hvor beskeden blev sendt
        public DateTime Timestamp => Message.Timestamp;

        // Bestemmer om beskeden skal placeres til venstre eller højre
        public HorizontalAlignment Alignment { get; }

        // Bestemmer baggrundsfarven på beskeden
        public SolidColorBrush Background { get; }

        // Constructor der modtager en besked og den nuværende bruger
        public MessageViewModel(Message message, User currentUser)
        {
            // Gemmer beskeden
            Message = message;

            // Tjekker om beskeden er sendt af den nuværende bruger
            bool isOwnMessage = message.Sender.Id == currentUser.Id;

            // Hvis beskeden er sendt af brugeren selv
            if (isOwnMessage)
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