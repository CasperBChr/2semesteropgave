using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using _2SemesterOpgave.ViewModels;

namespace _2SemesterOpgave.Pages
{
	/// <summary>
	/// Interaction logic for EmbeddedMessagePage.xaml
	/// </summary>
	public partial class EmbeddedMessagePage : UserControl
	{
		readonly UserServices _userServices;
		readonly ConversationServices _conversationServices;
		readonly UnreadBadgeServices _unreadBadgeService;

		ConversationViewModel? _viewModel;

		public EmbeddedMessagePage(Router router, UserServices userServices, ConversationServices conversationServices, UnreadBadgeServices unreadBadgeService, Conversation conversation)
		{
			InitializeComponent();
			_userServices = userServices;
			_conversationServices = conversationServices;
			_unreadBadgeService = unreadBadgeService;

			_viewModel = new ConversationViewModel(conversation, _userServices.CurrentUser);
			MessagesItemsControl.ItemsSource = _viewModel.Messages;
			_viewModel.Messages.CollectionChanged += OnMessagesCollectionChanged;
			
			if(_viewModel.OtherParticipants.Count > 0)
			{
				ParticipantsHeader.Text = string.Join(", ", _viewModel.OtherParticipants.Select(u => u.Username));
			}

			Dispatcher.BeginInvoke(ScrollToBottom);
		}

		void OnMessagesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add) 
			{
				Dispatcher.BeginInvoke(ScrollToBottom);
			}
		}

		void ScrollToBottom() => MessagesScrollViewer.ScrollToEnd();
		private void SendButton_Click(object sender, RoutedEventArgs e) => TrySendMessage();

		private void MessageInputBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
			{
				e.Handled = true;
				TrySendMessage();
			}
		}

		void TrySendMessage()
		{
			string text = MessageInputBox.Text?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(text) || _viewModel == null) return;

			_conversationServices.SendMessage(
				_viewModel.Conversation,
				_userServices.CurrentUser,
				text);

			MessageInputBox.Clear();
		}
	}
}
