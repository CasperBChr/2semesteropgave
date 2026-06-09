using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.ViewModels
{
	public class ConversationViewModel
	{
		public Conversation Conversation { get; }
		public ObservableCollection<MessageViewModel> Messages { get; }
		public ObservableCollection<User> OtherParticipants { get; }
		User _currentUser;


		public ConversationViewModel(Conversation conversation, User currentUser)
		{
			Conversation = conversation;
			_currentUser = currentUser;

			Debug.WriteLine($"UI Conversation: {conversation.GetHashCode()}");

			OtherParticipants = new ObservableCollection<User>();

			for (int i = 0; i < Conversation.Participants.Count; i++)
			{
				if (Conversation.Participants[i].Id != _currentUser.Id)
				{
					OtherParticipants.Add(Conversation.Participants[i]);
				}
			}

			Messages = new ObservableCollection<MessageViewModel>(conversation.Messages.Select(m => new MessageViewModel(m, currentUser)));	

			conversation.Messages.CollectionChanged += OnMessagesChanged;
		}

		private void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems == null)
				return;

			Application.Current.Dispatcher.Invoke(() =>
			{
				foreach (Message message in e.NewItems)
				{
					Messages.Add(new MessageViewModel(message, _currentUser));
				}
			});
		}
	}
}
