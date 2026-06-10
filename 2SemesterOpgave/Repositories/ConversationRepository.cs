using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
	public class ConversationRepository
	{
		IDatabaseFactory _db;

		public ConversationRepository(IDatabaseFactory db)
		{
			_db = db;
		}

		public int CreateConversation()
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO Conversations DEFAULT VALUES; SELECT last_insert_rowid();";
				return Convert.ToInt32(command.ExecuteScalar());

		}

		public void AddParticipant(int conversationId, int userId)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO ConversationParticipants (conversation_id, user_id) VALUES (@ConversationId, @UserId)";

				DbParameter convParam = command.CreateParameter();
				convParam.ParameterName = "@ConversationId";
				convParam.DbType = DbType.Int32;
				convParam.Value = conversationId;
				command.Parameters.Add(convParam);

				DbParameter userParam = command.CreateParameter();
				userParam.ParameterName = "@UserId";
				userParam.DbType = DbType.Int32;
				userParam.Value = userId;
				command.Parameters.Add(userParam);

				command.ExecuteNonQuery();

		}

		public int AddMessage(MessageDTO message)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText =
					"INSERT INTO Messages (text, sender_id, conversation_id) " +
					"VALUES (@Text, @SenderId, @ConversationId); " +
					"SELECT last_insert_rowid();";

				DbParameter textParam = command.CreateParameter();
				textParam.ParameterName = "@Text";
				textParam.DbType = DbType.String;
				textParam.Value = message.Text;
				command.Parameters.Add(textParam);

				DbParameter senderParam = command.CreateParameter();
				senderParam.ParameterName = "@SenderId";
				senderParam.DbType = DbType.Int32;
				senderParam.Value = message.SenderId;
				command.Parameters.Add(senderParam);

				DbParameter convParam = command.CreateParameter();
				convParam.ParameterName = "@ConversationId";
				convParam.DbType = DbType.Int32;
				convParam.Value = message.ConversationId;
				command.Parameters.Add(convParam);

				return Convert.ToInt32(command.ExecuteScalar());
		}

		public void MarkAsRead(int userId, int conversationId)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = @"
            INSERT INTO ConversationReadStatus (user_id, conversation_id, last_read_at)
            VALUES (@UserId, @ConversationId, datetime('now'))
            ON CONFLICT(user_id, conversation_id) 
            DO UPDATE SET last_read_at = datetime('now')";

				DbParameter userParam = command.CreateParameter();
				userParam.ParameterName = "@UserId";
				userParam.DbType = DbType.Int32;
				userParam.Value = userId;
				command.Parameters.Add(userParam);

				DbParameter convParam = command.CreateParameter();
				convParam.ParameterName = "@ConversationId";
				convParam.DbType = DbType.Int32;
				convParam.Value = conversationId;
				command.Parameters.Add(convParam);

				command.ExecuteNonQuery();
		}

		public int GetUnreadConversationCount(int userId)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = @"
					SELECT COUNT(DISTINCT c.id)
					FROM Conversations c
					JOIN ConversationParticipants cp ON cp.conversation_id = c.id AND cp.user_id = @UserId
					JOIN Messages m ON m.conversation_id = c.id
					LEFT JOIN ConversationReadStatus rs 
					ON rs.conversation_id = c.id AND rs.user_id = @UserId
					WHERE m.sender_id != @UserId
					AND (rs.last_read_at IS NULL OR m.created_at > rs.last_read_at)";

				DbParameter userParam = command.CreateParameter();
				userParam.ParameterName = "@UserId";
				userParam.DbType = DbType.Int32;
				userParam.Value = userId;
				command.Parameters.Add(userParam);

				return Convert.ToInt32(command.ExecuteScalar());
		}

		public IEnumerable<ConversationDTO> GetConversationsByUserId(int userId)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = @"
                    SELECT c.id, c.created_at
                    FROM Conversations c
                    JOIN ConversationParticipants cp ON cp.conversation_id = c.id
                    WHERE cp.user_id = @UserId";

				DbParameter userParam = command.CreateParameter();
				userParam.ParameterName = "@UserId";
				userParam.DbType = DbType.Int32;
				userParam.Value = userId;
				command.Parameters.Add(userParam);

				List<ConversationDTO> conversations = new();

				using DbDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					conversations.Add(new ConversationDTO
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at")))
					});
				}

				foreach (ConversationDTO conv in conversations)
				{
					conv.ParticipantIds = GetParticipantIds(conv.Id);
					conv.Messages = GetMessages(conv.Id);
				}

				return conversations;
		}

		public ConversationDTO? GetConversationById(int conversationId)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT id, created_at FROM Conversations WHERE id = @Id";

				DbParameter idParam = command.CreateParameter();
				idParam.ParameterName = "@Id";
				idParam.DbType = DbType.Int32;
				idParam.Value = conversationId;
				command.Parameters.Add(idParam);

				using DbDataReader reader = command.ExecuteReader();

				if (!reader.Read()) return null;

				ConversationDTO conv = new ConversationDTO
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at")))
				};

				reader.Close();

				conv.ParticipantIds = GetParticipantIds(conv.Id);
				conv.Messages = GetMessages(conv.Id);

				return conv;
		}

		public ConversationDTO? GetConversationBetween(int userIdA, int userIdB)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = @"
                    SELECT c.id, c.created_at
                    FROM Conversations c
                    JOIN ConversationParticipants cpA ON cpA.conversation_id = c.id AND cpA.user_id = @UserA
                    JOIN ConversationParticipants cpB ON cpB.conversation_id = c.id AND cpB.user_id = @UserB";

				DbParameter paramA = command.CreateParameter();
				paramA.ParameterName = "@UserA";
				paramA.DbType = DbType.Int32;
				paramA.Value = userIdA;
				command.Parameters.Add(paramA);

				DbParameter paramB = command.CreateParameter();
				paramB.ParameterName = "@UserB";
				paramB.DbType = DbType.Int32;
				paramB.Value = userIdB;
				command.Parameters.Add(paramB);

				using DbDataReader reader = command.ExecuteReader();

				if (!reader.Read()) return null;

				ConversationDTO conv = new ConversationDTO
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at")))
				};

				reader.Close();

				conv.ParticipantIds = GetParticipantIds(conv.Id);
				conv.Messages = GetMessages(conv.Id);

				return conv;
		}

		List<int> GetParticipantIds(int conversationId)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT user_id FROM ConversationParticipants WHERE conversation_id = @ConversationId";

				DbParameter param = command.CreateParameter();
				param.ParameterName = "@ConversationId";
				param.DbType = DbType.Int32;
				param.Value = conversationId;
				command.Parameters.Add(param);

				List<int> ids = new();
				using DbDataReader reader = command.ExecuteReader();
				while (reader.Read())
					ids.Add(reader.GetInt32(0));

				return ids;

		}

		List<MessageDTO> GetMessages(int conversationId)
		{
			using SqliteConnection connection = _db.CreateConnection();
			using DbCommand command = connection.CreateCommand();
			command.CommandText = @"
                    SELECT id, text, created_at, sender_id, conversation_id
                    FROM Messages
                    WHERE conversation_id = @ConversationId
                    ORDER BY created_at ASC";

				DbParameter param = command.CreateParameter();
				param.ParameterName = "@ConversationId";
				param.DbType = DbType.Int32;
				param.Value = conversationId;
				command.Parameters.Add(param);

				List<MessageDTO> messages = new();
				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					messages.Add(new MessageDTO
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						Text = reader.GetString(reader.GetOrdinal("text")),
						CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at"))),
						SenderId = reader.GetInt32(reader.GetOrdinal("sender_id")),
						ConversationId = reader.GetInt32(reader.GetOrdinal("conversation_id"))
					});
				}

				return messages;
		}
	}
}
