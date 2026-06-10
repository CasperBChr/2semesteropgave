using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
using Microsoft.VisualBasic;


namespace _2SemesterOpgave.Utils
{
	public class FakeConversation
	{
		bool _isShuttingDown = false;
		static readonly object _lock = new object();
		public event Action<Conversation> OnNewMessage;

		public void StartFakeBots(List<User> botUsers, User currentUser, ConversationServices conversationServices)
		{
			Random rng = new Random();
			List<User> randomBots = PickRandom(botUsers, currentUser, 3, rng);

			foreach (User bot in randomBots)
			{
				Conversation conversation = conversationServices.GetOrCreateConversation(currentUser, bot);
				ContinueConversationBot(conversation, bot, conversationServices, rng.Next(3000, 8000));
			}
		}
		public void ContinueConversationBot(Conversation conversation, User botUser, ConversationServices conversationServices, int intervalMs = 5000)
		{
			Thread thread = new Thread(() =>
			{
				RunFakeConversations(conversation, botUser, conversationServices, intervalMs);
			});
			thread.IsBackground = true;
			thread.Start();
		}


		List<User> PickRandom(List<User> users, User exclude, int count, Random rng)
		{
			List<User> pool = new List<User>();
			for (int i = 0; i < users.Count; i++)
			{
				if (users[i].Id != exclude.Id)
				{
					pool.Add(users[i]);
				}
			}

			List<User> result = new List<User>();
			while (result.Count < count && pool.Count > 0)
			{
				int index = rng.Next(pool.Count);
				result.Add(pool[index]);
				pool.RemoveAt(index);
			}
			return result;
		}

		void RunFakeConversations(Conversation conversation, User botUser, ConversationServices conversationServices, int intervalMs)
		{
			Random rng = new Random();

			while (!_isShuttingDown)
			{
				Thread.Sleep(intervalMs);

				lock (_lock)
				{
					conversationServices.SendMessage(conversation, botUser, RandomMessageText(rng));
					OnNewMessage?.Invoke(conversation);
				}

				intervalMs = rng.Next(3000, 10000);
			}
		}

		public string RandomMessageText(Random rng)
		{
			string[] randomText = new string[]
			{
				"Er den på lager?????",
				"Haaaaallo",
				"Er den small en rigtig small?",
				"Kan du gøre det billigere?",
				"Kan du sende flere billeder????",
				"Hvor hurtigt kan du sende?",
				"Hvor brugt er den?"
			};
			return randomText[rng.Next(randomText.Length)];
		}

		public string RandomMessageText() => RandomMessageText(new Random());

		public void Shutdown()
		{
			_isShuttingDown = true;
		}
	}
}
