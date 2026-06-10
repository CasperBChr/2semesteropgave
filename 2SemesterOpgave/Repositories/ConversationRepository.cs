using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ConversationDTO og MessageDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
    // Repositoryklasse der håndterer databasekald for samtaler og beskeder
    public class ConversationRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public ConversationRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Opretter en ny samtale i databasen og returnerer dens id
        public int CreateConversation()
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der opretter en tom samtale og henter id'et på den nye række
            command.CommandText = "INSERT INTO Conversations DEFAULT VALUES; SELECT last_insert_rowid();";

            // Kører kommandoen og returnerer det nye conversation id
            return Convert.ToInt32(command.ExecuteScalar());
        }

        // Tilføjer en bruger som deltager i en samtale
        public void AddParticipant(int conversationId, int userId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der tilføjer en deltager til en samtale
            command.CommandText = "INSERT INTO ConversationParticipants (conversation_id, user_id) VALUES (@ConversationId, @UserId)";

            // Opretter parameter til samtalens id
            IDbDataParameter convParam = command.CreateParameter();
            convParam.ParameterName = "@ConversationId";
            convParam.DbType = DbType.Int32;
            convParam.Value = conversationId;
            command.Parameters.Add(convParam);

            // Opretter parameter til brugerens id
            IDbDataParameter userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.DbType = DbType.Int32;
            userParam.Value = userId;
            command.Parameters.Add(userParam);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Tilføjer en besked til en samtale og returnerer beskedens nye id
        public int AddMessage(MessageDTO message)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter en besked og henter id'et på den nye besked
            command.CommandText =
                    "INSERT INTO Messages (text, sender_id, conversation_id) " +
                    "VALUES (@Text, @SenderId, @ConversationId); " +
                    "SELECT last_insert_rowid();";

            // Opretter parameter til beskedens tekst
            IDbDataParameter textParam = command.CreateParameter();
            textParam.ParameterName = "@Text";
            textParam.DbType = DbType.String;
            textParam.Value = message.Text;
            command.Parameters.Add(textParam);

            // Opretter parameter til afsenderens id
            IDbDataParameter senderParam = command.CreateParameter();
            senderParam.ParameterName = "@SenderId";
            senderParam.DbType = DbType.Int32;
            senderParam.Value = message.SenderId;
            command.Parameters.Add(senderParam);

            // Opretter parameter til samtalens id
            IDbDataParameter convParam = command.CreateParameter();
            convParam.ParameterName = "@ConversationId";
            convParam.DbType = DbType.Int32;
            convParam.Value = message.ConversationId;
            command.Parameters.Add(convParam);

            // Kører kommandoen og returnerer id'et på den nye besked
            return Convert.ToInt32(command.ExecuteScalar());
        }

        // Marker en samtale som læst for en bestemt bruger
        public void MarkAsRead(int userId, int conversationId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter eller opdaterer hvornår brugeren sidst læste samtalen
            command.CommandText = @"
            INSERT INTO ConversationReadStatus (user_id, conversation_id, last_read_at)
            VALUES (@UserId, @ConversationId, datetime('now'))
            ON CONFLICT(user_id, conversation_id) 
            DO UPDATE SET last_read_at = datetime('now')";

            // Opretter parameter til brugerens id
            IDbDataParameter userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.DbType = DbType.Int32;
            userParam.Value = userId;
            command.Parameters.Add(userParam);

            // Opretter parameter til samtalens id
            IDbDataParameter convParam = command.CreateParameter();
            convParam.ParameterName = "@ConversationId";
            convParam.DbType = DbType.Int32;
            convParam.Value = conversationId;
            command.Parameters.Add(convParam);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Henter antal samtaler hvor brugeren har ulæste beskeder
        public int GetUnreadConversationCount(int userId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der tæller samtaler med ulæste beskeder for brugeren
            command.CommandText = @"
					SELECT COUNT(DISTINCT c.id)
					FROM Conversations c
					JOIN ConversationParticipants cp ON cp.conversation_id = c.id AND cp.user_id = @UserId
					JOIN Messages m ON m.conversation_id = c.id
					LEFT JOIN ConversationReadStatus rs 
					ON rs.conversation_id = c.id AND rs.user_id = @UserId
					WHERE m.sender_id != @UserId
					AND (rs.last_read_at IS NULL OR m.created_at > rs.last_read_at)";

            // Opretter parameter til brugerens id
            IDbDataParameter userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.DbType = DbType.Int32;
            userParam.Value = userId;
            command.Parameters.Add(userParam);

            // Kører SQL'en og returnerer antallet som et heltal
            return Convert.ToInt32(command.ExecuteScalar());
        }

        // Henter alle samtaler som en bestemt bruger deltager i
        public IEnumerable<ConversationDTO> GetConversationsByUserId(int userId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter samtaler ud fra brugerens id
            command.CommandText = @"
                    SELECT c.id, c.created_at
                    FROM Conversations c
                    JOIN ConversationParticipants cp ON cp.conversation_id = c.id
                    WHERE cp.user_id = @UserId";

            // Opretter parameter til brugerens id
            IDbDataParameter userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.DbType = DbType.Int32;
            userParam.Value = userId;
            command.Parameters.Add(userParam);

            // Opretter en liste til samtaler
            List<ConversationDTO> conversations = new();

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle samtaler
            while (reader.Read())
            {
                // Opretter en ConversationDTO og tilføjer den til listen
                conversations.Add(new ConversationDTO
                {
                    // Sætter samtalens id
                    Id = reader.GetInt32(reader.GetOrdinal("id")),

                    // Sætter hvornår samtalen blev oprettet
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at")))
                });
            }

            // Gennemgår alle samtaler for at hente deltagere og beskeder
            foreach (ConversationDTO conv in conversations)
            {
                // Henter deltager-id'er til samtalen
                conv.ParticipantIds = GetParticipantIds(conv.Id);

                // Henter beskeder til samtalen
                conv.Messages = GetMessages(conv.Id);
            }

            // Returnerer samtalerne
            return conversations;
        }

        // Henter en bestemt samtale ud fra dens id
        public ConversationDTO? GetConversationById(int conversationId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter en samtale ud fra id
            command.CommandText = "SELECT id, created_at FROM Conversations WHERE id = @Id";

            // Opretter parameter til samtalens id
            IDbDataParameter idParam = command.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.DbType = DbType.Int32;
            idParam.Value = conversationId;
            command.Parameters.Add(idParam);

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Returnerer null hvis samtalen ikke findes
            if (!reader.Read()) return null;

            // Opretter en ConversationDTO med data fra databasen
            ConversationDTO conv = new ConversationDTO
            {
                // Sætter samtalens id
                Id = reader.GetInt32(reader.GetOrdinal("id")),

                // Sætter hvornår samtalen blev oprettet
                CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at")))
            };

            // Lukker readeren, så der kan laves nye databasekald bagefter
            reader.Close();

            // Henter deltager-id'er til samtalen
            conv.ParticipantIds = GetParticipantIds(conv.Id);

            // Henter beskeder til samtalen
            conv.Messages = GetMessages(conv.Id);

            // Returnerer samtalen
            return conv;
        }

        // Henter en samtale mellem to bestemte brugere
        public ConversationDTO? GetConversationBetween(int userIdA, int userIdB)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der finder samtaler hvor begge brugere deltager
            command.CommandText = @"
                    SELECT c.id, c.created_at
                    FROM Conversations c
                    JOIN ConversationParticipants cpA ON cpA.conversation_id = c.id AND cpA.user_id = @UserA
                    JOIN ConversationParticipants cpB ON cpB.conversation_id = c.id AND cpB.user_id = @UserB";

            // Opretter parameter til første bruger
            IDbDataParameter paramA = command.CreateParameter();
            paramA.ParameterName = "@UserA";
            paramA.DbType = DbType.Int32;
            paramA.Value = userIdA;
            command.Parameters.Add(paramA);

            // Opretter parameter til anden bruger
            IDbDataParameter paramB = command.CreateParameter();
            paramB.ParameterName = "@UserB";
            paramB.DbType = DbType.Int32;
            paramB.Value = userIdB;
            command.Parameters.Add(paramB);

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Returnerer null hvis der ikke findes en samtale mellem brugerne
            if (!reader.Read()) return null;

            // Opretter en ConversationDTO med data fra databasen
            ConversationDTO conv = new ConversationDTO
            {
                // Sætter samtalens id
                Id = reader.GetInt32(reader.GetOrdinal("id")),

                // Sætter hvornår samtalen blev oprettet
                CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at")))
            };

            // Lukker readeren, så der kan laves nye databasekald bagefter
            reader.Close();

            // Henter deltager-id'er til samtalen
            conv.ParticipantIds = GetParticipantIds(conv.Id);

            // Henter beskeder til samtalen
            conv.Messages = GetMessages(conv.Id);

            // Returnerer samtalen
            return conv;
        }

        // Henter id'er på deltagere i en bestemt samtale
        List<int> GetParticipantIds(int conversationId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter deltager-id'er for samtalen
            command.CommandText = "SELECT user_id FROM ConversationParticipants WHERE conversation_id = @ConversationId";

            // Opretter parameter til samtalens id
            IDbDataParameter param = command.CreateParameter();
            param.ParameterName = "@ConversationId";
            param.DbType = DbType.Int32;
            param.Value = conversationId;
            command.Parameters.Add(param);

            // Opretter en liste til bruger-id'er
            List<int> ids = new();

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle deltagere
            while (reader.Read())
            {
                // Tilføjer brugerens id til listen
                ids.Add(reader.GetInt32(0));
            }

            // Returnerer listen med deltager-id'er
            return ids;
        }

        // Henter alle beskeder i en bestemt samtale
        List<MessageDTO> GetMessages(int conversationId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle beskeder i samtalen, sorteret efter oprettelsestid
            command.CommandText = @"
                    SELECT id, text, created_at, sender_id, conversation_id
                    FROM Messages
                    WHERE conversation_id = @ConversationId
                    ORDER BY created_at ASC";

            // Opretter parameter til samtalens id
            IDbDataParameter param = command.CreateParameter();
            param.ParameterName = "@ConversationId";
            param.DbType = DbType.Int32;
            param.Value = conversationId;
            command.Parameters.Add(param);

            // Opretter en liste til beskeder
            List<MessageDTO> messages = new();

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle beskeder
            while (reader.Read())
            {
                // Opretter en MessageDTO og tilføjer den til listen
                messages.Add(new MessageDTO
                {
                    // Sætter beskedens id
                    Id = reader.GetInt32(reader.GetOrdinal("id")),

                    // Sætter beskedens tekst
                    Text = reader.GetString(reader.GetOrdinal("text")),

                    // Sætter hvornår beskeden blev oprettet
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("created_at"))),

                    // Sætter id på brugeren der sendte beskeden
                    SenderId = reader.GetInt32(reader.GetOrdinal("sender_id")),

                    // Sætter id på samtalen beskeden hører til
                    ConversationId = reader.GetInt32(reader.GetOrdinal("conversation_id"))
                });
            }

            // Returnerer listen med beskeder
            return messages;
        }
    }
}