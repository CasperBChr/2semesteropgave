using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Services
{
	// Serviceklasse der håndterer opdatering af unread badge
	/// <summary>
	/// Kodet af Martin
	/// </summary>
	public class UnreadBadgeServices
    {
        // Service der bruges til at hente antal ulæste samtaler
        ConversationServices _conversationServices;

        // Service der bruges til at hente den nuværende bruger
        UserServices _userServices;

        // Event der kaldes, når antal ulæste beskeder ændrer sig
        public event Action<int>? UnreadCountChanged;

        // Constructor der modtager ConversationServices og UserServices
        public UnreadBadgeServices(ConversationServices conversationServices, UserServices userServices)
        {
            // Gemmer ConversationServices, så den kan bruges i Refresh
            _conversationServices = conversationServices;

            // Gemmer UserServices, så den nuværende bruger kan hentes
            _userServices = userServices;
        }

        // Opdaterer antal ulæste samtaler
        public void Refresh()
        {
            // Henter antal ulæste samtaler for den nuværende bruger
            int count = _conversationServices.GetUnreadConversationCount(_userServices.CurrentUser);

            // Kalder eventet og sender det nye antal ulæste samtaler med
            UnreadCountChanged?.Invoke(count);
        }
    }
}