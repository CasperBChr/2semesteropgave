using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for MyAccountPage.xaml
    /// </summary>
    public partial class MyAccountPage : UserControl
    {
        // Holder den bruger, som siden viser og binder data til i UI
        public User CurrentUser { get; private set; }

        // Reference til service-laget, som håndterer dataadgang og opdatering af brugere
        private readonly UserServices? _userServices;

        public MyAccountPage()
        {
            InitializeComponent();
            // Opretter en tom bruger, så UI har et objekt at binde til
            CurrentUser = new User();
            // Binder hele UI til CurrentUser, så felterne automatisk viser brugerens data
            DataContext = CurrentUser;
        }

        public MyAccountPage(UserServices userServices)
        {
            InitializeComponent();
            // Gemmer servicen, så vi kan hente og opdatere brugeren
            _userServices = userServices;

            // Vælger specifikt brugeren med id = 1.
            CurrentUser = userServices?.Users?.FirstOrDefault(u => u.Id == 1) ?? new User();  // Ændrer 1 til 2 for at vise en anden bruger

            // Binder UI til den valgte bruger
            DataContext = CurrentUser;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Stopper, hvis der ikke er en service til at opdatere brugeren med
            if (_userServices == null)
            {
                MessageBox.Show("User service is not available.");
                return;
            }

            try
            {
                // Gemmer ændringerne for CurrentUser via service-laget
                _userServices.UpdateUser(CurrentUser);

                // Informerer brugeren om, at profilen er gemt
                MessageBox.Show("Din profil er gemt.");
            }
            catch (Exception ex)
            {
                // Viser fejlbeskeden, hvis noget går galt ved gemningen
                MessageBox.Show(ex.Message);
            }
        }
    }
}