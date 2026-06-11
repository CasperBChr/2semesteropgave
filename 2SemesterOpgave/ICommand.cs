using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave
{
    // Interface der beskriver en kommando, som kan udføres
    public interface ICommand
    {
        // Metode som alle klasser der bruger ICommand skal implementere
        void Execute();
    }
}