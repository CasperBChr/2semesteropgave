using System;
using System.Collections.Generic;
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
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ArticleServices, UserServices, CategoryServices og Router

namespace _2SemesterOpgave.Pages
{
	/// <summary>
	/// Kodet af Casper
	/// </summary>
	// Klassen er en WPF-side, som arver fra UserControl
	public partial class HomePage : UserControl
    {
        // Service der håndterer artikler
        ArticleServices _articleServices;

        // Service der håndterer brugerdata
        UserServices _userServices;

        // Service der håndterer kategorier
        CategoryServices _categoryServices;

        // Router bruges til at navigere mellem sider
        Router _router;


        // Constructor der modtager de services siden skal bruge
        public HomePage(Router router, ArticleServices articleServices, UserServices userServices, CategoryServices categoryServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer article service, så siden kan hente og vælge artikler
            _articleServices = articleServices;

            // Gemmer user service, så siden kan opdatere profilvisningen
            _userServices = userServices;

            // Gemmer category service, så siden kan hente kategorier
            _categoryServices = categoryServices;

            // Sætter ItemsSource til de nyeste artikler, så de vises på forsiden
            ArticlesItemsControl.ItemsSource = _articleServices.GetNewestArticles();

            // Sætter ItemsSource til 10 tilfældige artikler, så de vises på forsiden
            RandomArticlesItemsControl.ItemsSource = _articleServices.GetRandomArticles(10);

            // Sætter ItemsSource til 10 tilfældige kategorier, så de vises på forsiden
            RandomCategoriesItemsControl.ItemsSource = _categoryServices.GetRandomCategories(10);
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