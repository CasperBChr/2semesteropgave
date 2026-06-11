using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2SemesterOpgave.Services;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Pages
{
	/// <summary>
	/// Kodet af Martin
	/// </summary>
	public partial class LoginPage : UserControl
	{
		AuthServices _authServices;
		//public event Action<User>? LoginSucceeded;
		Action<User> _onSuccess;

		public LoginPage(AuthServices authServices, Action<User> onSuccess)
		{
			InitializeComponent();
			_authServices = authServices;
			_onSuccess = onSuccess;
		}

        //Metode der håndterer login ved at tjekke brugernavn og password og kalder onSuccess action hvis login lykkedes
        private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			bool success = _authServices.Login(UsernameTextBox.Text, PasswordBox.Password);

			if (success)
			{
				//LoginSucceeded?.Invoke(_authServices.CurrentUser!);
				_onSuccess(_authServices.CurrentUser!);
			}
			else
			{
				MessageBox.Show("Forkert brugernavn eller adgangskode.");
			}
		}
	}
}
