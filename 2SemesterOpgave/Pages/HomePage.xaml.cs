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
<<<<<<< Updated upstream
        ArticleServices _articleServices;
        Router _router;

<<<<<<< Updated upstream
        public HomePage(Router router, ArticleServices articleServices)
        {
            InitializeComponent();

            _router = router;
=======
=======
        ArticleServices _articelServices;
>>>>>>> Stashed changes
        public HomePage(ArticleServices articleServices)
        {
            InitializeComponent();

<<<<<<< Updated upstream
>>>>>>> Stashed changes
            _articleServices = articleServices;

            ArticlesItemsControl.ItemsSource = _articleServices.GetNewestArticles();
=======
            _articelServices = articleServices;

            ArticlesItemsControl.ItemsSource = _articelServices.GetNewestArticles();
>>>>>>> Stashed changes
        }

		private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
            _router.NavigateTo(Routes.Article);

            Button button = (Button)sender;

            _articleServices.SelectedArticle = (Article)button.DataContext;
		}
	}
}
