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

        //Knap der opretter en konto ved at kalde CreateAccount-funktionen i UserServices, og navigerer derefter til LoginPage
        private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
            //Henter input fra tekstfelterne
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

            //Tjeker om brugernavnet eller emailen allerede er i brug
            User? existingUser =
				_userServices.GetAllUsers()
					.FirstOrDefault(u =>
						u.Username.Equals(
							username,
							StringComparison.OrdinalIgnoreCase));

            //Hvis der allerede findes en bruger med det brugernavn eller email, vises en fejlbesked
            if (existingUser != null)
			{
				MessageBox.Show("Username already exists.");
				return;
			}

            //Opretter en ny bruger ved at kalde CreateAccount-funktionen i UserServices
            User user = new User
			{
				Username = username,
				Email = email,
				Password = password
			};

            //Prøver at oprette brugeren, og hvis det lykkes, vises en succesbesked og tekstfelterne ryddes
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

            //Fejlbesked hvis der opstår en fejl under oprettelsen af brugeren
            catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
