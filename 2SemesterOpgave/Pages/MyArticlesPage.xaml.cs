using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
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
    /// Interaction logic for MyArticlesPage.xaml
    /// </summary>
    public partial class MyArticlesPage : UserControl
    {
        private ArticleServices _articleServices;
        private Router _router;
        private UserServices _userServices;
        private AuthServices _authServices;
        private User _selectedUser;
 
        public MyArticlesPage(ArticleServices articleServices, CategoryServices categoryServices, Router router, UserServices userServices, AuthServices authServices, User selectedUser)
        {
            InitializeComponent();
            _articleServices = articleServices;
            _router = router;
            _userServices = userServices;
            _authServices = authServices;
            _selectedUser = selectedUser;

            List<Article> articles = new List<Article>(_articleServices.GetArticlesByOwner(_selectedUser.Id));

            LoadArticles();
            UpdateUI();
        }

        private void LoadArticles()
        {
            IEnumerable<Article> articles = _articleServices.GetArticlesByOwner(_selectedUser.Id);
            List<Article> articleList = new List<Article>(articles);
            ArticlesItemsControl.ItemsSource = articleList;
        }

        //Metode til at opdatere UI-elementer baseret på den valgte kategori
        private void UpdateUI()
        {
            OwnerTextBlock.Text = _selectedUser.FirstName;
        }

        private void EditPageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _articleServices.SelectedArticle = (Article)button.DataContext;
            _userServices.UserProfile.UpdateUserProfileView(_articleServices.SelectedArticle.ItemProfile);
            _router.NavigateTo(Routes.EditArticlePage);
        }
    }
}
