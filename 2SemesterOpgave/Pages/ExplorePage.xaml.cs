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
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for ExplorePage.xaml
    /// </summary>
    public partial class ExplorePage : UserControl
    {

        ArticleServices _articleServices;
        Router _router;
        UserServices _userServices;

        public ExplorePage(Router router, ArticleServices articleServices, UserServices userServices)
        {
            InitializeComponent();
			_articleServices = articleServices;
			_userServices = userServices;

            _router = router;
            ArticlesItemsControl.ItemsSource = _articleServices.GetAllArticles();
        }

		private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			_articleServices.SelectedArticle = (Article)button.DataContext;
            _userServices.UserProfile.UpdateUserProfileView(_articleServices.SelectedArticle.ItemProfile);
            _router.NavigateTo(Routes.Article);
		}
	}
}
