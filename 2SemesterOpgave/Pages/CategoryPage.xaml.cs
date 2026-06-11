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
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Category
using System.Collections.ObjectModel;
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx CategoryServices, ArticleServices og Router

namespace _2SemesterOpgave.Pages
{
    // Klassen er en WPF-side, som arver fra UserControl
    /// <summary>
    /// Kodet af Camilla. Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : UserControl
    {
        // Service der håndterer kategorier
        private CategoryServices _services;

        // Router bruges til at navigere mellem sider
        private Router _router;

        // Service der håndterer artikler
        private ArticleServices _articlesServices;

        // Constructor der modtager de services siden skal bruge
        public CategoryPage(CategoryServices categoryService, Router router, ArticleServices articleServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer category service, så siden kan hente kategorier
            _services = categoryService;

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer article service, hvis siden skal bruge artikler
            _articlesServices = articleServices;

            // Sætter ItemsSource til alle kategorier, så de vises i UI
            CategoriesItemsControl.ItemsSource = _services.GetAllCategories();
        }

        //Metode der navigerer til kategoriens artikler ved klik. 
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter kategorien fra knappens DataContext
            Category selectedCategory = (Category)button.DataContext;

            // Gemmer den valgte kategori i routeren, så næste side ved hvilken kategori der skal vises
            _router.SetSelectedCategory(selectedCategory);

            // Navigerer til siden med artikler fra den valgte kategori og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.ArticleSortByCategory));

            //_router.NavigateTo(Routes.ArticleSortByCategory);                
        }
    }
}