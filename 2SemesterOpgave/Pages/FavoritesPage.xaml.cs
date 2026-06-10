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
	/// Interaction logic for FavoritesPage.xaml
	/// </summary>
	public partial class FavoritesPage : UserControl
	{
		Router _router;
		UserServices _userServices;
		ArticleServices _articleServices;

		public FavoritesPage(Router router, UserServices userServices, ArticleServices articleServices)
		{
			InitializeComponent();

			_router = router;
			_userServices = userServices;
			_articleServices = articleServices;

			LoadFavorites();
		}

        //Metode der indlæser brugerens favorit artikler og viser dem i UI
        private void LoadFavorites()
		{
			List<Article> favorites = new List<Article>(
				_articleServices.GetAllFavoritedArticlesByUser(
					_userServices.CurrentUser.Id));

			FavoriteArticlesItemsControl.ItemsSource = favorites;
		}

        //Metode der håndterer klik på en favorit artikel og navigerer til ArticlePage
        private void FavoriteArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			Article article = (Article)button.DataContext;

			_articleServices.SelectedArticle = article;

			if (article.ItemProfile != null)
			{
				_userServices.UserProfile.UpdateUserProfileView(article.ItemProfile);
			}

			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));
			//_router.NavigateTo(Routes.Article);
		}
	}
}
