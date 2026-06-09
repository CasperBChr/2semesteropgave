using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
    /// Kodet af Camilla. Interaction logic for ArticleSortByCategory.xaml
    /// </summary>
    public partial class ArticleSortByCategory : UserControl
    {
        private ArticleServices _articleServices;
        private CategoryServices _categoryServices;
        private Router _router;
        private Category _selectedCategory;
        private UserServices _userServices;

        public ArticleSortByCategory(Category category, ArticleServices articleServices, CategoryServices categoryServices, Router router, UserServices userServices)
        {
            InitializeComponent();
            _selectedCategory = category;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
            _router = router;
            _userServices = userServices;
            List<Article> articles = new List<Article>(_articleServices.GetArticlesByCategory(_selectedCategory.Id));

            LoadArticles();
            UpdateUI();
        }

        //Metode til at hente og vise artikler baseret på den valgte kategori
        private void LoadArticles()
        {         
                IEnumerable<Article> articles = _articleServices.GetArticlesByCategory(_selectedCategory.Id);
                List<Article> articleList = new List<Article>(articles);
                ArticlesItemsControl.ItemsSource = articleList;         
        }

        //Metode til at opdatere UI-elementer baseret på den valgte kategori
        private void UpdateUI()
        {
            CategoryNameTextBlock.Text = _selectedCategory.Name;
        }

        //Metode til at navigere til den valgte artikel, når den klikkes på
        private void ArticleCategoryPageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _articleServices.SelectedArticle = (Article)button.DataContext;
            _userServices.UserProfile.UpdateUserProfileView(_articleServices.SelectedArticle.ItemProfile);
            _router.NavigateTo(Routes.Article);
        }
    }
}
