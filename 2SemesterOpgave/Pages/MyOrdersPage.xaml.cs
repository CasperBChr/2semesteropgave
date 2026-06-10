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

        private readonly Router _router;
        RentalServices _rentalServices;
        private readonly ReviewServices? _reviewServices;
        UserServices _userServices;
        public User CurrentUser { get; private set; }

        ArticleServices _articleServices;


		public MyOrdersPage(Router router, UserServices userServices, ReviewServices reviewServices, RentalServices rentalServices, ArticleServices articleServices)
        {
            InitializeComponent();

            _router = router;
            _reviewServices = reviewServices;
			_rentalServices = rentalServices;
			_userServices = userServices;
            _articleServices = articleServices;
			DataContext = this;
            CurrentUser = _userServices.CurrentUser;

            Rentals = _rentalServices.GetByRenter(CurrentUser);
            RentedOut = _rentalServices.GetByRentee(CurrentUser);
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

			_userServices.TargetUser = rental.Rentee;
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Reviews));
			//_router?.NavigateTo(Routes.Reviews);
		}

		private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Rental rental = (Rental)button.DataContext;

			if (rental.Article == null)
			{
				MessageBox.Show("Kan ikke finde artiklen.");
				return;
			}

			_articleServices.SelectedArticle = rental.Article;
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));
			//_router?.NavigateTo(Routes.Article);
		}
	}
}