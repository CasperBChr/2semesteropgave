using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    public partial class MyOrdersPage : UserControl
    {
        //public User CurrentUser { get; private set; }
        public ObservableCollection<Rental> Rentals { get; private set; }
        public ObservableCollection<Rental> RentedOut { get; private set; }

        private readonly Router? _router;
        RentalServices _rentalServices;
        private readonly ReviewServices? _reviewServices;
        UserServices _userServices;
        public User CurrentUser { get; private set; }


		public MyOrdersPage(Router router, UserServices userServices, ReviewServices reviewServices, RentalServices rentalServices)
        {
            InitializeComponent();

            _router = router;
            _reviewServices = reviewServices;
			_rentalServices = rentalServices;
			_userServices = userServices;
			DataContext = this;
            CurrentUser = _userServices.CurrentUser;
            //CurrentUser = _userServices.GetAllUsers()[1];

            //CurrentUser = userServices?.CurrentUser;
            Rentals = _rentalServices.GetByRenter(CurrentUser);
            RentedOut = _rentalServices.GetByRentee(CurrentUser);
			foreach (var rental in Rentals)
			{
				Debug.WriteLine($"Rental ID: {rental.Id}");
				Debug.WriteLine($"Artikel navn: {rental.Article?.Title}");
				Debug.WriteLine($"Beskrivelse: {rental.Article?.Description}");
				Debug.WriteLine($"Kategori: {rental.Article?.Category.Name}");
				Debug.WriteLine($"Underkategori: {rental.Article?.SubCategory.Name}");
				Debug.WriteLine($"Brand: {rental.Article?.Brand?.Name}");
				Debug.WriteLine($"Udlejer: {rental.Renter?.Username}");
				Debug.WriteLine($"Lejer: {rental.Rentee?.Username}");
				Debug.WriteLine($"Startdato: {rental.StartDate:dd.MM.yyyy}");
				Debug.WriteLine($"Slutdato: {rental.EndDate:dd.MM.yyyy}");
				Debug.WriteLine($"Pris: {rental.TotalPrice} kr.");
				Debug.WriteLine("--------------------------------");
			}
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