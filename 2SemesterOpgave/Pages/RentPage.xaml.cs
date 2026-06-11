using _2SemesterOpgave.Services;
using _2SemesterOpgave.Models;
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

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Camilla. Interaction logic for RentPage.xaml.
    /// </summary>
    public partial class RentPage : UserControl
    {
        private Router _router;
        private ArticleServices _articleServices;
        private UserServices _userServices;
        RentalServices _rentalServices;
        private ShippingOptionServices _shippingOptionServices;
        private InsuranceOptionServices _insuranceOptionServices;

		private Article? _currentArticle;
		private HashSet<DateOnly> _bookedDates = new HashSet<DateOnly>();
		public RentPage(Router router, ArticleServices articleServices, UserServices userServices, ShippingOptionServices shippingOptionServices, InsuranceOptionServices insuranceOptionServices, RentalServices rentalServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _userServices = userServices;
            _rentalServices = rentalServices;
            _shippingOptionServices = shippingOptionServices;
            _insuranceOptionServices = insuranceOptionServices;

			DataContext = _articleServices.SelectedArticle;

			RenterName.Text = _userServices.CurrentUser.Username;
			RenteeName.Text = _articleServices.SelectedArticle.Owner.Username;
			
			_currentArticle = _articleServices.SelectedArticle;

			ShippingComboBox.ItemsSource = _shippingOptionServices.GetAll();
            InsuranceComboBox.ItemsSource = _insuranceOptionServices.GetAll();

			LoadBookedDates();
		}

        //Metoder der booker datoer for den valgte artikel
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
			{
				MessageBox.Show("Vælg venligst både start- og slutdato.", "Mangler datoer", MessageBoxButton.OK);
				return;
			}

			if (ShippingComboBox.SelectedItem == null)
			{
				MessageBox.Show("Vælg venligst en fragtløsning.", "Mangler fragt", MessageBoxButton.OK);
				return;
			}

			if (InsuranceComboBox.SelectedItem == null)
			{
				MessageBox.Show("Vælg venligst en forsikring.", "Mangler forsikring", MessageBoxButton.OK);
				return;
			}

            //Opretter en ny lejeaftale baseret på brugerens valg
            DateTime start = StartDatePicker.SelectedDate.Value;
			DateTime end = EndDatePicker.SelectedDate.Value;

			if (end <= start)
			{
				MessageBox.Show("Slutdato skal være efter startdato.", "Ugyldig periode", MessageBoxButton.OK);
				return;
			}

			//Dobbelttjek for konflikter
			for (DateTime d = start; d <= end; d = d.AddDays(1))
			{
				if (_bookedDates.Contains(DateOnly.FromDateTime(d)))
				{
					MessageBox.Show("Perioden indeholder allerede udlejede dage.", "Konflikt", MessageBoxButton.OK);
					return;
				}
			}

            //Opretter lejeaftalen
            Article article = _articleServices.SelectedArticle!;
			int days = (int)(end - start).TotalDays + 1;

			ShippingOption shippingOption = (ShippingOption)ShippingComboBox.SelectedItem;
			InsuranceOption insuranceOption = (InsuranceOption)InsuranceComboBox.SelectedItem;

			Rental rental = new Rental
			{
				Article = article,
				Renter = _userServices.CurrentUser,
				Rentee = article.Owner!,
				StartDate = DateOnly.FromDateTime(start),
				EndDate = DateOnly.FromDateTime(end),
				TotalPrice = (((decimal)days * (decimal)article.DailyPrice) + (decimal)shippingOption.BaseFee + (decimal)insuranceOption.BaseFees),
				ShippingChoice = shippingOption,
				InsuranceChoice = insuranceOption
			};

            //Gemmer lejeaftalen
            _rentalServices.CreateRental(rental);
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyOrders));
			//_router.NavigateTo(Routes.MyOrders);
		}

        //Knap der annullerer lejeprocessen og sender brugeren tilbage til ExplorePage
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Explore));
			//_router.NavigateTo(Routes.Explore);
        }

        //Metode til at håndtere lejeaftalens datoer og sikre, at de ikke overlapper med eksisterende lejeaftaler
        private void DatePicker_Changed(object sender, SelectionChangedEventArgs e)
		{
			if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
			{
				PriceBorder.Visibility = Visibility.Collapsed;
				return;
			}

			DateTime start = StartDatePicker.SelectedDate.Value;
			DateTime end = EndDatePicker.SelectedDate.Value;

			if (end <= start)
			{
				PreviewText.Text = "Slutdato skal være efter startdato";
				PriceBorder.Visibility = Visibility.Visible;
				BookedWarningText.Visibility = Visibility.Collapsed;
				return;
			}

			bool hasConflict = false;
			for (DateTime d = start; d <= end; d = d.AddDays(1))
			{
				if (_bookedDates.Contains(DateOnly.FromDateTime(d)))
				{
					hasConflict = true;
					break;
				}
			}

			if (hasConflict)
			{
				BookedWarningText.Text = "Perioden indeholder allerede udlejede dage";
				BookedWarningText.Visibility = Visibility.Visible;
				PreviewText.Text = string.Empty;
				PriceBorder.Visibility = Visibility.Visible;
				return;
			}

			BookedWarningText.Visibility = Visibility.Collapsed;

			int days = (int)(end - start).TotalDays + 1;
			double total = 0;
			if(_articleServices.SelectedArticle != null) 
			{
				total = days * _articleServices.SelectedArticle.DailyPrice;
			}

			UpdateTotalPreview();
		}
		private void Options_Changed(object sender, SelectionChangedEventArgs e)
		{
			UpdateTotalPreview();
		}

        //Metode der viser den samlede pris for lejeaftalen baseret på de valgte datoer, fragt- og forsikringsmuligheder
        private void UpdateTotalPreview()
		{
			ShippingOption? shipping = (ShippingOption)ShippingComboBox.SelectedItem;
			InsuranceOption? insurance = (InsuranceOption)InsuranceComboBox.SelectedItem;

			Article? article = _articleServices.SelectedArticle;
			if (article == null)
			{
				return;
			}

			DateTime? start = StartDatePicker.SelectedDate;
			DateTime? end = EndDatePicker.SelectedDate;

			int days = 0;
			if(start != null && end != null && end > start)
			{
				days = (int)(end.Value - start.Value).TotalDays + 1;
			}

			decimal rentTotalPrice = days * (decimal)article.DailyPrice;
			decimal shippingPrice = 0;
			decimal insurancePrice = 0;

			if (shipping != null)
			{
				shippingPrice = (decimal)shipping.BaseFee;
				ShippingPrice.Text = $"{shippingPrice:F2} kr. ({shipping.Name})";
			}

			if (insurance != null)
			{
				insurancePrice = (decimal)insurance.BaseFees;
				InsurancePrice.Text = $"{insurancePrice:F2} kr. ({insurance.Name})";
			}

			decimal totalPrice = rentTotalPrice + shippingPrice + insurancePrice;

			RentPrice.Text = $"{(days > 0 ? $"{days} dage * {article.DailyPrice:F0} kr./dag = {rentTotalPrice:F0} kr." : "vælg datoer")}";
			TotalPrice.Text = $"{totalPrice:F2} kr.";
		}

        //Metode der indlæser allerede bookede datoer for den valgte artikel
        void LoadBookedDates()
		{
			_bookedDates = _rentalServices.GetBookedDatesForArticle(_articleServices.SelectedArticle.Id);

			foreach (DateOnly d in _bookedDates)
			{
				DateTime dt = d.ToDateTime(TimeOnly.MinValue);
				StartDatePicker.BlackoutDates.Add(new CalendarDateRange(dt, dt));
				EndDatePicker.BlackoutDates.Add(new CalendarDateRange(dt, dt));
			}
		}
	}
}
