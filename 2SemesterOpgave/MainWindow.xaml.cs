using _2SemesterOpgave.Algoritme;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Services;
using _2SemesterOpgave.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace _2SemesterOpgave
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ContentControl _pageControl;
		Router _router;

        ObservableCollection<Article> _articles = new ObservableCollection<Article>();

		ArticleRepository _articleRepository;
		CategoryRepository _categoryRepository;
		BrandRepository _brandRepository;
		ColorRepository _colorRepository;
		SizeRepository _sizeRepository;
		RentalRepository _rentalRepository;
		ShippingOptionRepository _shippingOptionRepository;
		InsuranceOptionRepository _insuranceOptionRepository;
		ConversationRepository _conversationRepository;
		ReviewRepository _reviewRepository;

		UserServices _userServices;
		ArticleServices _articleServices;
		CategoryServices _categoryServices;
		BrandServices _brandServices;
		ColorServices _colorServices;
		SizeServices _sizeServices;
		RentalServices _rentalServices;
		ShippingOptionServices _shippingOptionServices;
		InsuranceOptionServices _insuranceOptionServices;
		ReviewServices _reviewServices;
        AuthServices _authServices;

		ConversationServices _conversationServices;
		UnreadBadgeServices _unreadBadgeService;

		FilterCriteria _filter = new FilterCriteria();

		IDatabaseFactory _db;

        public MainWindow(IDatabaseFactory db, UserServices userServices, ReviewServices reviewServices)
		{
			InitializeComponent();
			_pageControl = PageContentControl;

			_db = db;
			_userServices = userServices;
			
			_categoryRepository = new CategoryRepository(_db);
			_brandRepository = new BrandRepository(_db);
			_colorRepository = new ColorRepository(_db);
			_sizeRepository = new SizeRepository(_db);
			_articleRepository = new ArticleRepository(_db);
			_shippingOptionRepository = new ShippingOptionRepository(_db);
			_insuranceOptionRepository = new InsuranceOptionRepository(_db);
			_rentalRepository = new RentalRepository(_db);
			_conversationRepository = new ConversationRepository(_db);
			_reviewRepository = new ReviewRepository(_db);

			_categoryServices = new CategoryServices(_categoryRepository);
			_brandServices = new BrandServices(_brandRepository);
			_colorServices = new ColorServices(_colorRepository);
			_sizeServices = new SizeServices(_sizeRepository);
			_articleServices = new ArticleServices(_articleRepository, _userServices, _brandServices, _categoryServices, _colorServices, _sizeServices);
			_shippingOptionServices = new ShippingOptionServices(_shippingOptionRepository);
			_insuranceOptionServices = new InsuranceOptionServices(_insuranceOptionRepository);
			_rentalServices = new RentalServices(_rentalRepository, _userServices, _articleServices, _shippingOptionServices, _insuranceOptionServices);
			_conversationServices = new ConversationServices(_conversationRepository, userServices);
			_unreadBadgeService = new UnreadBadgeServices(_conversationServices, _userServices);
			_reviewServices = reviewServices;

			_router = new Router(_pageControl, _articles, _categoryServices, _articleServices, _userServices, _rentalServices, _shippingOptionServices, _insuranceOptionServices, _sizeServices, _brandServices, _filter, _colorServices, _reviewServices, _authServices, _conversationServices, _unreadBadgeService);
			
			InitializeAlgorithm(_userServices.CurrentUser);

			_userServices.StartFakeBots(_conversationServices);

			_userServices.FakeConversation.OnNewMessage += (conversation) =>
			{
				_unreadBadgeService.Refresh();
			};

			_unreadBadgeService.UnreadCountChanged += OnUnreadCountChanged;
			_unreadBadgeService.Refresh();

			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Home));
			//_router.NavigateTo(Routes.Home);
		}

		private void OnUnreadCountChanged(int count)
		{
			Dispatcher.Invoke(() =>
			{
				if (count > 0)
				{
					MessagesBadgeBorder.Visibility = Visibility.Visible;
					MessagesAmountTextBlock.Text = count.ToString();
				}
				else
				{
					MessagesBadgeBorder.Visibility = Visibility.Collapsed;
				}
			});
		}

		//Algoritme, kodet af Camilla
		public void InitializeAlgorithm(User user)
        {
            //Liste der henter kategorier
            IEnumerable<Category> categories = _categoryServices.GetAllCategories();

			//Opretter en liste af features baseret på kategorierne, som bruges til at oprette brugerprofilen
			List<string> features = new List<string>();
            foreach (Category category in categories)
            {
                features.Add(category.Name);
            }

			//Katalog af elementer
			List<ItemProfile> catalog = new List<ItemProfile>();
			List<Article> articles = new List<Article>(_articleServices.GetAllArticles());

			for (int i = 0; i < articles.Count; i++)
			{
				Article article = articles[i];

				if (article.Category == null)
				{
					continue;
				}
				ItemProfile itemProfile = new ItemProfile
				{
					ArticleID = article.Id,
					Article = article,
					Features = new Dictionary<string, double>
						{
							{ article.Category.Name, 1.0 }
						}
				};

				article.ItemProfile = itemProfile;
				catalog.Add(itemProfile);
			}

			//Opret en ny brugerprofil til algoritme
			_userServices.UserProfile = new UserProfile(user.Id.ToString(), features);
            _userServices.UserProfile.User = _userServices.GetById(user.Id);
        }

        private void HomeMenuButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Home));
			//_router.NavigateTo(Routes.Home);
		}

		private void MyOrderButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyOrders));
			//_router.NavigateTo(Routes.MyOrders);
		}

		private void ExplorerMenuButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Explore));
			//_router.NavigateTo(Routes.Explore);
		}

		private void CategoriMenuButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Categories));
			//_router.NavigateTo(Routes.Categories);
		}

		private void FavoritPageButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Favorites));
			//_router.NavigateTo(Routes.Favorites);
		}

		private void MyAccountButton_Click(object sender, RoutedEventArgs e)
		{
			_userServices.TargetUser = _userServices.CurrentUser;
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyAccount));
			//_router.NavigateTo(Routes.MyAccount);
		}

		private void MessagesButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Messages));
			//	_router.NavigateTo(Routes.Messages);
		}

		private void CreateArticleButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.CreateArticle));
			//_router.NavigateTo(Routes.CreateArticle);
		}
		private void ForYouButton_Click(object sender, RoutedEventArgs e)
		{
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.ForYou));
			//_router.NavigateTo(Routes.ForYou);
		}
        private void MyArticlePageButton_Click(object sender, RoutedEventArgs e)
        {
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyArticlesPage));
			//_router.NavigateTo(Routes.MyArticlesPage);
        }

		private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (SearchBox.Text == "Søg...")
			{
				SearchBox.Text = string.Empty;
				SearchBox.Foreground = new SolidColorBrush(Colors.Black);
			}
		}

		private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(SearchBox.Text))
			{
				SearchBox.Text = "Søg...";
				SearchBox.Foreground = new SolidColorBrush(Colors.Gray);
			}
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (SearchBox.Text == "Søg...")
			{
				return;
			}
			_filter.SearchText = SearchBox.Text;
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Overview));
			//_router.NavigateTo(Routes.Overview);
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			_router.GoBack();
		}
	}
}