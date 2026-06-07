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
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
	/// <summary>
	/// Interaction logic for CreateAccountPage.xaml
	/// </summary>
	public partial class CreateAccountPage : UserControl
	{
		UserServices _userServices;
		public CreateAccountPage(UserServices userServices)
		{
			InitializeComponent();

			_userServices = userServices;
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameTextBox.Text.Trim();
			string email = EmailTextBox.Text.Trim();
			string password = PasswordBox.Password;
			string passwordAgain = PasswordAgainBox.Password;

			if (string.IsNullOrWhiteSpace(username))
			{
				MessageBox.Show("Username is required.");
				return;
			}

			if (string.IsNullOrWhiteSpace(email))
			{
				MessageBox.Show("Email is required.");
				return;
			}

			if (string.IsNullOrWhiteSpace(password))
			{
				MessageBox.Show("Password is required.");
				return;
			}

			if (password != passwordAgain)
			{
				MessageBox.Show("Passwords do not match.");
				return;
			}

			User? existingUser =
				_userServices.GetAllUsers()
					.FirstOrDefault(u =>
						u.Username.Equals(
							username,
							StringComparison.OrdinalIgnoreCase));

			if (existingUser != null)
			{
				MessageBox.Show("Username already exists.");
				return;
			}

			User user = new User
			{
				Username = username,
				Email = email,
				Password = password
			};

			try
			{
				_userServices.CreateUser(user);

				MessageBox.Show(
					"Account created successfully.",
					"Success",
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				UsernameTextBox.Clear();
				EmailTextBox.Clear();
				PasswordBox.Clear();
				PasswordAgainBox.Clear();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
