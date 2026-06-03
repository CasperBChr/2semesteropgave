using System.Windows;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    public partial class UserPage : UserControl
    {
        private readonly Router _router;
        private readonly UserServices _userServices;
        private readonly ReviewServices _reviewServices;

        public User TargetUser { get; private set; }

        public UserPage(Router router, UserServices userServices, ReviewServices reviewServices)
        {
            InitializeComponent();

            _router = router;
            _userServices = userServices;
            _reviewServices = reviewServices;

            TargetUser = _userServices.TargetUser ?? new User();

            DataContext = TargetUser;
        }

        private void ReviewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (TargetUser.Id == 0)
            {
                MessageBox.Show("Kan ikke finde brugeren.");
                return;
            }

            if (TargetUser.Id == _userServices.CurrentUser.Id)
            {
                MessageBox.Show("Du kan ikke vurdere dig selv.");
                return;
            }

            _reviewServices.SetReviewTarget(TargetUser);
            _router.NavigateTo(Routes.Reviews);
        }
    }
}