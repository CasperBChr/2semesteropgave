using System; 
using System.Windows; 
using System.Windows.Controls;      // Giver adgang til WPF controls som UserControl
using _2SemesterOpgave.Models;      // Giver adgang til vores modelklasser, fx User
using _2SemesterOpgave.Services;    // Giver adgang til vores serviceklasser, fx UserServices og Router

namespace _2SemesterOpgave.Pages
{
    public partial class MyAccountPage : UserControl // Klassen er en WPF-side, som arver fra UserControl
    {
        public User CurrentUser { get; private set; } // Indeholder den bruger, som vises og redigeres på siden

        private readonly UserServices? _userServices; // Service der håndterer brugerdata
        private readonly Router? _router; // Router bruges til at navigere mellem sider

        public MyAccountPage() // Tom constructor, som kan bruges hvis siden oprettes uden services
        {
            InitializeComponent(); // Initialiserer XAML-designet og gør sidens UI-elementer klar

            CurrentUser = new User(); // Opretter en tom bruger, så siden stadig har data at binde til
            DataContext = CurrentUser; // Sætter datakonteksten til CurrentUser, så XAML kan vise og redigere brugerens data
        }

        public MyAccountPage(Router router, UserServices userServices) // Constructor der modtager router og user service
        {
            InitializeComponent(); // Initialiserer XAML-designet og gør sidens UI-elementer klar

            _router = router; // Gemmer routeren, så siden kan navigere til andre sider
            _userServices = userServices; // Gemmer user service, så siden kan hente og opdatere brugeren

            CurrentUser = _userServices.GetUserById(_userServices.CurrentUser.Id) ?? new User();
            // Henter den aktuelle bruger ud fra brugerens Id
            // Hvis brugeren ikke findes, oprettes der en tom User i stedet

            DataContext = CurrentUser; // Sætter datakonteksten til CurrentUser, så XAML kan binde til brugerens properties
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) // Metode der kører, når brugeren klikker på gem-knappen
        {
            if (_userServices == null) // Tjekker om user service mangler
            {
                MessageBox.Show("User service is not available."); // Viser en fejlbesked hvis servicen ikke findes
                return; // Stopper metoden, fordi vi ikke kan gemme uden user service
            }

            try // Forsøger at gemme brugeren
            {
                _userServices.UpdateUser(CurrentUser); // Opdaterer den aktuelle bruger med de nye oplysninger
                MessageBox.Show("Din profil er gemt."); // Viser besked om at profilen er gemt
            }
            catch (Exception ex) // Fanger fejl hvis noget går galt under gemning
            {
                MessageBox.Show(ex.Message); // Viser fejlbeskeden til brugeren
            }
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e) // Metode der kører, når brugeren klikker på "Anmeldelser"-knappen
        {
            _router?.NavigateTo(Routes.Reviews); // Navigerer til review-siden, hvis routeren findes
        }
    }
}