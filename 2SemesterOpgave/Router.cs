using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        
        ObservableCollection<Article> _articles;
        CategoryServices _categoryServices;
        ArticleServices _articleServices;
        UserServices _userServices;
        FilterCriteria _currentFilter;

		public Router(ContentControl pageControl, ObservableCollection<Article> articles, CategoryServices categoryServices, ArticleServices articleServices, UserServices userServices, FilterCriteria filterCriteria)
        {
			_currentPage = Routes.Home;
            _pageControl = pageControl;
            _articles = articles;
			_categoryServices = categoryServices;
            _articleServices = articleServices;
            _currentFilter = filterCriteria;
            _userServices = userServices;
            _pageControl.Content = new HomePage(this, _articleServices);
        }

		public void SetFilter(FilterCriteria filter)
		{
			_currentFilter = filter;
		}

		public void NavigateTo(Routes route, Article article = null)
		{
			switch (route)
            {
                case Routes.Home:
                    _pageControl.Content = new HomePage(this, _articleServices);
					_currentPage = Routes.Home;
                    break;
                case Routes.Explore:
                    _pageControl.Content = new ExplorePage(this, _articleServices);
					_currentPage = Routes.Explore;
                    break;
                case Routes.Categories:
                    _pageControl.Content = new CategoryPage(_categoryServices);
					_currentPage = Routes.Categories;
                    break;
                case Routes.Announcements:
                    _pageControl.Content = new NewsPage();
					_currentPage = Routes.Announcements;
                    break;
                case Routes.Favorites:
                    _pageControl.Content = new FavoritPage();
                    _currentPage = Routes.Favorites;
                    break;
                case Routes.MyOrders:
                    _pageControl.Content = new MyOrdersPage();
                    _currentPage = Routes.MyOrders;
                    break;
                case Routes.MyAccount:
                    _pageControl.Content = new MyAccountPage();
                    _currentPage = Routes.MyAccount;
                    break;
                case Routes.Messages:
                    _pageControl.Content = new MessagesPage(this, _userServices);
                    _currentPage = Routes.Messages;
                    break;
                case Routes.Support:
                    _pageControl.Content = new SupportPage();
                    _currentPage = Routes.Support;
                    break;
                case Routes.Overview:
					_pageControl.Content = new OverviewPage(this, _articleServices, _currentFilter);
					_currentPage = Routes.Overview;
                    break;
                case Routes.Article:
                    _pageControl.Content = new ArticlePage(this, _articleServices, _categoryServices);
                    if (article != null)
                    {
                        //DataContext til at binde data til UI-elementer.
                        ((ArticlePage)_pageControl.Content).SetArticle(article);
                    }
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
					_pageControl.Content = new MessagePage(this, _userServices);
					_currentPage = Routes.Message;
					break;
				case Routes.UserProfile:
					_pageControl.Content = new UserPage(_userServices);
					_currentPage = Routes.UserProfile;
					break;
                case Routes.CreateArticle:
                    _pageControl.Content = new CreateArticlePage(this, _articleServices, _categoryServices);              
                    _currentPage = Routes.CreateArticle;
                    break;
                case Routes.Rent:
                    _pageControl.Content = new RentPage(this, _articleServices, _userServices);              
                    _currentPage = Routes.Rent;
                    break;
            }
        }
    }

    public enum Routes 
    {
        Home = 0,
        Explore = 1,
        Categories = 2,
        Announcements = 3,
        Favorites = 4,
        MyOrders = 5,
        MyAccount = 6,
        Messages = 7,
        Support = 8,
        Overview = 9,
        Article = 10,
        ForYou = 11,
        Notifications = 12,
        Message = 13,
        UserProfile = 14,
        CreateArticle = 15,
        Rent = 16
    }
}



