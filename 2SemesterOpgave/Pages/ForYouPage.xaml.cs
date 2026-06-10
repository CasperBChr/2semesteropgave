using _2SemesterOpgave.Algoritme; // Giver adgang til algoritmen, fx ContentBasedAlgorithm og Recommendation
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User, Article og ItemProfile
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx UserServices, ArticleServices og Router
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Camilla. Interaction logic for ForYouPage.xaml
    /// </summary>

    // Klassen er en WPF-side, som arver fra UserControl
    public partial class ForYouPage : UserControl
    {
        // Router bruges til at navigere mellem sider
        Router _router;

        // Service der håndterer brugerdata og brugerens profil
        public UserServices UserServices;

        // Service der håndterer artikler
        public ArticleServices ArticleServices;

        // Service der håndterer kategorier
        public CategoryServices CategoryServices;

        //Constructor
        public ForYouPage(Router router, UserServices userService, ArticleServices articleService, CategoryServices categoryService)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer user service, så siden kan bruge brugerens profil i algoritmen
            UserServices = userService;

            // Gemmer article service, så siden kan hente og vælge artikler
            ArticleServices = articleService;

            // Gemmer category service, hvis siden skal bruge kategori-data
            CategoryServices = categoryService;

            //Liste med alle artikler til brug til udregning for algoritmen
            List<ItemProfile> itemProfiles = new List<ItemProfile>();

            // Henter alle artikler fra ArticleServices og lægger dem i en liste
            List<Article> articles = new List<Article>(ArticleServices.GetAllArticles());

            // Looper igennem alle artikler
            for (int i = 0; i < articles.Count; i++)
            {
                // Tjekker om artiklen har en ItemProfile
                if (articles[i].ItemProfile != null)
                {
                    // Tilføjer artiklens ItemProfile til listen, så algoritmen kan bruge den
                    itemProfiles.Add(articles[i].ItemProfile);
                }
            }

            //Liste med fem anbefalede artikler baseret på algoritmen, hvor den henter artikler via den ovenstående liste
            List<Recommendation> recommendations = ContentBasedAlgorithm.GetRecommendations(UserServices.UserProfile, itemProfiles, 5);

            //Liste med de fem anbefalede artikler der vises på ForYouPage
            List<Article> recommendedArticles = recommendations.Select(r => r.Item.Article).Where(a => a != null).ToList();

            // Sætter ItemsSource til de anbefalede artikler, så de vises i UI
            ForYouArticlesItemsControl.ItemsSource = recommendedArticles;
        }

        //Knap til at komme ind på ArticlePage for at se en artikel
        private void ForYouArticlePageButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter artiklen fra knappens DataContext og gemmer den som valgt artikel
            ArticleServices.SelectedArticle = (Article)button.DataContext;

            // Opdaterer brugerprofilen med profilen fra den valgte artikel
            UserServices.UserProfile.UpdateUserProfileView(ArticleServices.SelectedArticle.ItemProfile);

            // Navigerer til artikelsiden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));

            //Router.NavigateTo(Routes.Article);
        }
    }
}