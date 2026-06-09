using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Utils.Messages
{
	public class MessageUIModel
	{
		public Message Message { get; }

		public HorizontalAlignment Alignment { get; set; }
		public SolidColorBrush Background { get; set; }

		public string Text;
		public string Sender;
		public DateTime Timestamp;

		public MessageUIModel(Message message, User currentUser)
		{
			Message = message;
			Sender = Message.Sender.Username;
			Text = Message.Text;
			Timestamp = Message.Timestamp;

			bool isOwn = message.Sender.Id == currentUser.Id;

			if(isOwn) 
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
