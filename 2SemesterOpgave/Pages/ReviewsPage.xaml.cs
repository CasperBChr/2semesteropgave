using System; 
using System.Collections.ObjectModel; 
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User og Review
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ReviewServices og UserServices

namespace _2SemesterOpgave.Pages
{
    public partial class ReviewsPage : UserControl
    {
        public User CurrentUser { get; private set; } // Indeholder den bruger, som er logget ind
        public User TargetUser { get; private set; } // Indeholder den bruger, hvis reviews vi viser
        public ObservableCollection<Review> ReviewsAboutMe { get; private set; } // Liste over reviews som TargetUser har modtaget
        public ObservableCollection<Review> ReviewsByMe { get; private set; } // Liste over reviews som TargetUser har skrevet

        private readonly ReviewServices _reviewServices; // Service der håndterer logik omkring reviews

        public ReviewsPage(ReviewServices reviewServices, UserServices userServices) // Constructor der modtager review service og user service
        {
            InitializeComponent(); // Initialiserer XAML-designet og gør sidens UI-elementer klar

            _reviewServices = reviewServices; // Gemmer review service, så siden kan hente og oprette reviews
            CurrentUser = userServices?.CurrentUser ?? new User(); // Henter den aktuelle bruger, eller laver en tom bruger hvis den ikke findes
            TargetUser = userServices?.TargetUser ?? new User(); // Henter den valgte bruger, eller laver en tom bruger hvis den ikke findes

            ReviewsAboutMe = new ObservableCollection<Review>(); // Opretter listen til reviews som brugeren har modtaget
            ReviewsByMe = new ObservableCollection<Review>(); // Opretter listen til reviews som brugeren har skrevet

            LoadReviews(); // Henter reviews og lægger dem i listerne
            DataContext = this; // Sætter datakonteksten til siden selv, så XAML kan binde til properties herfra

            FillTargetReviewFields(); // Udfylder felter automatisk, hvis der allerede er valgt en bruger/lejeaftale til review
        }

        private void LoadReviews() // Henter reviews fra ReviewServices og opdaterer listerne
        {
            ReviewsAboutMe.Clear(); // Tømmer listen, så gamle reviews ikke vises dobbelt

            foreach (Review review in _reviewServices.GetReviewsByRevieweeId(TargetUser.Id)) // Gennemgår alle reviews som TargetUser har modtaget
            {
                ReviewsAboutMe.Add(review); // Tilføjer reviewet til listen der vises i UI
            }

            ReviewsByMe.Clear(); // Tømmer listen, så gamle reviews ikke vises dobbelt

            foreach (Review review in _reviewServices.GetReviewsByReviewerId(TargetUser.Id)) // Gennemgår alle reviews som TargetUser har skrevet
            {
                ReviewsByMe.Add(review); // Tilføjer reviewet til listen der vises i UI
            }
        }

        private void FillTargetReviewFields() // Udfylder review-felterne hvis der er valgt en bruger eller rental på forhånd
        {
            if (_reviewServices.TargetRevieweeId.HasValue) // Tjekker om der er valgt en bruger, der skal vurderes
            {
                RevieweeIdTextBox.Text = _reviewServices.TargetRevieweeId.Value.ToString(); // Sætter brugerens id ind i tekstfeltet
            }

            if (_reviewServices.TargetRentalId.HasValue) // Tjekker om der er valgt en lejeaftale til reviewet
            {
                RentalIdTextBox.Text = _reviewServices.TargetRentalId.Value.ToString(); // Sætter rental id ind i tekstfeltet
            }
        }

        private void CreateReviewButton_Click(object sender, RoutedEventArgs e) // Metode der kører, når brugeren klikker på opret review-knappen
        {
            if (!int.TryParse(RevieweeIdTextBox.Text, out int revieweeId)) // Tjekker om RevieweeIdTextBox indeholder et tal
            {
                MessageBox.Show("Reviewee ID skal være et tal."); // Viser fejlbesked hvis id ikke er et tal
                return; // Stopper metoden, fordi reviewee id er ugyldigt
            }

            if (revieweeId == CurrentUser.Id) // Tjekker om brugeren prøver at vurdere sig selv
            {
                MessageBox.Show("Du kan ikke vurdere dig selv."); // Viser fejlbesked
                return; // Stopper metoden, fordi man ikke må vurdere sig selv
            }

            if (!int.TryParse(RatingTextBox.Text, out int rating) || rating < 1 || rating > 5) // Tjekker om rating er et tal mellem 1 og 5
            {
                MessageBox.Show("Rating skal være mellem 1 og 5."); // Viser fejlbesked hvis rating er ugyldig
                return; // Stopper metoden, fordi rating ikke er korrekt
            }

            int? rentalId = null; // Starter med at rental id er null, fordi det ikke nødvendigvis er udfyldt

            if (!string.IsNullOrWhiteSpace(RentalIdTextBox.Text)) // Tjekker om rental id-feltet ikke er tomt
            {
                if (int.TryParse(RentalIdTextBox.Text, out int parsedRentalId)) // Prøver at konvertere rental id til et tal
                {
                    rentalId = parsedRentalId; // Gemmer rental id hvis konverteringen lykkes
                }
                else // Hvis rental id ikke kan konverteres til et tal
                {
                    MessageBox.Show("Rental ID skal være et tal."); // Viser fejlbesked
                    return; // Stopper metoden, fordi rental id er ugyldigt
                }
            }

            Review newReview = new Review // Opretter et nyt Review-objekt
            {
                ReviewerId = CurrentUser.Id, // Sætter afsenderen af reviewet til den aktuelle bruger
                RevieweeId = revieweeId, // Sætter modtageren af reviewet
                Rating = rating, // Sætter ratingen fra tekstfeltet
                Comment = CommentTextBox.Text ?? string.Empty, // Sætter kommentaren, eller tom tekst hvis feltet er null
                RentalId = rentalId, // Sætter rental id, hvis der er et
                CreatedAt = DateTime.Now // Sætter tidspunktet for hvornår reviewet blev oprettet
            };

            _reviewServices.CreateReview(newReview); // Gemmer reviewet via ReviewServices
            _reviewServices.ClearReviewTarget(); // Nulstiller den valgte review-modtager og rental

            LoadReviews(); // Genindlæser reviews, så det nye review kommer med på siden

            RevieweeIdTextBox.Text = string.Empty; // Tømmer reviewee id-feltet
            RentalIdTextBox.Text = string.Empty; // Tømmer rental id-feltet
            RatingTextBox.Text = string.Empty; // Tømmer rating-feltet
            CommentTextBox.Text = string.Empty; // Tømmer kommentar-feltet

            MessageBox.Show("Anmeldelsen er gemt."); // Viser besked om at anmeldelsen er gemt
        }
    }
}