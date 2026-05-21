using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using _2SemesterOpgave.Pages;

namespace _2SemesterOpgave
{
    public class Router
    {
        public Routes CurrentPage { get; set; } // Property: gemmer den aktuelle side, som standard er sat til Home
        public ContentControl PageControl;
        public Router(ContentControl pageControl)
        {
            CurrentPage = Routes.Home;
            PageControl = pageControl;
            PageControl.Content = new HomePage();
        }
        public void NavigateTo(Routes route)
        {
            switch (route)
            {
                case Routes.Home:
                    PageControl.Content = new HomePage();
                    break;
                case Routes.Explore:
                    PageControl.Content = new ExplorePage();    
                    break;
                case Routes.Categories:
                    PageControl.Content = new CategoryPage();   
                    break;
                case Routes.Announcements:
                    PageControl.Content = new NewsPage();
                    break;
                case Routes.Favorites:
                    PageControl.Content = new FavoritPage();
                    break;
                case Routes.MyOrders:
                    PageControl.Content = new MyOrdersPage();
                    break;
                case Routes.MyAccount:
                    PageControl.Content = new MyAccountPage();
                    break;
                case Routes.Messages:
                    PageControl.Content = new MessagesPage();
                    break;
                case Routes.Support:
                    PageControl.Content = new SupportPage();
                    break;
                case Routes.Overview:
                    PageControl.Content = new OverViewPage();
                    break;
                case Routes.Article:
                    PageControl.Content = new ArticlePage();
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


