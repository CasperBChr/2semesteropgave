using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave
{
	// Kommando der bruges til at navigere til en bestemt side
	/// <summary>
	/// Kodet af Martin
	/// </summary>
	public class NavigateCommand : ICommand
    {
        // Routeren der står for selve navigationen
        readonly Router _router;

        // Den route/side der skal navigeres til
        readonly Routes _route;

        // Constructor der modtager routeren og den route der skal navigeres til
        public NavigateCommand(Router router, Routes route)
        {
            // Gemmer routeren, så den kan bruges i Execute
            _router = router;

            // Gemmer routen, så kommandoen ved hvilken side den skal åbne
            _route = route;
        }

        // Udfører kommandoen
        public void Execute()
        {
            // Bruger routeren til at navigere til den gemte route
            _router.NavigateTo(_route);
        }
    }
}