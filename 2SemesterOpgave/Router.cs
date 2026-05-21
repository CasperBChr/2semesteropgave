using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using _2SemesterOpgave.Pages;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave
{
    public class Router
    {
        public Routes CurrentPage { get; set; } // Property: gemmer den aktuelle side, som standard er sat til Home
        public ContentControl PageControl;
        
        private ObservableCollection<Article> articles;
        private ObservableCollection<Category> categories;
        public Router(ContentControl pageControl, ObservableCollection<Article> articles, ObservableCollection<Category> categories)
        {
            CurrentPage = Routes.Home;
            PageControl = pageControl;
            this.articles = articles;
            this.categories = categories;

            PageControl.Content = new HomePage();
        }
        public void NavigateTo(Routes route)
        {
            switch (route)
            {
                case Routes.Home:
                    PageControl.Content = new HomePage();
                    CurrentPage = Routes.Home;
                    break;
                case Routes.Explore:
                    PageControl.Content = new ExplorePage(articles);
                    CurrentPage = Routes.Explore;
                    break;
                case Routes.Categories:
                    PageControl.Content = new CategoryPage(categories);
                    CurrentPage = Routes.Categories;
                    break;
                case Routes.Announcements:
                    PageControl.Content = new NewsPage();
                    CurrentPage = Routes.Announcements;
                    break;
                case Routes.Favorites:
                    PageControl.Content = new FavoritPage();
                    CurrentPage = Routes.Favorites;
                    break;
                case Routes.MyOrders:
                    PageControl.Content = new MyOrdersPage();
                    CurrentPage = Routes.MyOrders;
                    break;
                case Routes.MyAccount:
                    PageControl.Content = new MyAccountPage();
                    CurrentPage = Routes.MyAccount;
                    break;
                case Routes.Messages:
                    PageControl.Content = new MessagesPage();
                    CurrentPage = Routes.Messages;
                    break;
                case Routes.Support:
                    PageControl.Content = new SupportPage();
                    CurrentPage = Routes.Support;
                    break;
                case Routes.Overview:
                    PageControl.Content = new OverViewPage(articles);
                    CurrentPage = Routes.Overview;
                    break;
                case Routes.Article:
                    PageControl.Content = new ArticlePage();
                    CurrentPage = Routes.Article;
                    break;
            }

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
    }


