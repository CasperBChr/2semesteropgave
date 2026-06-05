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

		MessageService _messageService;
		Conversation _conversation;
		ConversationViewModel _conversationViewModel;

		UnreadBadgeServices _unreadBadgeService;
		ConversationServices _conversationServices;

		Router _router;

		Action<Conversation> _onNewMessageHandler;

		public MessagePage(Router router, UserServices userServices, ConversationServices conversationServices, UnreadBadgeServices unreadBadgeService)
		{
			InitializeComponent();
			_router = router;
			_userServices = userServices;
			_conversationServices = conversationServices;
			_unreadBadgeService = unreadBadgeService;
			_messageService = new MessageService();

			_conversation = _userServices.Conversations[0];

			_conversationViewModel = new ConversationViewModel(_conversation, _userServices.CurrentUser);
			DataContext = _conversationViewModel;

			_conversationServices.MarkConversationAsRead(_conversation, _userServices.CurrentUser);
			_unreadBadgeService.Refresh();

			_onNewMessageHandler = (conv) =>
			{
				Dispatcher.Invoke((Delegate)(() =>
				{
					_messageService.SendMessage(conv, conv.Participants[1], _userServices.FakeConversation.RandomMessageText());
					_conversationServices.MarkConversationAsRead(_conversation, _userServices.CurrentUser);
					_unreadBadgeService.Refresh();
				}));
			};

			_userServices.FakeConversation.OnNewMessage += _onNewMessageHandler;

			Unloaded += (s, e) =>
			{
				_userServices.FakeConversation.OnNewMessage -= _onNewMessageHandler;
				_conversationServices.MarkConversationAsRead(_conversation, _userServices.CurrentUser);
				_unreadBadgeService.Refresh();
			};

			MessageItemsControl.ItemsSource = _conversationViewModel.Messages;
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


		private void UserProfileButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			User user = (User)button.DataContext;
			_userServices.TargetUser = user;
			_router.NavigateTo(Routes.UserProfile);
		}
	}

}
