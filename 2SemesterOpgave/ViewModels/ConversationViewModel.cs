using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.ViewModels
{
	public class ConversationViewModel
	{
		public Conversation Conversation { get; }

		public ObservableCollection<MessageViewModel> Messages { get; }

		User _currentUser;

		public List<User> OtherParticipants { get; }

		public ConversationViewModel(Conversation conversation, User currentUser)
		{
			Conversation = conversation;
			_currentUser = currentUser;

			//Messages = new ObservableCollection<MessageViewModel>(conversation.Messages.Select(m => new MessageViewModel(m, currentUser)));

			OtherParticipants = new List<User>();

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
			{
				return;
			}

			foreach (Message message in e.NewItems)
			{
				Messages.Add(new MessageViewModel(message, _currentUser));
			}
		}
	}
}
