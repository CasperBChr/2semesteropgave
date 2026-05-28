using _2SemesterOpgave.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Threading;


namespace _2SemesterOpgave.Utils
{
	public class FakeConversation
	{
		bool isShuttingDown = false;
		public event Action<Conversation> OnNewMessage;
		
		//public FakeConversation()
		//{
           
  //      }

		public void ContinueConversationBot(Conversation conversation, User botUser) {
			Thread thread = new Thread(() => { RunFakeConversations(conversation, botUser);  });
			thread.IsBackground = true;
			thread.Start();
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
			return new Conversation(new List<User>() { targetUser, botUser });
		}

		public void RunFakeConversations(Conversation conversation, User botUser) 
		{
			while(!isShuttingDown)
			{
				Thread.Sleep(5000);
				OnNewMessage?.Invoke(conversation);

            }
		}

		public string RandomMessageText()
		{
			string[] randomText = new string[] { "Er den på lager?????", "Haaaaallo", "Er den small en rigtig small?", "Kan du gøre det billigere?", "Kan du sende flere billeder????", "Hvor hurtigt kan du sende?", "Hvor brugt er den?" };
			Random random = new Random();

			return randomText[random.Next(0, randomText.Length)];
		}
	}
}
