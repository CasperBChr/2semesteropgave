using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ArticleServices, UserServices og RentalServices
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article, Rental og ShippingOption
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl, ComboBox og DatePicker
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

    // Klassen er en WPF-side, som arver fra UserControl
    public partial class RentPage : UserControl
    {
        // Router bruges til at navigere mellem sider
        private Router _router;

        // Service der håndterer artikler
        private ArticleServices _articleServices;

        // Service der håndterer brugerdata
        private UserServices _userServices;

        // Service der håndterer udlejninger
        RentalServices _rentalServices;

        // Service der håndterer fragtmuligheder
        private ShippingOptionServices _shippingOptionServices;

        // Service der håndterer forsikringsmuligheder
        private InsuranceOptionServices _insuranceOptionServices;

        // Indeholder den artikel, som brugeren er ved at leje
        private Article? _currentArticle;

        // HashSet med datoer hvor artiklen allerede er booket
        private HashSet<DateOnly> _bookedDates = new HashSet<DateOnly>();

        // Constructor der modtager de services siden skal bruge
        public RentPage(Router router, ArticleServices articleServices, UserServices userServices, ShippingOptionServices shippingOptionServices, InsuranceOptionServices insuranceOptionServices, RentalServices rentalServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer article service, så siden kan hente den valgte artikel
            _articleServices = articleServices;

            // Gemmer user service, så siden kan hente den aktuelle bruger
            _userServices = userServices;

            // Gemmer rental service, så siden kan oprette en lejeaftale og hente bookede datoer
            _rentalServices = rentalServices;

            // Gemmer shipping service, så siden kan hente fragtmuligheder
            _shippingOptionServices = shippingOptionServices;

            // Gemmer insurance service, så siden kan hente forsikringsmuligheder
            _insuranceOptionServices = insuranceOptionServices;

            // Sætter DataContext til den valgte artikel, så XAML kan vise artiklens data
            DataContext = _articleServices.SelectedArticle;

            // Viser navnet på brugeren der lejer artiklen
            RenterName.Text = _userServices.CurrentUser.Username;

            // Viser navnet på ejeren af artiklen
            RenteeName.Text = _articleServices.SelectedArticle.Owner.Username;

            // Gemmer den valgte artikel som den aktuelle artikel
            _currentArticle = _articleServices.SelectedArticle;

            // Fylder fragt-comboboxen med alle fragtmuligheder
            ShippingComboBox.ItemsSource = _shippingOptionServices.GetAll();

            // Fylder forsikrings-comboboxen med alle forsikringsmuligheder
            InsuranceComboBox.ItemsSource = _insuranceOptionServices.GetAll();

            // Henter allerede bookede datoer og blokerer dem i kalenderen
            LoadBookedDates();
        }

        // Metode der kører, når brugeren bekræfter en lejeaftale
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker om både startdato og slutdato er valgt
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                // Viser fejlbesked hvis datoer mangler
                MessageBox.Show("Vælg venligst både start- og slutdato.", "Mangler datoer", MessageBoxButton.OK);

                // Stopper metoden, fordi datoerne mangler
                return;
            }

            // Tjekker om brugeren har valgt fragt
            if (ShippingComboBox.SelectedItem == null)
            {
                // Viser fejlbesked hvis fragt mangler
                MessageBox.Show("Vælg venligst en fragtløsning.", "Mangler fragt", MessageBoxButton.OK);

                // Stopper metoden, fordi fragt mangler
                return;
            }

            // Tjekker om brugeren har valgt forsikring
            if (InsuranceComboBox.SelectedItem == null)
            {
                // Viser fejlbesked hvis forsikring mangler
                MessageBox.Show("Vælg venligst en forsikring.", "Mangler forsikring", MessageBoxButton.OK);

                // Stopper metoden, fordi forsikring mangler
                return;
            }

            // Henter den valgte startdato
            DateTime start = StartDatePicker.SelectedDate.Value;

            // Henter den valgte slutdato
            DateTime end = EndDatePicker.SelectedDate.Value;

            // Tjekker om slutdatoen ikke er efter startdatoen
            if (end <= start)
            {
                // Viser fejlbesked hvis perioden er ugyldig
                MessageBox.Show("Slutdato skal være efter startdato.", "Ugyldig periode", MessageBoxButton.OK);

                // Stopper metoden, fordi datoerne er ugyldige
                return;
            }

            // Dobbelttjek for konflikter
            for (DateTime d = start; d <= end; d = d.AddDays(1))
            {
                // Tjekker om datoen allerede er booket
                if (_bookedDates.Contains(DateOnly.FromDateTime(d)))
                {
                    // Viser fejlbesked hvis perioden indeholder bookede dage
                    MessageBox.Show("Perioden indeholder allerede udlejede dage.", "Konflikt", MessageBoxButton.OK);

                    // Stopper metoden, fordi perioden ikke kan bookes
                    return;
                }
            }

            // Henter den valgte artikel
            Article article = _articleServices.SelectedArticle!;

            // Beregner antal dage i lejeperioden
            int days = (int)(end - start).TotalDays + 1;

            // Henter den valgte fragtmulighed
            ShippingOption shippingOption = (ShippingOption)ShippingComboBox.SelectedItem;

            // Henter den valgte forsikring
            InsuranceOption insuranceOption = (InsuranceOption)InsuranceComboBox.SelectedItem;

            // Opretter en ny lejeaftale
            Rental rental = new Rental
            {
                // Sætter artiklen der skal lejes
                Article = article,

                // Sætter brugeren der lejer artiklen
                Renter = _userServices.CurrentUser,

                // Sætter ejeren/udlejeren af artiklen
                Rentee = article.Owner!,

                // Sætter startdatoen for lejeperioden
                StartDate = DateOnly.FromDateTime(start),

                // Sætter slutdatoen for lejeperioden
                EndDate = DateOnly.FromDateTime(end),

                // Beregner totalprisen ud fra antal dage, dagspris, fragt og forsikring
                TotalPrice = (((decimal)days * (decimal)article.DailyPrice) + (decimal)shippingOption.BaseFee + (decimal)insuranceOption.BaseFees),

                // Gemmer den valgte fragtmulighed
                ShippingChoice = shippingOption,

                // Gemmer den valgte forsikring
                InsuranceChoice = insuranceOption
            };

            // Gemmer lejeaftalen gennem RentalServices
            _rentalServices.CreateRental(rental);

            // Navigerer til mine ordrer og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyOrders));

            //_router.NavigateTo(Routes.MyOrders);
        }

        // Metode der kører, når brugeren annullerer
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til explore-siden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Explore));

            //_router.NavigateTo(Routes.Explore);
        }

        //Metode til at håndtere lejeaftalens datoer og sikre, at de ikke overlapper med eksisterende lejeaftaler
        private void DatePicker_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Tjekker om enten startdato eller slutdato mangler
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                // Skjuler prisfeltet, fordi der ikke kan beregnes pris endnu
                PriceBorder.Visibility = Visibility.Collapsed;

                // Stopper metoden, fordi datoer mangler
                return;
            }

            // Henter den valgte startdato
            DateTime start = StartDatePicker.SelectedDate.Value;

            // Henter den valgte slutdato
            DateTime end = EndDatePicker.SelectedDate.Value;

            // Tjekker om slutdatoen ikke er efter startdatoen
            if (end <= start)
            {
                // Viser besked om at slutdatoen skal være efter startdatoen
                PreviewText.Text = "Slutdato skal være efter startdato";

                // Viser prisfeltet, så brugeren kan se beskeden
                PriceBorder.Visibility = Visibility.Visible;

                // Skjuler advarsel om bookede datoer
                BookedWarningText.Visibility = Visibility.Collapsed;

                // Stopper metoden, fordi perioden er ugyldig
                return;
            }

            // Variabel der holder styr på om perioden indeholder en booket dato
            bool hasConflict = false;

            // Gennemgår hver dag i den valgte periode
            for (DateTime d = start; d <= end; d = d.AddDays(1))
            {
                // Tjekker om datoen allerede er booket
                if (_bookedDates.Contains(DateOnly.FromDateTime(d)))
                {
                    // Markerer at der er en konflikt
                    hasConflict = true;

                    // Stopper løkken, fordi vi allerede har fundet en konflikt
                    break;
                }
            }

            // Tjekker om perioden indeholder bookede datoer
            if (hasConflict)
            {
                // Viser advarsel om at perioden indeholder udlejede dage
                BookedWarningText.Text = "Perioden indeholder allerede udlejede dage";

                // Viser advarslen
                BookedWarningText.Visibility = Visibility.Visible;

                // Tømmer previewteksten
                PreviewText.Text = string.Empty;

                // Viser prisfeltet
                PriceBorder.Visibility = Visibility.Visible;

                // Stopper metoden, fordi perioden ikke kan bookes
                return;
            }

            // Skjuler advarsel om bookede datoer
            BookedWarningText.Visibility = Visibility.Collapsed;

            // Beregner antal dage i perioden
            int days = (int)(end - start).TotalDays + 1;

            // Starter total på 0
            double total = 0;

            // Tjekker om der er valgt en artikel
            if (_articleServices.SelectedArticle != null)
            {
                // Beregner lejeprisen uden fragt og forsikring
                total = days * _articleServices.SelectedArticle.DailyPrice;
            }

            // Opdaterer pris-previewet
            UpdateTotalPreview();
        }

        // Metode der kører, når fragt eller forsikring ændres
        private void Options_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Opdaterer pris-previewet
            UpdateTotalPreview();
        }


        // Opdaterer visningen af totalpris
        private void UpdateTotalPreview()
        {
            // Henter den valgte fragtmulighed
            ShippingOption? shipping = (ShippingOption)ShippingComboBox.SelectedItem;

            // Henter den valgte forsikring
            InsuranceOption? insurance = (InsuranceOption)InsuranceComboBox.SelectedItem;

            // Henter den valgte artikel
            Article? article = _articleServices.SelectedArticle;

            // Tjekker om der ikke er valgt en artikel
            if (article == null)
            {
                // Stopper metoden, fordi der ikke kan beregnes pris uden artikel
                return;
            }

            // Henter startdatoen
            DateTime? start = StartDatePicker.SelectedDate;

            // Henter slutdatoen
            DateTime? end = EndDatePicker.SelectedDate;

            // Starter antal dage på 0
            int days = 0;

            // Tjekker at begge datoer er valgt, og at slutdatoen er efter startdatoen
            if (start != null && end != null && end > start)
            {
                // Beregner antal dage i perioden
                days = (int)(end.Value - start.Value).TotalDays + 1;
            }

            // Beregner pris for selve lejeperioden
            decimal rentTotalPrice = days * (decimal)article.DailyPrice;

            // Starter fragtprisen på 0
            decimal shippingPrice = 0;

            // Starter forsikringsprisen på 0
            decimal insurancePrice = 0;

            // Tjekker om der er valgt fragt
            if (shipping != null)
            {
                // Sætter fragtprisen
                shippingPrice = (decimal)shipping.BaseFee;

                // Viser fragtprisen og navnet på fragten
                ShippingPrice.Text = $"{shippingPrice:F2} kr. ({shipping.Name})";
            }

            // Tjekker om der er valgt forsikring
            if (insurance != null)
            {
                // Sætter forsikringsprisen
                insurancePrice = (decimal)insurance.BaseFees;

                // Viser forsikringsprisen og navnet på forsikringen
                InsurancePrice.Text = $"{insurancePrice:F2} kr. ({insurance.Name})";
            }

            // Beregner samlet pris
            decimal totalPrice = rentTotalPrice + shippingPrice + insurancePrice;

            // Viser lejeprisen eller besked om at vælge datoer
            RentPrice.Text = $"{(days > 0 ? $"{days} dage * {article.DailyPrice:F0} kr./dag = {rentTotalPrice:F0} kr." : "vælg datoer")}";

            // Viser totalprisen
            TotalPrice.Text = $"{totalPrice:F2} kr.";
        }

        // Henter bookede datoer og blokerer dem i kalenderen
        void LoadBookedDates()
        {
            // Henter alle bookede datoer for den valgte artikel
            _bookedDates = _rentalServices.GetBookedDatesForArticle(_articleServices.SelectedArticle.Id);

            // Gennemgår alle bookede datoer
            foreach (DateOnly d in _bookedDates)
            {
                // Konverterer DateOnly til DateTime, så DatePicker kan bruge datoen
                DateTime dt = d.ToDateTime(TimeOnly.MinValue);

                // Blokerer datoen i startdato-kalenderen
                StartDatePicker.BlackoutDates.Add(new CalendarDateRange(dt, dt));

                // Blokerer datoen i slutdato-kalenderen
                EndDatePicker.BlackoutDates.Add(new CalendarDateRange(dt, dt));
            }
        }
    }
}