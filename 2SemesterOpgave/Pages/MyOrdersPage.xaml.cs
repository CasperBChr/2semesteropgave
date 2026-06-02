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

        public MyOrdersPage()
        {
            InitializeComponent();
            CurrentUser = new User();
            Rentals = CreateDemoRentals();
            DataContext = this;
        }

        public MyOrdersPage(UserServices userServices, ObservableCollection<Rental> rentals)
        {
            InitializeComponent();
            CurrentUser = userServices?.CurrentUser ?? new User();
            Rentals = rentals != null && rentals.Count > 0 ? rentals : CreateDemoRentals();
            DataContext = this;
        }

        private static ObservableCollection<Rental> CreateDemoRentals()
        {
            User renter = new User { Id = 1, Username = "camitøs", FirstName = "Camilla", LastName = "Nielsen" };
            User owner = new User { Id = 2, Username = "Sofia M.", FirstName = "Sofia", LastName = "M." };

            Brand brand = new Brand("H&M", "Demo brand", "");
            Article article1 = new Article("Satin Midi Dress", "Rød kjole", 1000, 250, true, false, false, true)
            {
                Brand = brand,
                Owner = owner
            };

            Article article2 = new Article("Silk Evening Gown", "Blå kjole", 900, 200, true, false, false, true)
            {
                Brand = brand,
                Owner = owner
            };

            return new ObservableCollection<Rental>
            {
                new Rental(
                    renter, owner, article1,
                    new DateOnly(2026, 6, 8),
                    new DateOnly(2026, 6, 12),
                    1006m,
                    DateTime.Now,
                    new ShippingOption(),
                    new InsuranceOption()),

                new Rental(
                    renter, owner, article2,
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
