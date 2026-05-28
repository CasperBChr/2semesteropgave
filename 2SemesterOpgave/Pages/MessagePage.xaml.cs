using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Threading;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
using _2SemesterOpgave.Services.Interfaces;
using _2SemesterOpgave.Utils.Messages;
using _2SemesterOpgave.ViewModels;
using Microsoft.VisualBasic;

namespace _2SemesterOpgave.Pages
{
	/// <summary>
	/// Interaction logic for MessagePage.xaml
	/// </summary>
	public partial class MessagePage : UserControl
	{
		UserServices _userServices;

		IMessageService _messageService;

		Conversation _conversation;

		ConversationViewModel _conversationViewModel;

		Router _router;

		public MessagePage(Router router, UserServices userServices)
		{
			InitializeComponent();

			_router = router;

			_userServices = userServices;

			_messageService = new MessageService();

			_conversation = _userServices.Conversations[0];

			_conversationViewModel = new ConversationViewModel(
				_conversation,
				_userServices.CurrentUser
			);

			DataContext = _conversationViewModel;

			_userServices.FakeConversation.OnNewMessage += (_conversation) =>
			{
				Dispatcher.Invoke(() =>
				{
					_messageService.SendMessage(_conversation, _conversation.Participants[1], _userServices.FakeConversation.RandomMessageText());
				});
			};

			MessageItemsControl.ItemsSource = _conversationViewModel.Messages;
			Debug.Write(MessageItemsControl.ItemsSource);
		}

		private void SendButton_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
			{
				return;
			}

			_messageService.SendMessage(
				_conversation,
				_userServices.CurrentUser,
				MessageTextBox.Text
			);

			MessageTextBox.Text = string.Empty;
		}


		private void UserButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;


			User user = (User)button.DataContext;

			_userServices.TargetUser = user;
			_router.NavigateTo(Routes.UserProfile);

		}
	}

}
