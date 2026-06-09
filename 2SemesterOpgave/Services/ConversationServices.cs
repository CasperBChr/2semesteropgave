using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class ConversationServices
	{
		private readonly ConversationRepository _conversationRepository;
		private readonly UserServices _userServices;

		// Cache sikrer samme instance bruges i hele UI
		private readonly Dictionary<int, Conversation> _conversationCache = new();

		// Track read state pr user + conversation
		private readonly Dictionary<int, DateTime> _lastReadAt = new();

		public Conversation CurrentConversation { get; set; }

		public ConversationServices(
			ConversationRepository conversationRepository,
			UserServices userServices)
		{
			_conversationRepository = conversationRepository;
			_userServices = userServices;
		}

		// ─────────────────────────────────────────────
		// GET OR CREATE
		// ─────────────────────────────────────────────
		public Conversation GetOrCreateConversation(User userA, User userB)
		{
			ConversationDTO? existing =
				_conversationRepository.GetConversationBetween(userA.Id, userB.Id);

			if (existing != null)
				return Map(existing);

			int conversationId = _conversationRepository.CreateConversation();

			_conversationRepository.AddParticipant(conversationId, userA.Id);
			_conversationRepository.AddParticipant(conversationId, userB.Id);

			ConversationDTO? created =
				_conversationRepository.GetConversationById(conversationId);

			return Map(created!);
		}

		// ─────────────────────────────────────────────
		// GET USER CONVERSATIONS
		// ─────────────────────────────────────────────
		public ObservableCollection<Conversation> GetConversationsForUser(User user)
		{
			IEnumerable<ConversationDTO> dtos =
				_conversationRepository.GetConversationsByUserId(user.Id);

			ObservableCollection<Conversation> conversations = new();

			foreach (ConversationDTO dto in dtos)
			{
				conversations.Add(Map(dto));
			}

			return conversations;
		}

		// ─────────────────────────────────────────────
		// SEND MESSAGE (REAL-TIME FRIENDLY)
		// ─────────────────────────────────────────────
		public void SendMessage(Conversation conversation, User sender, string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return;

			MessageDTO dto = new()
			{
				Text = text,
				SenderId = sender.Id,
				ConversationId = conversation.Id
			};

			_conversationRepository.AddMessage(dto);

			Message message = new(
				text,
				conversation,
				sender,
				DateTime.Now);

			// IMPORTANT: shared instance (UI listens here)
			conversation.Messages.Add(message);
			conversation.LastActive = DateTime.Now;
		}

		// ─────────────────────────────────────────────
		// UNREAD COUNT (GLOBAL BADGE)
		// ─────────────────────────────────────────────
		public int GetUnreadConversationCount(User user)
		{
			int count = 0;

			foreach (Conversation conv in _userServices.Conversations)
			{
				var lastOther = conv.Messages
					.LastOrDefault(m => m.Sender.Id != user.Id);

				if (lastOther == null)
					continue;

				DateTime lastRead = GetLastReadTime(conv.Id, user);

				if (lastOther.Timestamp > lastRead)
					count++;
			}

			return count;
		}

		// ─────────────────────────────────────────────
		// MARK AS READ
		// ─────────────────────────────────────────────
		public void MarkConversationAsRead(Conversation conversation, User user)
		{
			_lastReadAt[conversation.Id] = DateTime.Now;

			if (conversation.Id > 0)
			{
				_conversationRepository.MarkAsRead(user.Id, conversation.Id);
			}
		}

		public DateTime GetLastReadTime(int conversationId, User user)
		{
			if (_lastReadAt.TryGetValue(conversationId, out DateTime time))
				return time;

			return DateTime.MinValue;
		}

		// ─────────────────────────────────────────────
		// MAP DTO -> DOMAIN (WITH CACHE)
		// ─────────────────────────────────────────────
		private Conversation Map(ConversationDTO dto)
		{
			if (_conversationCache.TryGetValue(dto.Id, out Conversation cached))
				return cached;

			List<User> participants = dto.ParticipantIds
				.Select(id => _userServices.GetById(id))
				.Where(u => u != null)
				.Select(u => u!)
				.ToList();

			ObservableCollection<Message> messages = new();

			Conversation conversation = new()
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
				if (sender == null)
					continue;

				messages.Add(new Message(
					m.Text,
					conversation,
					sender,
					m.CreatedAt));
			}

			_conversationCache[dto.Id] = conversation;

			return conversation;
		}










		//private readonly ConversationRepository _conversationRepository;
		//private readonly UserServices _userServices;

		//// 🔥 IMPORTANT: Cache så alle bruger samme Conversation-instans
		//private readonly Dictionary<int, Conversation> _conversationCache = new();

		//public Conversation CurrentConversation { get; set; }

		//private readonly Dictionary<int, DateTime> _lastReadAt = new();

		//public ConversationServices(
		//	ConversationRepository conversationRepository,
		//	UserServices userServices)
		//{
		//	_conversationRepository = conversationRepository;
		//	_userServices = userServices;
		//}

		//public Conversation GetOrCreateConversation(User userA, User userB)
		//{
		//	ConversationDTO? existing =
		//		_conversationRepository.GetConversationBetween(userA.Id, userB.Id);

		//	if (existing != null)
		//		return Map(existing);

		//	int conversationId = _conversationRepository.CreateConversation();

		//	_conversationRepository.AddParticipant(conversationId, userA.Id);
		//	_conversationRepository.AddParticipant(conversationId, userB.Id);

		//	ConversationDTO? created =
		//		_conversationRepository.GetConversationById(conversationId);

		//	return Map(created!);
		//}

		//public ObservableCollection<Conversation> GetConversationsForUser(User user)
		//{
		//	IEnumerable<ConversationDTO> dtos =
		//		_conversationRepository.GetConversationsByUserId(user.Id);

		//	ObservableCollection<Conversation> conversations = new();

		//	foreach (ConversationDTO dto in dtos)
		//	{
		//		conversations.Add(Map(dto));
		//	}

		//	return conversations;
		//}

		//public void SendMessage(Conversation conversation, User sender, string text)
		//{
		//	if (string.IsNullOrWhiteSpace(text))
		//		return;

		//	MessageDTO messageDTO = new()
		//	{
		//		Text = text,
		//		SenderId = sender.Id,
		//		ConversationId = conversation.Id
		//	};

		//	_conversationRepository.AddMessage(messageDTO);

		//	Message message = new Message(text, conversation, sender, DateTime.Now);

		//	// ⚠️ vigtigt: samme instance som UI lytter på
		//	conversation.Messages.Add(message);
		//	conversation.LastActive = DateTime.Now;
		//}

		//public int GetUnreadConversationCount(User user)
		//{
		//	int count = 0;

		//	foreach (Conversation conv in _userServices.Conversations)
		//	{
		//		if (conv.Messages.Count == 0)
		//			continue;

		//		Message? newestFromOther =
		//			conv.Messages.LastOrDefault(m => m.Sender.Id != user.Id);

		//		if (newestFromOther == null)
		//			continue;

		//		bool hasRead = _lastReadAt.TryGetValue(conv.Id, out DateTime lastRead);

		//		if (!hasRead || newestFromOther.Timestamp > lastRead)
		//			count++;
		//	}

		//	return count;
		//}

		//public void MarkConversationAsRead(Conversation conversation, User user)
		//{
		//	_lastReadAt[conversation.Id] = DateTime.Now;

		//	if (conversation.Id > 0)
		//	{
		//		_conversationRepository.MarkAsRead(user.Id, conversation.Id);
		//	}
		//}

		//// =========================================================
		//// 🔥 CRITICAL FIX: SINGLE SOURCE OF TRUTH FOR CONVERSATIONS
		//// =========================================================
		//private Conversation Map(ConversationDTO dto)
		//{
		//	// 1. Returnér cached instance hvis den findes
		//	if (_conversationCache.TryGetValue(dto.Id, out Conversation cached))
		//		return cached;

		//	List<User> participants = dto.ParticipantIds
		//		.Select(id => _userServices.GetById(id))
		//		.Where(u => u != null)
		//		.Select(u => u!)
		//		.ToList();

		//	ObservableCollection<Message> messages = new();

		//	Conversation conversation = new()
		//	{
		//		Id = dto.Id,
		//		CreationTime = dto.CreatedAt,
		//		LastActive = dto.CreatedAt,
		//		Participants = participants,
		//		Messages = messages
		//	};

		//	// 2. Map messages
		//	foreach (MessageDTO m in dto.Messages)
		//	{
		//		User? sender = _userServices.GetById(m.SenderId);
		//		if (sender == null)
		//			continue;

		//		messages.Add(new Message(
		//			m.Text,
		//			conversation,
		//			sender,
		//			m.CreatedAt));
		//	}

		//	// 3. CACHE IT (THIS IS THE FIX)
		//	_conversationCache[dto.Id] = conversation;

		//	return conversation;
		//}






















		//ConversationRepository _conversationRepository;
		//UserServices _userServices;

		//public Conversation CurrentConversation { get; set; }
		//Dictionary<int, Conversation> _conversationCache = new Dictionary<int, Conversation>();
		//Dictionary<int, DateTime> _lastReadAt = new Dictionary<int, DateTime>();

		//public ConversationServices(ConversationRepository conversationRepository, UserServices userServices)
		//{
		//	_conversationRepository = conversationRepository;
		//	_userServices = userServices;
		//}

		//public Conversation GetOrCreateConversation(User userA, User userB)
		//{
		//	ConversationDTO? existing = _conversationRepository.GetConversationBetween(userA.Id, userB.Id);
		//	if (existing != null)
		//	{
		//		return Map(existing);
		//	}
		//	int conversationId = _conversationRepository.CreateConversation();
		//	_conversationRepository.AddParticipant(conversationId, userA.Id);
		//	_conversationRepository.AddParticipant(conversationId, userB.Id);

		//	ConversationDTO? created = _conversationRepository.GetConversationById(conversationId);
		//	return Map(created!);
		//}

		//public ObservableCollection<Conversation> GetConversationsForUser(User user)
		//{
		//	IEnumerable<ConversationDTO> dtos = _conversationRepository.GetConversationsByUserId(user.Id);
		//	ObservableCollection<Conversation> conversations = new ObservableCollection<Conversation>();
		//	foreach (ConversationDTO dto in dtos)
		//	{
		//		conversations.Add(Map(dto));
		//	}
		//	return conversations;
		//}

		//public void SendMessage(Conversation conversation, User sender, string text)
		//{
		//	if (string.IsNullOrWhiteSpace(text))
		//	{
		//		return;
		//	}
		//	Debug.WriteLine($"SendMessage Conversation: {conversation.GetHashCode()}");

		//	MessageDTO messageDTO = new MessageDTO
		//	{
		//		Text = text,
		//		SenderId = sender.Id,
		//		ConversationId = conversation.Id
		//	};

		//	_conversationRepository.AddMessage(messageDTO);

		//	Message message = new Message(text, conversation, sender, DateTime.Now);
		//	conversation.Messages.Add(message);
		//	conversation.LastActive = DateTime.Now;
		//}

		//// Tæller in-memory conversations med ulæste beskeder fra andre
		//public int GetUnreadConversationCount(User user)
		//{
		//	int count = 0;

		//	foreach (Conversation conv in _userServices.Conversations)
		//	{
		//		if (conv.Messages.Count == 0)
		//		{
		//			continue;
		//		}
		//		// Find nyeste besked fra en anden end currentUser
		//		Message? newestFromOther = conv.Messages.Where(m => m.Sender.Id != user.Id).LastOrDefault();

		//		if (newestFromOther == null)
		//		{
		//			continue;
		//		}
		//		bool hasRead = _lastReadAt.TryGetValue(conv.Id, out DateTime lastRead);

		//		if (!hasRead || newestFromOther.Timestamp > lastRead)
		//		{
		//			count++;
		//		}
		//	}
		//	return count;
		//}

		//public void MarkConversationAsRead(Conversation conversation, User user)
		//{
		//	_lastReadAt[conversation.Id] = DateTime.Now;

		//	if (conversation.Id > 0)
		//	{
		//		_conversationRepository.MarkAsRead(user.Id, conversation.Id);
		//	}
		//}

		////Conversation Map(ConversationDTO dto)
		////{
		////	List<User> participants = dto.ParticipantIds.Select(id => _userServices.GetById(id)).Where(u => u != null).Select(u => u!).ToList();

		////	ObservableCollection<Message> messages = new();

		////	Conversation conversation = new Conversation
		////	{
		////		Id = dto.Id,
		////		CreationTime = dto.CreatedAt,
		////		LastActive = dto.CreatedAt,
		////		Participants = participants,
		////		Messages = messages
		////	};

		////	foreach (MessageDTO m in dto.Messages)
		////	{
		////		User? sender = _userServices.GetById(m.SenderId);
		////		if (sender == null) continue;
		////		messages.Add(new Message(m.Text, conversation, sender, m.CreatedAt));
		////	}

		////	return conversation;
		////}


		//Conversation Map(ConversationDTO dto)
		//{
		//	if (_conversationCache.TryGetValue(dto.Id, out Conversation existing))
		//	{
		//		return existing;
		//	}

		//	List<User> participants =
		//		dto.ParticipantIds
		//		   .Select(id => _userServices.GetById(id))
		//		   .Where(u => u != null)
		//		   .Select(u => u!)
		//		   .ToList();

		//	ObservableCollection<Message> messages = new();

		//	Conversation conversation = new Conversation
		//	{
		//		Id = dto.Id,
		//		CreationTime = dto.CreatedAt,
		//		LastActive = dto.CreatedAt,
		//		Participants = participants,
		//		Messages = messages
		//	};

		//	foreach (MessageDTO m in dto.Messages)
		//	{
		//		User? sender = _userServices.GetById(m.SenderId);

		//		if (sender == null)
		//			continue;

		//		messages.Add(
		//			new Message(
		//				m.Text,
		//				conversation,
		//				sender,
		//				m.CreatedAt));
		//	}

		//	_conversationCache[dto.Id] = conversation;

		//	return conversation;
		//}
	}
}
