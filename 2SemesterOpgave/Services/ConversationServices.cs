using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class ConversationServices
	{
		ConversationRepository _conversationRepository;
		UserServices _userServices;

		// In-memory read-status: conversation.Id -> tidspunkt for sidst læst
		Dictionary<int, DateTime> _lastReadAt = new Dictionary<int, DateTime>();

		public ConversationServices(ConversationRepository conversationRepository, UserServices userServices)
		{
			_conversationRepository = conversationRepository;
			_userServices = userServices;
		}

		public Conversation GetOrCreateConversation(User userA, User userB)
		{
			ConversationDTO? existing = _conversationRepository.GetConversationBetween(userA.Id, userB.Id);
			if (existing != null) return Map(existing);

			int conversationId = _conversationRepository.CreateConversation();
			_conversationRepository.AddParticipant(conversationId, userA.Id);
			_conversationRepository.AddParticipant(conversationId, userB.Id);

			ConversationDTO? created = _conversationRepository.GetConversationById(conversationId);
			return Map(created!);
		}

		public ObservableCollection<Conversation> GetConversationsForUser(User user)
		{
			IEnumerable<ConversationDTO> dtos = _conversationRepository.GetConversationsByUserId(user.Id);
			ObservableCollection<Conversation> conversations = new();
			foreach (ConversationDTO dto in dtos)
				conversations.Add(Map(dto));
			return conversations;
		}

		public void SendMessage(Conversation conversation, User sender, string text)
		{
			if (string.IsNullOrWhiteSpace(text)) return;

			MessageDTO messageDTO = new MessageDTO
			{
				Text = text,
				SenderId = sender.Id,
				ConversationId = conversation.Id
			};

			_conversationRepository.AddMessage(messageDTO);

			Message message = new Message(text, conversation, sender, DateTime.Now);
			conversation.Messages.Add(message);
			conversation.LastActive = DateTime.Now;
		}

		// Tæller in-memory conversations med ulæste beskeder fra andre
		public int GetUnreadConversationCount(User user)
		{
			int count = 0;

			foreach (Conversation conv in _userServices.Conversations)
			{
				if (conv.Messages.Count == 0) continue;

				// Find nyeste besked fra en anden end currentUser
				Message? newestFromOther = conv.Messages
					.Where(m => m.Sender.Id != user.Id)
					.LastOrDefault();

				if (newestFromOther == null) continue;

				bool hasRead = _lastReadAt.TryGetValue(conv.Id, out DateTime lastRead);

				if (!hasRead || newestFromOther.Timestamp > lastRead)
					count++;
			}

			return count;
		}

		public void MarkConversationAsRead(Conversation conversation, User user)
		{
			_lastReadAt[conversation.Id] = DateTime.Now;

			if (conversation.Id > 0)
			{
				_conversationRepository.MarkAsRead(user.Id, conversation.Id);
			}
		}

		Conversation Map(ConversationDTO dto)
		{
			List<User> participants = dto.ParticipantIds
				.Select(id => _userServices.GetById(id))
				.Where(u => u != null)
				.Select(u => u!)
				.ToList();

			ObservableCollection<Message> messages = new();

			Conversation conversation = new Conversation
			{
				Id = dto.Id,
				CreationTime = dto.CreatedAt,
				LastActive = dto.CreatedAt,
				Participants = participants,
				Messages = messages
			};

			foreach (MessageDTO m in dto.Messages)
			{
				User? sender = _userServices.GetById(m.SenderId);
				if (sender == null) continue;
				messages.Add(new Message(m.Text, conversation, sender, m.CreatedAt));
			}

			return conversation;
		}
	}
}
