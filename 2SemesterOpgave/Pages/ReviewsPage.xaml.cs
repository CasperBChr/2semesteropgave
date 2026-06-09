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

        ReviewServices _reviewServices;
        UserServices _userServices;
        Router _router;

        public ReviewsPage(Router router, ReviewServices reviewServices, UserServices userServices)
        {
            InitializeComponent();
            _router = router;
            _reviewServices = reviewServices;
            _userServices = userServices;
            CurrentUser = userServices.CurrentUser;
			TargetUser = userServices.TargetUser;

            if(CurrentUser == TargetUser)
            {
				SaveReviewButton.IsEnabled = false;
			}

            ReviewsAboutMe = new ObservableCollection<Review>();
            ReviewsByMe = new ObservableCollection<Review>();

            LoadReviews();
            DataContext = this;
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

        private void CreateReviewButton_Click(object sender, RoutedEventArgs e)
        {

            if (TargetUser.Id == CurrentUser.Id)
            {
                MessageBox.Show("Du kan ikke vurdere dig selv.");
                return;
            }

			if (RatingComboBox.SelectedItem == null)
			{
				MessageBox.Show("Vælg en rating.");
				return;
			}

			int rating = int.Parse(((ComboBoxItem)RatingComboBox.SelectedItem).Content.ToString());

			int? rentalId = null;

            if(_reviewServices.TargetRentalId != null) 
            {
                rentalId = _reviewServices.TargetRentalId;
			}

            Review newReview = new Review
            {
                ReviewerId = CurrentUser.Id,
                RevieweeId = TargetUser.Id,
                Rating = rating,
                Comment = CommentTextBox.Text ?? string.Empty,
                RentalId = rentalId,
                CreatedAt = DateTime.Now
            };

            _reviewServices.CreateReview(newReview);
            _reviewServices.ClearReviewTarget();

            LoadReviews();

			RatingComboBox.SelectedItem = null;
			CommentTextBox.Text = string.Empty;

            MessageBox.Show("Anmeldelsen er gemt.");
        }

		private void ReviewerButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Review review = (Review)button.DataContext;

			_userServices.TargetUser = review.Reviewer;
			_router.NavigateTo(Routes.UserProfile);
		}

		private void RevieweeButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Review review = (Review)button.DataContext;

			_userServices.TargetUser = review.Reviewee;
			_router.NavigateTo(Routes.UserProfile);
		}
	}
}