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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        ArticleServices _articleServices;
        Router _router;


        public HomePage(Router router, ArticleServices articleServices)
        {
            InitializeComponent();

            _router = router;
            _articleServices = articleServices;

            ArticlesItemsControl.ItemsSource = _articleServices.GetNewestArticles();
        }

		private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
            Button button = (Button)sender;
            _articleServices.SelectedArticle = (Article)button.DataContext;
            _router.NavigateTo(Routes.Article);
		}
	}
}
