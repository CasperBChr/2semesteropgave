using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class MessageDTO
	{
        //DTO property for Message
        public int Id { get; set; }
		public string Text { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public int SenderId { get; set; }
		public int ConversationId { get; set; }
	}
}
