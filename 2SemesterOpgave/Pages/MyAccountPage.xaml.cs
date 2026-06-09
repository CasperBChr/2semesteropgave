using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    public partial class MyAccountPage : UserControl
    {
        public User CurrentUser { get; private set; }
        public float UserRating { get; private set; }

        UserServices? _userServices;
        Router? _router;

        public MyAccountPage(Router router, UserServices userServices, ReviewServices reviewServices)
        {
            InitializeComponent();

            _router = router;
            _userServices = userServices;

            CurrentUser = _userServices.GetUserById(_userServices.CurrentUser.Id);
			//CurrentUser = _userServices.CurrentUser;

			UserRating = reviewServices.GetAverageRating(CurrentUser.Id);

            DataContext = this;
            //DataContext = CurrentUser;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userServices.CurrentUser == null)
            {
                MessageBox.Show("User is not available.");
                return;
            }

            try
            {
                _userServices.UpdateUser(CurrentUser);
                MessageBox.Show("Din profil er gemt.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            _router?.NavigateTo(Routes.Reviews);
        }
    }
}