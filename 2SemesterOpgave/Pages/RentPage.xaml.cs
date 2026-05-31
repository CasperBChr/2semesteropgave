using _2SemesterOpgave.Services;
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
    public partial class RentPage : UserControl
    {
        private Router _router;
        private Models.Article _currentArticle;
        private ArticleServices _articleServices;
        private UserServices _userServices;
        public RentPage(Router router, ArticleServices articleServices, UserServices userServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _userServices = userServices;
        }
    }
}
