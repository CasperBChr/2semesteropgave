using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Services;
using _2SemesterOpgave.Utils;

namespace _2SemesterOpgave
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		Database _db;
		UserRepository _userRepository;
		UserServices _userServices;
		SessionContext _session;
		AuthServices _authService;

		bool _testMode = true;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ShutdownMode = ShutdownMode.OnExplicitShutdown;
			try
			{
				string dbpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "db.db");
				_db = new Database($"Data Source={dbpath}");
				_userRepository = new UserRepository(_db);
				_session = new SessionContext();
				_userServices = new UserServices(_userRepository, _session);
				_authService = new AuthServices(_userRepository, _userServices, _session);

				if( _testMode )
				{
					LoginWindow loginWindow = new LoginWindow(_authService, _userServices);
					bool? loggedIn = loginWindow.ShowDialog();

					//MessageBox.Show($"ShowDialog returned: {loggedIn}");

					if (loggedIn != true)
					{
						Shutdown();
						return;
					}

					MainWindow mainWindow = new MainWindow(_db, _userServices);
					mainWindow.Show();

					ShutdownMode = ShutdownMode.OnLastWindowClose;
				}
				else 
				{
					ShutdownMode = ShutdownMode.OnLastWindowClose;
					List<User> users = new List<User>(_userServices.GetAllUsers());
					_session.CurrentUser = users[0];
					_userServices.CurrentUser = users[0];
					//_authService.CurrentUser = users[0];
					MainWindow mainWindow = new MainWindow(_db, _userServices);
					mainWindow.Show();
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show($"Fejl ved opstart:\n\n{ex.Message}", "Fejl");
				//Debug.WriteLine($"MainWindow fejl:\n\n{ex.Message}\n\n{ex.StackTrace}", "Fejl");
				//MessageBox.Show($"MainWindow fejl:\n\n{ex.Message}\n\n{ex.StackTrace}", "Fejl");
				Shutdown();
			}
		}
    }
}
