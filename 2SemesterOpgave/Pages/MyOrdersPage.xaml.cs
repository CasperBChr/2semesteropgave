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
using System.Collections.ObjectModel;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for MyOrdersPage.xaml
    /// </summary>
    public partial class MyOrdersPage : UserControl
    {
        public User CurrentUser { get; private set; }
        public ObservableCollection<Rental> Rentals { get; private set; }

        UserServices _userServices;
        ArticleServices _articleServices;

        public MyOrdersPage()
        {
            InitializeComponent();
            CurrentUser = new User();
            Rentals = CreateDemoRentals();
            DataContext = this;
        }

        public MyOrdersPage(UserServices userServices, ArticleServices articlesServices, ObservableCollection<Rental> rentals)
        {
            InitializeComponent();
            _userServices = userServices;
            _articleServices = articlesServices;
            CurrentUser = userServices?.CurrentUser ?? new User();
            Rentals = rentals != null && rentals.Count > 0 ? rentals : CreateDemoRentals();
            DataContext = this;
        }

        private ObservableCollection<Rental> CreateDemoRentals()
        {
            List<User> users = new List<User>(_userServices.GetAllUsers());
            User user1 = users[0];
            User user2 = users[1];
            List<Article> articles = new List<Article>(_articleServices.GetAllArticles());

			//User owner = new User { Id = 2, Username = "Sofia M.", FirstName = "Sofia", LastName = "M." };

            //Brand brand = new Brand("H&M", "Demo brand", "");
            //Article article1 = new Article("Satin Midi Dress", "Rød kjole", 1000, 250, true, false, false, true)
            //{
            //    Brand = brand,
            //    Owner = owner
            //};

            //Article article2 = new Article("Silk Evening Gown", "Blå kjole", 900, 200, true, false, false, true)
            //{
            //    Brand = brand,
            //    Owner = owner
            //};

            return new ObservableCollection<Rental>
            {
                new Rental(
                    user1, user2, articles[0],
                    new DateOnly(2026, 6, 8),
                    new DateOnly(2026, 6, 12),
                    1006m,
                    DateTime.Now,
                    new ShippingOption(),
                    new InsuranceOption()),

                new Rental(
					user2, user1, articles[1],
                    new DateOnly(2026, 5, 29),
                    new DateOnly(2026, 6, 1),
                    907m,
                    DateTime.Now,
                    new ShippingOption(),
                    new InsuranceOption())
            };
        }
    }
}
