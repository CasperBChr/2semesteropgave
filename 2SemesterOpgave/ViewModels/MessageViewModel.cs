using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.ViewModels
{
	public class MessageViewModel
	{
		public Message Message { get; }

		public string Text => Message.Text;

		public string SenderName => Message.Sender.Username;

		public DateTime Timestamp => Message.Timestamp;

		public HorizontalAlignment Alignment { get; }

		public SolidColorBrush Background { get; }

		public MessageViewModel(Message message, User currentUser)
		{
			Message = message;
			bool isOwnMessage = message.Sender.Id == currentUser.Id;

			if(isOwnMessage) 
			{
				Alignment = HorizontalAlignment.Right;
				Background = Brushes.HotPink;
			}
			else 
			{
				Alignment = HorizontalAlignment.Left;
				Background = Brushes.LightBlue;
			}
		}
	}
}
