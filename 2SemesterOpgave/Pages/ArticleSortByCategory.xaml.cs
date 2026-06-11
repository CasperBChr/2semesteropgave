using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article og Category
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ArticleServices, CategoryServices og Router
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
    /// Kodet af Camilla. Interaction logic for ArticleSortByCategory.xaml
    /// </summary>
    public partial class ArticleSortByCategory : UserControl
    {
        // Service der håndterer artikler
        private ArticleServices _articleServices;

        // Service der håndterer kategorier
        private CategoryServices _categoryServices;

        // Router bruges til at navigere mellem sider
        private Router _router;

        // Indeholder den kategori, som siden viser artikler for
        private Category _selectedCategory;

        // Service der håndterer brugerdata og brugerprofilvisning
        private UserServices _userServices;

        // Constructor der modtager den valgte kategori og de services siden skal bruge
        public ArticleSortByCategory(Category category, ArticleServices articleServices, CategoryServices categoryServices, Router router, UserServices userServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer den valgte kategori
            _selectedCategory = category;

            // Gemmer article service, så siden kan hente og vælge artikler
            _articleServices = articleServices;

            // Gemmer category service, hvis siden skal bruge kategori-data
            _categoryServices = categoryServices;

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer user service, så siden kan opdatere brugerprofil-visning
            _userServices = userServices;

            // Henter artikler fra den valgte kategori og lægger dem i en liste
            List<Article> articles = new List<Article>(_articleServices.GetArticlesByCategory(_selectedCategory.Id));

            // Henter og viser artikler for den valgte kategori
            LoadArticles();

            // Opdaterer UI med kategoriens navn
            UpdateUI();
        }

        //Metode til at hente og vise artikler baseret på den valgte kategori
        private void LoadArticles()
        {
            // Henter artikler fra ArticleServices ud fra den valgte kategoris Id
            IEnumerable<Article> articles = _articleServices.GetArticlesByCategory(_selectedCategory.Id);

            // Laver resultatet om til en List<Article>
            List<Article> articleList = new List<Article>(articles);

            // Sætter ItemsSource til listen, så artiklerne vises i UI
            ArticlesItemsControl.ItemsSource = articleList;
        }

        //Metode til at opdatere UI-elementer baseret på den valgte kategori
        private void UpdateUI()
        {
            // Viser navnet på den valgte kategori i tekstfeltet
            CategoryNameTextBlock.Text = _selectedCategory.Name;
        }

        //Metode til at navigere til den valgte artikel, når den klikkes på
        private void ArticleCategoryPageButton_Click(object sender, RoutedEventArgs e)
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