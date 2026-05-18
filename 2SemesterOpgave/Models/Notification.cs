using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Notification
    {
        public string Message { get; set; } = string.Empty;
        public IReferrer Referrer { get; set; }
        public Notification(string message, IReferrer referrer)
        {
            Message = message;
            Referrer = referrer;
        }

        public void ReceiveNotification(string message)
        {
            Console.WriteLine($"Notification: {message}");
        }
    }
}
