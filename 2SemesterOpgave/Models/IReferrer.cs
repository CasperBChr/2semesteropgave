using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public interface IReferrer
    {
        void ReceiveNotification(string message);

    }
}
