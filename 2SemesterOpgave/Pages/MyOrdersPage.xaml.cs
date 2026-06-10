using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl og Button
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User, Rental og Article
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx UserServices, RentalServices og Router

namespace _2SemesterOpgave.Pages
{

    /// <summary>
    /// Interaction logic for MyOrdersPage.xaml === Kodet af Daniel
    /// </summary>


    // Klassen er en WPF-side, som arver fra UserControl
    public partial class MyOrdersPage : UserControl
    {
        //public User CurrentUser { get; private set; }

        // Liste over lejeaftaler hvor den aktuelle bruger lejer noget
        public ObservableCollection<Rental> Rentals { get; private set; }

        // Liste over lejeaftaler hvor den aktuelle bruger udlejer noget
        public ObservableCollection<Rental> RentedOut { get; private set; }

        // Router bruges til navigation mellem sider
        private readonly Router _router;

        // Service der håndterer lejeaftaler
        RentalServices _rentalServices;

        // Service der håndterer anmeldelser, men bruges ikke direkte i denne version
        private readonly ReviewServices? _reviewServices;

        // Service der håndterer brugerdata
        UserServices _userServices;

        // Indeholder den bruger, som er logget ind lige nu
        public User CurrentUser { get; private set; }

        // Service der håndterer artikler
        ArticleServices _articleServices;


        public MyOrdersPage(Router router, UserServices userServices, ReviewServices reviewServices, RentalServices rentalServices, ArticleServices articleServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer review service, hvis siden skal bruge den til anmeldelser
            _reviewServices = reviewServices;

            // Gemmer rental service, så siden kan hente lejeaftaler
            _rentalServices = rentalServices;

            // Gemmer user service, så siden kan hente den aktuelle bruger
            _userServices = userServices;

            // Gemmer article service, så siden kan vælge en artikel
            _articleServices = articleServices;

            // Sætter datakonteksten til siden selv, så XAML kan binde til properties herfra
            DataContext = this;

            // Henter den bruger, som er logget ind
            CurrentUser = _userServices.CurrentUser;

            // Henter alle lejeaftaler hvor CurrentUser er lejer
            Rentals = _rentalServices.GetByRenter(CurrentUser);

            // Henter alle lejeaftaler hvor CurrentUser er udlejer
            RentedOut = _rentalServices.GetByRentee(CurrentUser);
        }

        // Metode der kører, når brugeren klikker på review-knappen
        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter den Rental, som knappen hører til
            Rental rental = (Rental)button.DataContext;

            // Tjekker om der ikke findes en bruger, som skal vurderes
            if (rental.Rentee == null)
            {
                // Viser en fejlbesked til brugeren
                MessageBox.Show("Kan ikke finde brugeren, der skal vurderes.");

                // Stopper metoden, fordi der ikke er en bruger at vurdere
                return;
            }

            // Sætter den bruger, der skal vurderes, som TargetUser
            _userServices.TargetUser = rental.Rentee;

            // Navigerer til review-siden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Reviews));

            //_router?.NavigateTo(Routes.Reviews);
        }

        // Metode der kører, når brugeren klikker på artikel-knappen
        private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter den Rental, som knappen hører til
            Rental rental = (Rental)button.DataContext;

            // Tjekker om lejeaftalen mangler en artikel
            if (rental.Article == null)
            {
                // Viser en fejlbesked hvis artiklen ikke findes
                MessageBox.Show("Kan ikke finde artiklen.");

                // Stopper metoden, fordi der ikke er en artikel at vise
                return;
            }

            // Gemmer den valgte artikel, så ArticlePage kan vise den
            _articleServices.SelectedArticle = rental.Article;

            // Navigerer til artikelsiden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));

            //_router?.NavigateTo(Routes.Article);
        }
    }
}