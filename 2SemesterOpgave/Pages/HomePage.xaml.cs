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
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        ArticleServices _articleServices;
        UserServices _userServices;
        CategoryServices _categoryServices;
        Router _router;


        public HomePage(Router router, ArticleServices articleServices, UserServices userServices, CategoryServices categoryServices)
        {
            InitializeComponent();

            _router = router;
            _articleServices = articleServices;
            _userServices = userServices;
			_categoryServices = categoryServices;
            ArticlesItemsControl.ItemsSource = _articleServices.GetNewestArticles();
			RandomArticlesItemsControl.ItemsSource = _articleServices.GetRandomArticles(10);
            RandomCategoriesItemsControl.ItemsSource = _categoryServices.GetRandomCategories(10);
        }

		private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
            Button button = (Button)sender;
            _articleServices.SelectedArticle = (Article)button.DataContext;
			_userServices.UserProfile.UpdateUserProfileView(_articleServices.SelectedArticle.ItemProfile);
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));
			//_router.NavigateTo(Routes.Article);
		}
	}
}
