using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
    // DTO-klasse der bruges til at transportere besked-data fra databasen
    public class MessageDTO
    {
        // Beskedens id i databasen
        public int Id { get; set; }

        // Selve beskedens tekst
        public string Text { get; set; } = string.Empty;

        // Dato og tidspunkt for hvornår beskeden blev sendt/oprettet
        public DateTime CreatedAt { get; set; }

        // Id på brugeren der har sendt beskeden
        public int SenderId { get; set; }

        // Id på samtalen som beskeden hører til
        public int ConversationId { get; set; }
    }
}
