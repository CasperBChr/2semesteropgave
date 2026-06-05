using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class ConversationDTO
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<int> ParticipantIds { get; set; } = new List<int>();
		public List<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
	}
}
