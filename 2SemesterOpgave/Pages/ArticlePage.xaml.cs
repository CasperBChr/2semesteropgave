using _2SemesterOpgave.Models;
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
    //Kodet af Camilla
    public partial class ArticlePage : UserControl
    {
        private Router _router;
        private Models.Article _currentArticle;

        //Constructor der tager en Router som parameter for at kunne navigere til andre sider
        public ArticlePage(Router router = null)
        {
            InitializeComponent();
            _router = router;
        }

        //DataContext for at binde Article-objektet til ArticlePage
        public void SetArticle(Models.Article article)
        {
            this.DataContext = article;
            _currentArticle = article;
        }

        //Funktion der navigerer til ejeren af artiklens side
        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentArticle?.Owner != null && _router != null)
            {
                //Navigerer til ejeren af artiklens "UserProfilePage" 
                _router.NavigateTo(Routes.UserProfile);

            }
        }
    }

}
