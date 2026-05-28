using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Services.Interfaces
{
	public interface IMessageService
	{
		Message SendMessage(Conversation conversation, User sender, string text);
	}
}
