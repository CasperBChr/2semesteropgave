using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Conversation : IReferrer
    {
        public DateTime CreationTime { get; set; }
        public DateTime LastActive { get; set; }
        public List<User> Participants { get; set; }
        public List<Message> Messages { get; set; }
        public void ReceiveNotification(string message)
        {
            return;
        }
        public Conversation()
        {
            Messages = new List<Message>();
            Participants = new List<User>();
            CreationTime = DateTime.Now;
            LastActive = DateTime.Now;
        }

        public Conversation(List<User> participants)
        {
            Messages = new List<Message>();
            Participants = participants;
            CreationTime = DateTime.Now;
            LastActive = DateTime.Now;
        }
        public Conversation(List<User> participants, List<Message> messages, DateTime creationTime, DateTime lastActive)
        {
            Participants = participants;
            Messages = messages;
            CreationTime = creationTime;
            LastActive = lastActive;

        }
        
    }
}
