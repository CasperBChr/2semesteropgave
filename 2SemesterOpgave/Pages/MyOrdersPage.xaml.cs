using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    public partial class MyOrdersPage : UserControl
    {
        public User CurrentUser { get; private set; }
        public ObservableCollection<Rental> Rentals { get; private set; }

        private readonly Router? _router;
        private readonly ReviewServices? _reviewServices;

        public MyOrdersPage()
        {
            InitializeComponent();

            CurrentUser = new User();
            Rentals = CreateDemoRentals();

            DataContext = this;
        }

        public MyOrdersPage(
            Router router,
            UserServices userServices,
            ObservableCollection<Rental> rentals,
            ReviewServices reviewServices)
        {
            InitializeComponent();

            _router = router;
            _reviewServices = reviewServices;

            CurrentUser = userServices?.CurrentUser ?? new User();
            Rentals = rentals != null && rentals.Count > 0 ? rentals : CreateDemoRentals();

            DataContext = this;
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Rental rental = (Rental)button.DataContext;

            if (rental.Rentee == null)
            {
                MessageBox.Show("Kan ikke finde brugeren, der skal vurderes.");
                return;
            }

            _reviewServices?.SetReviewTarget(rental.Rentee);
            _router?.NavigateTo(Routes.Reviews);
        }

        private static ObservableCollection<Rental> CreateDemoRentals()
        {
            User renter = new User
            {
                Id = 1,
                Username = "camitøs",
                FirstName = "Camilla",
                LastName = "Nielsen"
            };

            User owner = new User
            {
                Id = 2,
                Username = "Sofia M.",
                FirstName = "Sofia",
                LastName = "M."
            };

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
                    renter,
                    owner,
                    article1,
                    new DateOnly(2026, 6, 8),
                    new DateOnly(2026, 6, 12),
                    1006m,
                    DateTime.Now,
                    new ShippingOption(),
                    new InsuranceOption()),

                new Rental(
                    renter,
                    owner,
                    article2,
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