using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article og User
using _2SemesterOpgave.Repositories; // Giver adgang til repositories, hvis siden skal bruge databaseklasser direkte
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ArticleServices, UserServices og Router
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

namespace _2SemesterOpgave.Pages
{

    // Klassen er en WPF-side, som arver fra UserControl
    /// <summary>
    /// Kodet af Camilla. Interaction logic for MyArticlesPage.xaml
    /// </summary>
    public partial class MyArticlesPage : UserControl
    {
        // Service der håndterer artikler
        private ArticleServices _articleServices;

        // Router bruges til at navigere mellem sider
        private Router _router;

        // Service der håndterer brugerdata
        private UserServices _userServices;

        // Service der håndterer login/authentication
        private AuthServices _authServices;

        // Indeholder den bruger, hvis artikler vises på siden
        private User _selectedUser;

        //Constructor
        public MyArticlesPage(ArticleServices articleServices, CategoryServices categoryServices, Router router, UserServices userServices, AuthServices authServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer article service, så siden kan hente og vælge artikler
            _articleServices = articleServices;

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer user service, så siden kan hente den aktuelle bruger
            _userServices = userServices;

            // Gemmer auth service, hvis siden skal bruge login/auth-data
            _authServices = authServices;

            // Sætter den valgte bruger til den bruger, som er logget ind
            _selectedUser = _userServices.CurrentUser;

            //Liste med artikler tilhørende den valgte bruger
            List<Article> articles = new List<Article>(_articleServices.GetArticlesByOwner(_selectedUser.Id));

            // Henter og viser brugerens artikler
            LoadArticles();

            // Opdaterer UI med brugerens oplysninger
            UpdateUI();
        }

        //Metode til at opdatere UI baseret på om brugeren har artikler eller ej
        private void LoadArticles()
        {
            //Liste med artikler tilhørende den valgte bruger
            IEnumerable<Article> articles = _articleServices.GetArticlesByOwner(_selectedUser.Id);

            //Liste der bruges til at binde artiklerne til UI
            List<Article> articleList = new List<Article>(articles);

            // Sætter ItemsSource til brugerens artikler, så de vises i UI
            ArticlesItemsControl.ItemsSource = articleList;
        }

        //Metode til at opdatere UI-elementer baseret på den valgte kategori
        private void UpdateUI()
        {
            // Viser brugerens fornavn i tekstfeltet
            OwnerTextBlock.Text = _selectedUser.FirstName;
        }

        //Knap til at komme ind på EditPage for at redigere en artikel
        private void EditPageButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter artiklen fra knappens DataContext og gemmer den som valgt artikel
            _articleServices.SelectedArticle = (Article)button.DataContext;

            // Navigerer til rediger artikel-siden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.EditArticlePage));

            //_router.NavigateTo(Routes.EditArticlePage);
        }
    }
}