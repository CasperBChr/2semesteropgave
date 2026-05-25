using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Utils
{
	public class FakeConversation
	{
		public void ContinueConversationBot(Conversation conversation, User botUser) {
			Thread thread = new Thread(() => { RunFakeConversations(conversation, botUser);  });
		}

		public void StartConversationBot(User targetUser, User botUser)
		{
			Thread thread = new Thread(() => {
				Conversation conversation = CreateFakeConversation(targetUser, botUser);
				RunFakeConversations(conversation, botUser); 
			});
		}

		public Conversation CreateFakeConversation(User targetUser, User botUser) 
		{
			return new Conversation();
		}

		public void RunFakeConversations(Conversation conversation, User botUser) 
		{
			Thread.Sleep(5000);
			conversation.Messages.Add(new Message("Hey", conversation));
		}
	}
}
