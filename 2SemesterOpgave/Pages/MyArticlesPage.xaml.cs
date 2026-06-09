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
    /// Kodet af Camilla. Interaction logic for MyArticlesPage.xaml
    /// </summary>
    public partial class MyArticlesPage : UserControl
    {
        private ArticleServices _articleServices;
        private Router _router;
        private UserServices _userServices;
        private AuthServices _authServices;
        private User _selectedUser;

        //Constructor
        public MyArticlesPage(ArticleServices articleServices, CategoryServices categoryServices, Router router, UserServices userServices, AuthServices authServices)
        {
            InitializeComponent();
            _articleServices = articleServices;
            _router = router;
            _userServices = userServices;
            _authServices = authServices;
            _selectedUser = _userServices.CurrentUser;

            //Liste med artikler tilhørende den valgte bruger
            List<Article> articles = new List<Article>(_articleServices.GetArticlesByOwner(_selectedUser.Id));
                        
            LoadArticles();
            UpdateUI();
        }

        //Metode til at opdatere UI baseret på om brugeren har artikler eller ej
        private void LoadArticles()
        {
            //Liste med artikler tilhørende den valgte bruger
            IEnumerable<Article> articles = _articleServices.GetArticlesByOwner(_selectedUser.Id);

            //Liste der bruges til at binde artiklerne til UI
            List<Article> articleList = new List<Article>(articles);
            ArticlesItemsControl.ItemsSource = articleList;
        }

        //Metode til at opdatere UI-elementer baseret på den valgte kategori
        private void UpdateUI()
        {
            OwnerTextBlock.Text = _selectedUser.FirstName;
        }

        //Knap til at komme ind på EditPage for at redigere en artikel
        private void EditPageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _articleServices.SelectedArticle = (Article)button.DataContext;
            _userServices.UserProfile.UpdateUserProfileView(_articleServices.SelectedArticle.ItemProfile);
            _router.NavigateTo(Routes.EditArticlePage);
        }
    }
}
