using _2SemesterOpgave.Algoritme;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    //Kodet af Camilla
    public partial class ForYouPage : UserControl
    {
        public Router Router;
        public UserServices UserServices;
        public ArticleServices ArticleServices;
        public CategoryServices CategoryServices;
        public ForYouPage(Router router, UserServices userService, ArticleServices articleService, CategoryServices categoryService)
        {
            InitializeComponent();
            Router = router;
            UserServices = userService;
            ArticleServices = articleService;
            CategoryServices = categoryService;
            List<ItemProfile> catalog = new List<ItemProfile>();
            for (int i = 0; i < new List<Article>(ArticleServices.GetAllArticles()).Count; i++)
            {
                catalog.Add(new List<Article>(ArticleServices.GetAllArticles())[i].ItemProfile);
            }
            PrintRecommendations(UserServices.UserProfile, catalog);
        }

        //Metode der printer anbefalinger ud fra algoritmen
        static void PrintRecommendations(UserProfile user, List<ItemProfile> catalog)
        {
            //Henter og printer anbefalingerne for brugeren
            var recs = ContentBasedAlgorithm.GetRecommendations(user, catalog);

            //Printer anbefalingerne i konsollen
            foreach (var rec in recs)
            {
                //Udskriver anbefalingerne i konsollen
                Debug.WriteLine($"- {rec.Item.Name} (Match Score: {rec.Score:F2})");
            }

        }

        private void ForYouArticlePageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ArticleServices.SelectedArticle = (Article)button.DataContext;
            Router.NavigateTo(Routes.Article);
        }
              
        
    }
}
