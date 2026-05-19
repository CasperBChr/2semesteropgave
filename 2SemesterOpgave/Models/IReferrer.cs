using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public interface IReferrer // Interface til at repræsentere en referrer, som kan modtage notifikationer. Dette interface indeholder en metode ReceiveNotification, som tager en string parameter message, der repræsenterer beskeden, der skal modtages.
    {
        void ReceiveNotification(string message); // Metode til at modtage notifikationer, som skal implementeres af klasser, der implementerer IReferrer. Denne metode tager en string parameter message, som repræsenterer beskeden, der skal modtages.

    }
}
