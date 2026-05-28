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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        ArticleServices _articleServices;

        public HomePage(ArticleServices articleServices)
        {
            InitializeComponent();

            _articleServices = articleServices;

            ArticlesItemsControl.ItemsSource = _articleServices.GetNewestArticles();
        }


    }
}
