using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    public partial class ReviewsPage : UserControl
    {
        public User CurrentUser { get; private set; }
        public User TargetUser { get; private set; }
        public ObservableCollection<Review> ReviewsAboutMe { get; private set; }
        public ObservableCollection<Review> ReviewsByMe { get; private set; }

        private readonly ReviewServices _reviewServices;

        public ReviewsPage(ReviewServices reviewServices, UserServices userServices)
        {
            InitializeComponent();

            _reviewServices = reviewServices;
            CurrentUser = userServices?.CurrentUser ?? new User();
			TargetUser = userServices?.TargetUser ?? new User();

            ReviewsAboutMe = new ObservableCollection<Review>();
            ReviewsByMe = new ObservableCollection<Review>();

            LoadReviews();
            DataContext = this;

            FillTargetReviewFields();
        }

        private void LoadReviews()
        {
            ReviewsAboutMe.Clear();

            foreach (Review review in _reviewServices.GetReviewsByRevieweeId(TargetUser.Id))
            {
                ReviewsAboutMe.Add(review);
            }

            ReviewsByMe.Clear();

            foreach (Review review in _reviewServices.GetReviewsByReviewerId(TargetUser.Id))
            {
                ReviewsByMe.Add(review);
            }
        }

        private void FillTargetReviewFields()
        {
            if (_reviewServices.TargetRevieweeId.HasValue)
            {
                RevieweeIdTextBox.Text = _reviewServices.TargetRevieweeId.Value.ToString();
            }

            if (_reviewServices.TargetRentalId.HasValue)
            {
                RentalIdTextBox.Text = _reviewServices.TargetRentalId.Value.ToString();
            }
        }

        private void CreateReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(RevieweeIdTextBox.Text, out int revieweeId))
            {
                MessageBox.Show("Reviewee ID skal være et tal.");
                return;
            }

            if (revieweeId == CurrentUser.Id)
            {
                MessageBox.Show("Du kan ikke vurdere dig selv.");
                return;
            }

            if (!int.TryParse(RatingTextBox.Text, out int rating) || rating < 1 || rating > 5)
            {
                MessageBox.Show("Rating skal være mellem 1 og 5.");
                return;
            }

            int? rentalId = null;

            if (!string.IsNullOrWhiteSpace(RentalIdTextBox.Text))
            {
                if (int.TryParse(RentalIdTextBox.Text, out int parsedRentalId))
                {
                    rentalId = parsedRentalId;
                }
                else
                {
                    MessageBox.Show("Rental ID skal være et tal.");
                    return;
                }
            }

            Review newReview = new Review
            {
                ReviewerId = CurrentUser.Id,
                RevieweeId = revieweeId,
                Rating = rating,
                Comment = CommentTextBox.Text ?? string.Empty,
                RentalId = rentalId,
                CreatedAt = DateTime.Now
            };

            _reviewServices.CreateReview(newReview);
            _reviewServices.ClearReviewTarget();

            LoadReviews();

            RevieweeIdTextBox.Text = string.Empty;
            RentalIdTextBox.Text = string.Empty;
            RatingTextBox.Text = string.Empty;
            CommentTextBox.Text = string.Empty;

            MessageBox.Show("Anmeldelsen er gemt.");
        }
    }
}