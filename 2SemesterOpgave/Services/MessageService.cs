using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services.Interfaces;

namespace _2SemesterOpgave.Services
{
	public class MessageService : IMessageService
	{
		public Message SendMessage(Conversation conversation, User sender, string text)
		{
			Message message = new Message(text, conversation, sender, DateTime.Now);

			conversation.Messages.Add(message);
			conversation.LastActive = DateTime.Now;

			return message;
		}
	}
}
