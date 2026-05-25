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
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for MessagesPage.xaml
    /// </summary>
    public partial class MessagesPage : UserControl
    {
        Router _router;
        UserServices _userServices;

        public MessagesPage(Router router, UserServices userServices)
        {
            InitializeComponent();

            _router = router;
            _userServices = userServices;

            MessagesItemsControl.ItemsSource = _userServices.Conversations;

		}

		private void MessageButton_Click(object sender, RoutedEventArgs e)
		{
            //_userService
            _router.NavigateTo(Routes.Message);
		}
	}
}
