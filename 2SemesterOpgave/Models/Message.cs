using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Message
    {
        public string Text { get; set; }
        public Conversation Conversation { get; set; }
    
        public Message(string text, Conversation conversation)
        {
            Text = text;
            Conversation = conversation;
        }
    }
    
}
