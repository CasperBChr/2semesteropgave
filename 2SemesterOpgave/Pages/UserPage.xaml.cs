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
        ArticleServices _articleServices;
        Router _router;
        ReviewServices _reviewServices;
		public UserPage(UserServices userServices, ArticleServices articleServices, Router router, ReviewServices reviewServices)
		{
			InitializeComponent();
			_userServices = userServices;
			_articleServices = articleServices;
            _reviewServices = reviewServices;
			DataContext = userServices.GetAllUsers()[0]; // Sætter DataContext til den første bruger i listen, så UI kan binde til den
            _userServices.TargetUser = _userServices.GetAllUsers()[1];
            TextBlockUsername.Text = _userServices.TargetUser.Username;
			ArticlesItemsControl.ItemsSource = _articleServices.GetNewestArticles(); // Sætter ItemsSource til de nyeste artikler, så de vises i UI
			_router = router;
		}

		private void ReviewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userServices.TargetUser.Id == 0)
            {
                MessageBox.Show("Kan ikke finde brugeren.");
                return;
            }

            if (_userServices.TargetUser.Id == _userServices.CurrentUser.Id)
            {
                MessageBox.Show("Du kan ikke vurdere dig selv.");
                return;
            }

            _reviewServices.SetReviewTarget(_userServices.TargetUser);
            _router.NavigateTo(Routes.Reviews);
        }
        private void ArticlePageButton_Click(object sender, RoutedEventArgs e) // Event handler for når en artikelknap klikkes, som navigerer til ArticlePage med den valgte artikel
        {
            Button button = (Button)sender; // (Button) er typecasting, det fortæller kompileren at "sender" er en Button
            _articleServices.SelectedArticle = (Article)button.DataContext; // Her sætter vi den valgte artikel i ArticleServices, så den kan bruges på ArticlePage
            _router.NavigateTo(Routes.Article); // Navigerer til ArticlePage ved at kalde NavigateTo på Router med den relevante rute
        }
    }
}