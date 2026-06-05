using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Services
{
	public class UnreadBadgeServices
	{
		ConversationServices _conversationServices;
		UserServices _userServices;

		public event Action<int>? UnreadCountChanged;

		public UnreadBadgeServices(ConversationServices conversationServices, UserServices userServices)
		{
			_conversationServices = conversationServices;
			_userServices = userServices;
		}

		public void Refresh()
		{
			int count = _conversationServices.GetUnreadConversationCount(_userServices.CurrentUser);
			UnreadCountChanged?.Invoke(count);
		}
	}
}
