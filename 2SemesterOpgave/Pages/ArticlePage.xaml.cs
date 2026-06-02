using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ArticlePage : UserControl
    {
        private Router _router;
        private Article _currentArticle;
        private ArticleServices _articleServices;
        private CategoryServices _categoryServices; 
        //Constructor der tager en Router som parameter for at kunne navigere til andre sider
        public ArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices)
        {
            
            //Article article = new Article("Ganni", "Beskrivelse af Ganni-artiklen", new List<Category>(), new List<SubCategory>(), new Models.Size(36), 100.60f, "Hvid", new Brand("Ganni", "Ganni Brand", "Logo"), false, 10000f, false, false, true, new User("John Doe", "john@example.com", "hej", 6));
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
            this.DataContext = _articleServices.SelectedArticle;
			_currentArticle = _articleServices.SelectedArticle;

		}

        //DataContext for at binde Article-objektet til ArticlePage
        public void SetArticle(Article article)
        {
            this.DataContext = article;
            _currentArticle = article;
        }

        //Funktion der navigerer til chat med ejer af artiklen
        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentArticle?.Owner != null && _router != null)
            {
                _router.NavigateTo(Routes.Message);

            }
        }

       
    }

}
