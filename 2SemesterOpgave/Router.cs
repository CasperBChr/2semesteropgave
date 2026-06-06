using System.Collections.ObjectModel;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Pages;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave
{
    public class Router
    {
        Routes _currentPage;
        public Routes CurrentPage { get { return _currentPage; } } // Property: gemmer den aktuelle side, som standard er sat til Home
        ContentControl _pageControl;
        Category _selectedCategory;

        ObservableCollection<Article> _articles;
        ObservableCollection<Rental> _rentals;
        CategoryServices _categoryServices;
        ArticleServices _articleServices;
        UserServices _userServices;
        RentalServices _rentalServices;
        ShippingOptionServices _shippingOptionServices;
        InsuranceOptionServices _insuranceOptionServices;
        SizeServices _sizeServices; 
        FilterCriteria _currentFilter;
        BrandServices _brandServices;
        ColorServices _colorServices;
        ReviewServices _reviewServices;
        AuthServices _authServices;
        ConversationServices _conversationServices;
        UnreadBadgeServices _unreadBadgeService;
		public Router(ContentControl pageControl, ObservableCollection<Article> articles, CategoryServices categoryServices, ArticleServices articleServices, UserServices userServices, RentalServices rentalServices, ShippingOptionServices shippingOptionServices, InsuranceOptionServices insuranceOptionServices, SizeServices sizeServices, BrandServices brandServices, FilterCriteria filterCriteria, ColorServices colorServices, ReviewServices reviewServices, AuthServices authServices, ConversationServices conversationServices, UnreadBadgeServices unreadBadgeServices)
        {
            _currentPage = Routes.Home;
            _pageControl = pageControl;
            _articles = articles;
			_categoryServices = categoryServices;
            _categoryServices = categoryServices;
            _articleServices = articleServices;
            _reviewServices = reviewServices;
            _userServices = userServices;
            _colorServices = colorServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            _shippingOptionServices = shippingOptionServices;
            _insuranceOptionServices = insuranceOptionServices;
            _rentalServices = rentalServices;
            _currentFilter = filterCriteria;
            _authServices = authServices;
			_conversationServices = conversationServices;
            _unreadBadgeService = unreadBadgeServices;

			_rentals = _rentalServices.GetAll();
        }

        public void SetFilter(FilterCriteria filter)
        {
            _currentFilter = filter;
        }

		public void SetSelectedCategory(Category category)
		{
			_selectedCategory = category;
		}

		public void NavigateTo(Routes route)
		{
			switch (route)
            {
                case Routes.Home:
                    _pageControl.Content = new HomePage(this, _articleServices, _userServices, _categoryServices);
					_currentPage = Routes.Home;
                    break;
                case Routes.Explore:
                    _pageControl.Content = new ExplorePage(this, _articleServices, _userServices);
					_currentPage = Routes.Explore;
                    break;
				case Routes.Categories:
					_pageControl.Content = new CategoryPage(_categoryServices, this, _articleServices);
					_currentPage = Routes.Categories;
					break;
                case Routes.Favorites:
                    _pageControl.Content = new FavoritesPage(this, _userServices, _articleServices);
                    _currentPage = Routes.Favorites;
                    break;
                case Routes.MyOrders:
                    _pageControl.Content = new MyOrdersPage(this, _userServices, _reviewServices, _rentalServices);
                    _currentPage = Routes.MyOrders;
                    break;
                case Routes.MyAccount:
                    _pageControl.Content = new MyAccountPage(this, _userServices);
                    _currentPage = Routes.MyAccount;
                    break;
                case Routes.Messages:
                    _pageControl.Content = new MessagesPage(this, _userServices);
                    _currentPage = Routes.Messages;
                    break;
                case Routes.Overview:
                    _pageControl.Content = new OverviewPage(this, _articleServices, _currentFilter);
                    _currentPage = Routes.Overview;
                    break;
                case Routes.Article:
                    _pageControl.Content = new ArticlePage(this, _articleServices, _categoryServices, _userServices);
                    _currentPage = Routes.Article;
                    break;
                case Routes.ForYou:
                    _pageControl.Content = new ForYouPage(this, _userServices, _articleServices, _categoryServices);
                    _currentPage = Routes.ForYou;
                    break;
				case Routes.Notifications:
					_pageControl.Content = new NotificationsPage();
					_currentPage = Routes.Notifications;
					break;
				case Routes.Message:
					_pageControl.Content = new MessagePage(this, _userServices, _conversationServices, _unreadBadgeService);
					_currentPage = Routes.Message;
					break;
				case Routes.UserProfile:
					_pageControl.Content = new UserPage(_userServices, _articleServices, this, _reviewServices);
					_currentPage = Routes.UserProfile;
					break;
                case Routes.CreateArticle:
                    _pageControl.Content = new CreateArticlePage(this, _articleServices, _categoryServices, _sizeServices, _brandServices);              
                    _currentPage = Routes.CreateArticle;
                    break;
                case Routes.Rent:
                    _pageControl.Content = new RentPage(this, _articleServices, _userServices, _shippingOptionServices, _insuranceOptionServices);              
                    _currentPage = Routes.Rent;
                    break;
                case Routes.ArticleSortByCategory:
                    _pageControl.Content = new ArticleSortByCategory(_selectedCategory, _articleServices, _categoryServices, this, _userServices);
                    _currentPage = Routes.ArticleSortByCategory;
                    break;
                case Routes.Reviews:
                    _pageControl.Content = new ReviewsPage(_reviewServices, _userServices);
                    _currentPage = Routes.Reviews;
                    break;
                case Routes.EditArticlePage:
                    _pageControl.Content = new EditArticlePage(this, _articleServices, _categoryServices, _sizeServices, _brandServices);
                    _currentPage = Routes.EditArticlePage;
                    break;
                case Routes.MyArticlesPage:
                    _pageControl.Content = new MyArticlesPage(_articleServices, _categoryServices, this, _userServices, _authServices);
                    _currentPage = Routes.MyArticlesPage;
                    break;

            }
        }
    }

    public enum Routes
    {
        Home = 0,
        Explore = 1,
        Categories = 2,
        Favorites = 3,
        MyOrders = 4,
        MyAccount = 5,
        Messages = 6,
        Overview = 7,
        Article = 8,
        ForYou = 9,
        Notifications = 10,
        Message = 11,
        UserProfile = 12,
        CreateArticle = 13,
        Rent = 14,
        Reviews = 15,
        ArticleSortByCategory = 16,
        EditArticlePage = 17,
        MyArticlesPage = 18
    }
}