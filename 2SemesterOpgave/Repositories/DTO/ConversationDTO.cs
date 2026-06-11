using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	// DTO-klasse der bruges til at transportere samtale-data fra databasen
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class ConversationDTO
    {
        // Samtalens id i databasen
        public int Id { get; set; }

        // Dato og tidspunkt for hvornår samtalen blev oprettet
        public DateTime CreatedAt { get; set; }

        // Liste med id'er på de brugere der deltager i samtalen
        public List<int> ParticipantIds { get; set; } = new List<int>();

        // Liste med beskeder der hører til samtalen
        public List<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    }
}
