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
    /// Interaction logic for OverviewPage.xaml
    /// </summary>
    /// 
    public partial class OverviewPage : UserControl
    {

		ArticleServices _articleServices;
		UserServices _userServices;
		FilterCriteria _filter;
		Router _router;

		public OverviewPage(Router router, ArticleServices articleServices, UserServices userServices, FilterCriteria filter)
		{
			InitializeComponent();
			_router = router;
			_articleServices = articleServices;
			_userServices = userServices;
			_filter = filter;

            //Indlæs artikler baseret på filterkriterierne og viser dem i UI
            ArticlesItemsControl.ItemsSource = _articleServices.GetFilteredArticles(_filter);
		}

        //Metode der håndterer klik på en artikel og navigerer til ArticlePage
        private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			_userServices.UserProfile.UpdateUserProfileView(_articleServices.SelectedArticle.ItemProfile);
			_articleServices.SelectedArticle = (Article)button.DataContext;
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));
			//_router.NavigateTo(Routes.Article);
		}
	}
}
