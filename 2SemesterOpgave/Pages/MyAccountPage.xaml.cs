using System;
using System.Windows;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    public partial class MyAccountPage : UserControl
    {
        public User CurrentUser { get; private set; }

        private readonly UserServices? _userServices;
        private readonly Router? _router;

        public MyAccountPage()
        {
            InitializeComponent();

            CurrentUser = new User();
            DataContext = CurrentUser;
        }

        public MyAccountPage(Router router, UserServices userServices)
        {
            InitializeComponent();

            _router = router;
            _userServices = userServices;

            CurrentUser = _userServices.GetUserById(_userServices.CurrentUser.Id) ?? new User();

            DataContext = CurrentUser;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userServices == null)
            {
                MessageBox.Show("User service is not available.");
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