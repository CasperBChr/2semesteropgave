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
        RentalServices _rentalServices;
        private readonly ReviewServices? _reviewServices;
        public MyOrdersPage(Router router, UserServices userServices, ReviewServices reviewServices, RentalServices rentalServices)
        {
            InitializeComponent();

            _router = router;
            _reviewServices = reviewServices;
			_rentalServices = rentalServices;

			CurrentUser = userServices?.CurrentUser ?? new User();
            Rentals = _rentalServices.GetAll();
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
    }
}