using System; 
using System.Collections.ObjectModel; 
using System.Diagnostics; 
using System.Windows; 
using System.Windows.Controls;   // Giver adgang til WPF controls som UserControl og Button
using _2SemesterOpgave.Models;   // Giver adgang til vores modelklasser, fx User, Rental og Article
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx RentalServices og UserServices

namespace _2SemesterOpgave.Pages
    public partial class MyOrdersPage : UserControl // Klassen er en WPF-side, som arver fra UserControl
    {
        // public User CurrentUser { get; private set; }

        public ObservableCollection<Rental> Rentals { get; private set; } // Liste over lejeaftaler hvor den aktuelle bruger lejer noget
        public ObservableCollection<Rental> RentedOut { get; private set; } // Liste over lejeaftaler hvor den aktuelle bruger udlejer noget

        private readonly Router? _router;   // Router bruges til at navigere mellem sider
        RentalServices _rentalServices;     // Service der håndterer data og funktioner omkring lejeaftaler
        private readonly ReviewServices? _reviewServices; // Service der håndterer anmeldelser/vurderinger
        UserServices _userServices;         // Service der håndterer brugerdata
        public User CurrentUser { get; private set; } // Indeholder den bruger, som er logget ind lige nu

        ArticleServices _articleServices; // Service der håndterer artikler/produkter


        public MyOrdersPage(
            Router router,
            UserServices userServices,
            ReviewServices reviewServices,
            RentalServices rentalServices,
            ArticleServices articleServices)
        {
            InitializeComponent(); // Initialiserer XAML-designet og gør sidens UI-elementer klar så vi kan vise dem og interagere med dem

            _router = router; // Gemmer routeren, så siden kan navigere til andre sider
            _reviewServices = reviewServices;   // Gemmer review service, så vi kan sætte hvem der skal vurderes
            _rentalServices = rentalServices;   // Gemmer rental service, så vi kan hente lejer
            _userServices = userServices;       // Gemmer user service, så vi kan finde den aktuelle bruger
            _articleServices = articleServices; // Gemmer article service, så vi kan vælge en artikel

            DataContext = this; // Sætter sidens datakontekst til sig selv, så XAML kan binde til properties herfra

            CurrentUser = _userServices.CurrentUser; // Henter den bruger, der er logget ind

            Rentals = _rentalServices.GetByRenter(CurrentUser);   // Henter alle lejeaftaler hvor CurrentUser er lejer
            RentedOut = _rentalServices.GetByRentee(CurrentUser); // Henter alle lejeaftaler hvor CurrentUser er udlejer
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e) // Metode der kører, når brugeren klikker på review-knappen
        {
            Button button = (Button)sender; // Finder den knap, der blev klikket på
            Rental rental = (Rental)button.DataContext; // Henter den Rental, som knappen hører til

            if (rental.Rentee == null) // Tjekker om der mangler en bruger at vurdere
            {
                MessageBox.Show("Kan ikke finde brugeren, der skal vurderes."); // Viser en fejlbesked til brugeren
                return; // Stopper metoden, så vi ikke går videre uden en bruger
            }

            _reviewServices?.SetReviewTarget(rental.Rentee); // Sætter den bruger, som skal vurderes
            _router?.NavigateTo(Routes.Reviews); // Navigerer til review-siden
        }

        private void ArticlePageButton_Click(object sender, RoutedEventArgs e) // Metode der kører, når brugeren klikker på artikel-knappen
        {
            Button button = (Button)sender; // Finder den knap, der blev klikket på
            Article article = (Article)button.DataContext; // Henter den Article, som knappen hører til

            _articleServices.SelectedArticle = article; // Gemmer den valgte artikel i ArticleServices
            _router.NavigateTo(Routes.Article); // Navigerer til artikelsiden
        }
    }
}