using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Pages;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave
{
	/// <summary>
	/// Interaction logic for LoginWindows.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		AuthServices _authServices;
		UserServices _userServices;
		LoginPage? _loginPage;
		public User? LoggedInUser { get; private set; }

		public LoginWindow(AuthServices authServices, UserServices userServices)
		{
			InitializeComponent();
			_authServices = authServices;
			_userServices = userServices;
			GridPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(170, 200, 200, 200));

			GridInputBorder.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 20, 20, 20));
			GridInputBorder.CornerRadius = new CornerRadius(10);
			//GridInputBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
			GridInputBorder.BorderThickness = new Thickness(0);

			//_loginPage = new LoginPage(authServices);
			LoginPageContentControl.Content = new LoginPage(_authServices, OnLoginSuccess);
			//_loginPage.LoginSucceeded += OnLoginSuccess;

			//LoginPageContentControl.Content = _loginPage;


		}

		private void ShowCreateAccountButton_Click(object sender, RoutedEventArgs e)
		{
			//if (_loginPage != null)
			//{
			//	_loginPage.LoginSucceeded -= OnLoginSuccess;
			//	_loginPage = null;
			//}
			LoginPageContentControl.Content = new CreateAccountPage(_userServices);
			ShowCreateAccountButton.Visibility = Visibility.Collapsed;
			ShowLoginAccountButton.Visibility = Visibility.Visible;
		}

		private void ShowLoginAccountButton_Click(object sender, RoutedEventArgs e)
		{
			//if (_loginPage != null)
			//{
			//	_loginPage.LoginSucceeded -= OnLoginSuccess;
			//}
			//_loginPage = new LoginPage(_authServices);
			//_loginPage.LoginSucceeded += OnLoginSuccess;

			//LoginPageContentControl.Content = new LoginPage(_authServices, OnLoginSuccess);
			//LoginPageContentControl.Content = _loginPage;

			_loginPage = new LoginPage(_authServices, OnLoginSuccess);
			LoginPageContentControl.Content = _loginPage;

			ShowCreateAccountButton.Visibility = Visibility.Visible;
			ShowLoginAccountButton.Visibility = Visibility.Collapsed;
		}


		public void OnLoginSuccess(User user)
		{
			LoggedInUser = user; // gem brugeren
			DialogResult = true; // luk vinduet
		}
		//private void OnLoginSuccess(User user)
		//{
		//	MessageBox.Show($"OnLoginSuccess fired for: {user.Username}"); // ADD THIS
		//	DialogResult = true;
		//}
	}
}
