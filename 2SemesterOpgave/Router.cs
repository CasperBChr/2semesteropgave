using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave
{
    public class Router
    {
        public Routes CurrentPage { get; set; } = Routes.Home; // Property: gemmer den aktuelle side, som standard er sat til Home



        switch (Routes)
        {
            case Routes.Home:
                break;
            case Routes.Explore:
                break;
            case Routes.Categories:
                break;
            case Routes.Announcements:
                break;
            case Routes.Favorites:
                break;
            case Routes.MyOrders:
                break;
            case Routes.MyAccount:
                break;
            case Routes.Messages:
                break;
            case Routes.Support:
                break;
        }
    }
}

    public enum Routes
    {
        Home,
        Explore,
        Categories,
        Announcements,
        Favorites,
        MyOrders,
        MyAccount,
        Messages,
        Support
    }
}
