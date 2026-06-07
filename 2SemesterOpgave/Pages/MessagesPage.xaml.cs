using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using _2SemesterOpgave.Models;
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
		ConversationServices _conversationServices;

		public MessagesPage(Router router, UserServices userServices, ConversationServices conversationServices)
        {
            InitializeComponent();

            _router = router;
            _userServices = userServices;
			_conversationServices = conversationServices;

			ObservableCollection<Conversation> conversations = _conversationServices.GetConversationsForUser(_userServices.CurrentUser);
            MessagesItemsControl.ItemsSource = conversations;
			//MessagesItemsControl.ItemsSource = _userServices.Conversations;

		}

		private void MessageButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Conversation conversation = (Conversation)button.DataContext;
			_conversationServices.TargetConversation = conversation;
			_router.NavigateTo(Routes.Message);
		}
	}
}
