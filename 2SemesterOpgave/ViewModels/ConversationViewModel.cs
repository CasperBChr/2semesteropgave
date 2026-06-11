using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Conversation, Message og User

namespace _2SemesterOpgave.ViewModels
{
	// ViewModel der bruges til at vise en samtale i UI'et
	/// <summary>
	/// Kodet af Martin
	/// </summary>
	public class ConversationViewModel
    {
        // Den samtale som ViewModel'en bygger på
        public Conversation Conversation { get; }

        // Liste med beskeder, som UI'et kan binde sig til
        public ObservableCollection<MessageViewModel> Messages { get; }

        // Liste med de andre deltagere i samtalen
        public ObservableCollection<User> OtherParticipants { get; }

        // Gemmer den nuværende bruger
        User _currentUser;


        // Constructor der modtager en samtale og den nuværende bruger
        public ConversationViewModel(Conversation conversation, User currentUser)
        {
            // Gemmer samtalen
            Conversation = conversation;

            // Gemmer den nuværende bruger
            _currentUser = currentUser;

            // Skriver samtalens hashcode i debug output
            Debug.WriteLine($"UI Conversation: {conversation.GetHashCode()}");

            // Opretter en tom liste til de andre deltagere
            OtherParticipants = new ObservableCollection<User>();

            // Gennemgår alle deltagere i samtalen
            for (int i = 0; i < Conversation.Participants.Count; i++)
            {
                // Tjekker om deltageren ikke er den nuværende bruger
                if (Conversation.Participants[i].Id != _currentUser.Id)
                {
                    // Tilføjer deltageren til listen over andre deltagere
                    OtherParticipants.Add(Conversation.Participants[i]);
                }
            }

            // Opretter MessageViewModels ud fra samtalens beskeder
            Messages = new ObservableCollection<MessageViewModel>(conversation.Messages.Select(m => new MessageViewModel(m, currentUser)));

            // Lytter efter nye beskeder i samtalen
            conversation.Messages.CollectionChanged += OnMessagesChanged;
        }

        // Kaldes når beskedlisten ændrer sig
        private void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Stopper metoden hvis der ikke er nye beskeder
            if (e.NewItems == null)
                return;

            // Sørger for at UI-opdateringen sker på UI-tråden
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Gennemgår alle nye beskeder
                foreach (Message message in e.NewItems)
                {
                    // Opretter en MessageViewModel og tilføjer den til UI-listen
                    Messages.Add(new MessageViewModel(message, _currentUser));
                }
            });
        }
    }
}