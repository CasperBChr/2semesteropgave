using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
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

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Camilla. Interaction logic for ArticlePage.xaml.
    /// </summary>
    public partial class ArticlePage : UserControl
    {
        private Router _router;
        private Article _currentArticle;
        private ArticleServices _articleServices;
        private CategoryServices _categoryServices;
        UserServices _userServices;
		private bool _isFavorite;
		ConversationServices _conversationServices;
		//Constructor der tager en Router som parameter for at kunne navigere til andre sider
		public ArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices, UserServices userService, ConversationServices conversationServices)
        {
            
            //Article article = new Article("Ganni", "Beskrivelse af Ganni-artiklen", new List<Category>(), new List<SubCategory>(), new Models.Size(36), 100.60f, "Hvid", new Brand("Ganni", "Ganni Brand", "Logo"), false, 10000f, false, false, true, new User("John Doe", "john@example.com", "hej", 6));
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
			_conversationServices = conversationServices;
			this.DataContext = _articleServices.SelectedArticle;
			_currentArticle = _articleServices.SelectedArticle;
			_userServices = userService;
			
			SetArticle(_currentArticle);
		}

		//DataContext for at binde Article-objektet til ArticlePage
		public void SetArticle(Article article)
		{
			this.DataContext = article;
			_currentArticle = article;

			_isFavorite = _articleServices.IsFavorite(_userServices.CurrentUser, _currentArticle);

			UpdateFavoriteUI();
		}


		//Funktion der navigerer til chat med ejer af artiklen
		private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
			//if (_currentArticle?.Owner != null && _router != null)
			//{
			//    _router.NavigateTo(Routes.Message);
			//}

			if (_currentArticle?.Owner == null || _router == null)
			{
				return;
			}

			User currentUser = _userServices.CurrentUser;
			User owner = _currentArticle.Owner;

			Conversation conversation = _conversationServices.GetOrCreateConversation(currentUser, owner);
			_conversationServices.TargetConversation = conversation;

			_router.NavigateTo(Routes.Message);
		}
        //Metode der navigerer til udlejnings siden for artiklen
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Rent);
        }

		private void OwnerProfileButton_Click(object sender, RoutedEventArgs e)
		{
            Button button = (Button)sender;
            Article article = (Article)button.DataContext;
			_userServices.TargetUser = article.Owner;
			_router.NavigateTo(Routes.UserProfile);
		}

		private void FavoriteButton_Click(object sender, RoutedEventArgs e)
		{
			if (_currentArticle == null || _userServices.CurrentUser == null)
				return;

			if (_isFavorite)
			{
				_articleServices.RemoveFavorite(_userServices.CurrentUser, _currentArticle);

				_isFavorite = false;
			}
			else
			{
				_articleServices.AddFavorite(_userServices.CurrentUser, _currentArticle);

				_isFavorite = true;
			}

			UpdateFavoriteUI();
		}

		private void UpdateFavoriteUI()
		{
			if(_isFavorite)
			{
				FavoriteButton.Content = "❤️";
				return;
			}
			FavoriteButton.Content = "🤍";
			//FavoriteButton.Content = _isFavorite ? "❤️" : "🤍";
		}
	}
}
