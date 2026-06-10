using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave
{
	public class NavigateCommand : ICommand
	{
		readonly Router _router;
		readonly Routes _route;

		public NavigateCommand(Router router, Routes route)
		{
			_router = router;
			_route = route;
		}

		public void Execute()
		{
			_router.NavigateTo(_route);
		}
	}
}
