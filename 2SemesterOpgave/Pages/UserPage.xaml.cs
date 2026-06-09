using System.ComponentModel;
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
    public partial class UserPage : UserControl
    {
        UserServices _userServices;
        public User CurrentUser { get; private set; }
		public float UserRating { get; private set; }
        ArticleServices _articleServices;
        Router _router;
        ReviewServices _reviewServices;
		public UserPage(UserServices userServices, ArticleServices articleServices, Router router, ReviewServices reviewServices)
		{
			InitializeComponent();
			_userServices = userServices;
			_articleServices = articleServices;
            _reviewServices = reviewServices;
			_router = router;
            if(_userServices.TargetUser == null)
            {
				MessageBox.Show("Kan ikke finde brugeren.");
			}

            if(_userServices.CurrentUser == _userServices.TargetUser) 
            {
                FollowButton.Visibility = Visibility.Collapsed;
			}
            CurrentUser = _userServices.TargetUser;
			UserRating = _reviewServices.GetAverageRating(CurrentUser.Id);
			//DataContext = CurrentUser;
			DataContext = this;
			ArticlesItemsControl.ItemsSource = _articleServices.GetAllArticlesByOwner(_userServices.TargetUser); // Sætter ItemsSource til de nyeste artikler, så de vises i UI
			UpdateFollowButton();
		}

		private void ReviewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userServices.TargetUser.Id == 0)
            {
                MessageBox.Show("Kan ikke finde brugeren.");
                return;
            }

            _reviewServices.SetReviewTarget(_userServices.TargetUser);
            _router.NavigateTo(Routes.Reviews);
        }

		private void UpdateFollowButton()
		{
			if (_userServices.CurrentUser.Id == _userServices.TargetUser.Id)
			{
				FollowButton.Visibility = Visibility.Collapsed;
				return;
			}

			FollowButton.Visibility = Visibility.Visible;

			bool isFollowing = _userServices.IsFollowing(_userServices.CurrentUser, _userServices.TargetUser);

			if (isFollowing)
			{
				FollowButton.Content = "Følger";
				FollowButton.Background = Brushes.Red;
			}
			else
			{
				FollowButton.Content = "Følg";
				FollowButton.Background = Brushes.Turquoise;
			}
		}

		private void FollowButton_Click(object sender, RoutedEventArgs e)
		{
			if (_userServices.CurrentUser == null || _userServices.TargetUser == null)
				return;

			bool isFollowing = _userServices.IsFollowing(_userServices.CurrentUser, _userServices.TargetUser);

			if (isFollowing)
			{
				_userServices.RemoveFollower(_userServices.CurrentUser, _userServices.TargetUser);
				_userServices.TargetUser.FollowersCount--;
				_userServices.CurrentUser.FollowingCount--;
			}
			else
			{
				_userServices.AddFollower(_userServices.CurrentUser, _userServices.TargetUser);
				_userServices.TargetUser.FollowersCount++;
				_userServices.CurrentUser.FollowingCount++;
			}

			UpdateFollowButton();
		}

		private void ArticlePageButton_Click(object sender, RoutedEventArgs e) // Event handler for når en artikelknap klikkes, som navigerer til ArticlePage med den valgte artikel
        {
            Button button = (Button)sender; // (Button) er typecasting, det fortæller kompileren at "sender" er en Button
            _articleServices.SelectedArticle = (Article)button.DataContext; // Her sætter vi den valgte artikel i ArticleServices, så den kan bruges på ArticlePage
            _router.NavigateTo(Routes.Article); // Navigerer til ArticlePage ved at kalde NavigateTo på Router med den relevante rute
        }
    }
}