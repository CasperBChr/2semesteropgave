using System;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl
using System.Windows.Media;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx UserServices, ReviewServices og Router

namespace _2SemesterOpgave.Pages
{

	/// <summary>
	/// Kodet af Daniel
	/// </summary>
	// Klassen er en WPF-side, som arver fra UserControl
	public partial class MyAccountPage : UserControl
    {
        // Indeholder den bruger, som vises og redigeres på siden
        public User CurrentUser { get; private set; }

        // Indeholder brugerens gennemsnitlige rating
        public float UserRating { get; private set; }

        // Service der håndterer brugerdata
        UserServices? _userServices;

        // Router bruges til at navigere mellem sider
        Router _router;

        // Constructor der modtager de services siden skal bruge
        public MyAccountPage(Router router, UserServices userServices, ReviewServices reviewServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer user service, så siden kan hente og opdatere brugeren
            _userServices = userServices;

            // Henter den aktuelle bruger ud fra brugerens Id
            CurrentUser = _userServices.GetUserById(_userServices.CurrentUser.Id);

            //CurrentUser = _userServices.CurrentUser;

            // Henter brugerens gennemsnitlige rating ud fra brugerens Id
            UserRating = reviewServices.GetAverageRating(CurrentUser.Id);

            // Sætter datakonteksten til siden selv, så XAML kan binde til både CurrentUser og UserRating
            DataContext = this;

            //DataContext = CurrentUser;
        }

        // Metode der kører, når brugeren klikker på gem-knappen
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker om der ikke findes en aktuel bruger
            if (_userServices.CurrentUser == null)
            {
                // Viser en fejlbesked hvis brugeren ikke findes
                MessageBox.Show("User is not available.");

                // Stopper metoden, fordi vi ikke kan gemme uden en bruger
                return;
            }

            // Forsøger at gemme brugeren
            try
            {
                // Opdaterer den aktuelle bruger med de nye oplysninger
                _userServices.UpdateUser(CurrentUser);

                // Viser besked om at profilen er gemt
                MessageBox.Show("Din profil er gemt.");
            }
            // Fanger fejl hvis noget går galt under gemning
            catch (Exception ex)
            {
                // Viser fejlbeskeden til brugeren
                MessageBox.Show(ex.Message);
            }
        }

        // Metode der kører, når brugeren klikker på "Anmeldelser"-knappen
        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til review-siden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Reviews));

            //_router?.NavigateTo(Routes.Reviews);
        }
    }
}