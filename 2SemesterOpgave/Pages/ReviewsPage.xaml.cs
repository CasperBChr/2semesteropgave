using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl, Button og ComboBoxItem
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User og Review
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ReviewServices, UserServices og Router

namespace _2SemesterOpgave.Pages
{

    /// <summary>
    /// Interaction logic for ReviewsPage.xaml === Kodet af Daniel
    /// </summary>

    // Klassen er en WPF-side, som arver fra UserControl
    public partial class ReviewsPage : UserControl
    {
        // Indeholder den bruger, som er logget ind
        public User CurrentUser { get; private set; }

        // Indeholder den bruger, hvis reviews vi viser
        public User TargetUser { get; private set; }

        // Liste over reviews som TargetUser har modtaget
        public ObservableCollection<Review> ReviewsAboutMe { get; private set; }

        // Liste over reviews som TargetUser har skrevet
        public ObservableCollection<Review> ReviewsByMe { get; private set; }

        // Service der håndterer logik omkring reviews
        ReviewServices _reviewServices;

        // Service der håndterer brugerdata
        UserServices _userServices;

        // Router bruges til at navigere mellem sider
        Router _router;

        // Constructor der modtager de services siden skal bruge
        public ReviewsPage(Router router, ReviewServices reviewServices, UserServices userServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer review service, så siden kan hente og oprette reviews
            _reviewServices = reviewServices;

            // Gemmer user service, så siden kan hente CurrentUser og TargetUser
            _userServices = userServices;

            // Henter den bruger, som er logget ind
            CurrentUser = userServices.CurrentUser;

            // Henter den bruger, som siden viser reviews for
            TargetUser = userServices.TargetUser;

            // Tjekker om brugeren prøver at vurdere sig selv
            if (CurrentUser == TargetUser)
            {
                // Slår gem-knappen fra, så brugeren ikke kan gemme en anmeldelse af sig selv
                SaveReviewButton.IsEnabled = false;
            }

            // Opretter listen til reviews som brugeren har modtaget
            ReviewsAboutMe = new ObservableCollection<Review>();

            // Opretter listen til reviews som brugeren har skrevet
            ReviewsByMe = new ObservableCollection<Review>();

            // Henter reviews og lægger dem i listerne
            LoadReviews();

            // Sætter datakonteksten til siden selv, så XAML kan binde til properties herfra
            DataContext = this;
        }

        // Henter reviews fra ReviewServices og opdaterer listerne
        private void LoadReviews()
        {
            // Tømmer listen, så gamle reviews ikke vises dobbelt
            ReviewsAboutMe.Clear();

            // Gennemgår alle reviews som TargetUser har modtaget
            foreach (Review review in _reviewServices.GetReviewsByRevieweeId(TargetUser.Id))
            {
                // Tilføjer reviewet til listen der vises i UI
                ReviewsAboutMe.Add(review);
            }

            // Tømmer listen, så gamle reviews ikke vises dobbelt
            ReviewsByMe.Clear();

            // Gennemgår alle reviews som TargetUser har skrevet
            foreach (Review review in _reviewServices.GetReviewsByReviewerId(TargetUser.Id))
            {
                // Tilføjer reviewet til listen der vises i UI
                ReviewsByMe.Add(review);
            }
        }

        // Metode der kører, når brugeren klikker på opret review-knappen
        private void CreateReviewButton_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker om brugeren prøver at vurdere sig selv
            if (TargetUser.Id == CurrentUser.Id)
            {
                // Viser fejlbesked
                MessageBox.Show("Du kan ikke vurdere dig selv.");

                // Stopper metoden, fordi man ikke må vurdere sig selv
                return;
            }

            // Tjekker om brugeren ikke har valgt en rating
            if (RatingComboBox.SelectedItem == null)
            {
                // Viser fejlbesked
                MessageBox.Show("Vælg en rating.");

                // Stopper metoden, fordi rating mangler
                return;
            }

            // Henter den valgte rating fra ComboBox og konverterer den til et tal
            int rating = int.Parse(((ComboBoxItem)RatingComboBox.SelectedItem).Content.ToString());

            // Starter med at rental id er null, fordi det ikke nødvendigvis er udfyldt
            int? rentalId = null;

            // Tjekker om der allerede er gemt en rental id i ReviewServices
            if (_reviewServices.TargetRentalId != null)
            {
                // Gemmer rental id, så reviewet kan knyttes til en lejeaftale
                rentalId = _reviewServices.TargetRentalId;
            }

            // Opretter et nyt Review-objekt
            Review newReview = new Review
            {
                // Sætter afsenderen af reviewet til den aktuelle bruger
                ReviewerId = CurrentUser.Id,

                // Sætter modtageren af reviewet til den bruger, der vises på siden
                RevieweeId = TargetUser.Id,

                // Sætter ratingen fra ComboBox
                Rating = rating,

                // Sætter kommentaren, eller tom tekst hvis feltet er null
                Comment = CommentTextBox.Text ?? string.Empty,

                // Sætter rental id, hvis der er et
                RentalId = rentalId,

                // Sætter tidspunktet for hvornår reviewet blev oprettet
                CreatedAt = DateTime.Now
            };

            // Gemmer reviewet via ReviewServices
            _reviewServices.CreateReview(newReview);

            // Nulstiller den valgte review-modtager og rental
            _reviewServices.ClearReviewTarget();

            // Genindlæser reviews, så det nye review kommer med på siden
            LoadReviews();

            // Nulstiller valgt rating i ComboBox
            RatingComboBox.SelectedItem = null;

            // Tømmer kommentar-feltet
            CommentTextBox.Text = string.Empty;

            // Viser besked om at anmeldelsen er gemt
            MessageBox.Show("Anmeldelsen er gemt.");
        }

        // Metode der kører, når brugeren klikker på en reviewer
        private void ReviewerButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter det Review, som knappen hører til
            Review review = (Review)button.DataContext;

            // Sætter brugeren der skrev reviewet som TargetUser
            _userServices.TargetUser = review.Reviewer;

            // Navigerer til brugerprofilen og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.UserProfile));

            //_router.NavigateTo(Routes.UserProfile);
        }

        // Metode der kører, når brugeren klikker på en reviewee
        private void RevieweeButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter det Review, som knappen hører til
            Review review = (Review)button.DataContext;

            // Sætter brugeren der blev vurderet som TargetUser
            _userServices.TargetUser = review.Reviewee;

            // Navigerer til brugerprofilen og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.UserProfile));

            //_router.NavigateTo(Routes.UserProfile);
        }
    }
}