using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl og Button
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ArticleServices, UserServices og Router

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Casper
    /// </summary>

    // Klassen er en WPF-side, som arver fra UserControl
    public partial class ExplorePage : UserControl
    {
        // Service der håndterer artikler
        ArticleServices _articleServices;

        // Router bruges til at navigere mellem sider
        Router _router;

        // Service der håndterer brugerdata og brugerprofilvisning
        UserServices _userServices;

        // Constructor der modtager de services siden skal bruge
        public ExplorePage(Router router, ArticleServices articleServices, UserServices userServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer article service, så siden kan hente og vælge artikler
            _articleServices = articleServices;

            // Gemmer user service, så siden kan opdatere brugerprofil-visning
            _userServices = userServices;

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Sætter ItemsSource til alle artikler, så de vises på Explore-siden
            ArticlesItemsControl.ItemsSource = _articleServices.GetAllArticles();
        }

        // Event handler der kører, når brugeren klikker på en artikelknap
        private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter artiklen fra knappens DataContext og gemmer den som valgt artikel
            _articleServices.SelectedArticle = (Article)button.DataContext;

            // Opdaterer brugerprofilvisningen med profilen fra den valgte artikel
            _userServices.UserProfile.UpdateUserProfileView(_articleServices.SelectedArticle.ItemProfile);

            // Navigerer til artikelsiden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));

            //_router.NavigateTo(Routes.Article);
        }
    }
}