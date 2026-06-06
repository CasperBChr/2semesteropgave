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
		private readonly Router _router;
		private readonly UserServices _userServices;
		private readonly ArticleServices _articleServices;

		public FavoritesPage(Router router, UserServices userServices, ArticleServices articleServices)
		{
			InitializeComponent();

			_router = router;
			_userServices = userServices;
			_articleServices = articleServices;

			LoadFavorites();
		}

		private void LoadFavorites()
		{
			List<Article> favorites = new List<Article>(
				_articleServices.GetAllFavoritedArticlesByUser(
					_userServices.CurrentUser.Id));

			FavoriteArticlesItemsControl.ItemsSource = favorites;
		}

		private void FavoriteArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;

			Article article = (Article)button.DataContext;

			_articleServices.SelectedArticle = article;

			if (article.ItemProfile != null)
			{
				_userServices.UserProfile.UpdateUserProfileView(article.ItemProfile);
			}

			_router.NavigateTo(Routes.Article);
		}
	}
}
