using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
	/// 
	public partial class MessagesPage : UserControl
	{
		Router _router;
		UserServices _userServices;
		ConversationServices _conversationServices;
		UnreadBadgeServices _unreadBadgeService;

        //ViewModel designet til at holde visningsdata for MessagesPage
        private List<ConversationListItemViewModel> _viewModels = new List<ConversationListItemViewModel>();

		public MessagesPage(Router router, UserServices userServices, ConversationServices conversationServices, UnreadBadgeServices unreadBadgeServices)
		{
			InitializeComponent();

			_router = router;
			_userServices = userServices;
			_conversationServices = conversationServices;
			_unreadBadgeService = unreadBadgeServices;

			LoadConversations();
		}

        //Metode der indlæser brugerens samtaler og opretter viewmodels til hver samtale
        void LoadConversations()
		{
			ObservableCollection<Conversation> conversations = _conversationServices.GetConversationsForUser(_userServices.CurrentUser);

            //Opretter en viewmodel for hver samtale og tilføjer den til _viewModels listen
            _viewModels = conversations.Select(c => new ConversationListItemViewModel(c, _userServices.CurrentUser, _conversationServices)).ToList();

			ConversationListControl.ItemsSource = _viewModels;
		}

        //Metode der håndterer klik på en samtale og navigerer til ConversationPage
        private void ConversationButton_Click(object sender, RoutedEventArgs e)
		{
			if (((Button)sender).Tag is not ConversationListItemViewModel viewModel) return;

            //Markér samtalen som læst ved at fjerne den fra UnreadBadgeService
            _conversationServices.MarkConversationAsRead(viewModel.Conversation, _userServices.CurrentUser);
			viewModel.NotifyRead();
			_unreadBadgeService.Refresh();
						
			EmptyConversationView.Visibility = Visibility.Collapsed;
			ConversationContent.Content = new EmbeddedMessagePage(_router, _userServices, _conversationServices, _unreadBadgeService, viewModel.Conversation);
		}
	}

    public class ConversationListItemViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public Conversation Conversation { get; }
		public string ParticipantNames { get; }
		public string DisplayInitial { get; }

		public string LastMessagePreview => GetPreview();
		public bool IsUnread => GetIsUnread();

		User _currentUser;
		ConversationServices _conversationServices;

		public ConversationListItemViewModel(Conversation conversation, User currentUser, ConversationServices conversationServices)
		{
			Conversation = conversation;
			_currentUser = currentUser;
			_conversationServices = conversationServices;

			List<User> others = conversation.Participants.Where(p => p.Id != currentUser.Id).ToList();

			ParticipantNames = others.Count > 0 ? string.Join(", ", others.Select(u => u.Username)): "Du selv";

			DisplayInitial = others.Count > 0 && others[0].Username.Length > 0? others[0].Username[0].ToString().ToUpper(): "?";

			conversation.Messages.CollectionChanged += (_, _) =>
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastMessagePreview)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsUnread)));
			};
		}

		private string GetPreview()
		{
			Message? last = Conversation.Messages.LastOrDefault();
			if (last == null)
			{
				return "Ingen beskeder endnu";
			}
			string sender = last.Sender.Id == _currentUser.Id ? "Dig" : last.Sender.Username;
			string text = last.Text.Length > 35 ? last.Text[..35] + "…" : last.Text;
			return $"{sender}: {text}";
		}

		private bool GetIsUnread()
		{
			DateTime lastRead = _conversationServices.GetLastReadTime(Conversation.Id, _currentUser);
			return Conversation.Messages.Any(m => m.Sender.Id != _currentUser.Id && m.Timestamp > lastRead);
		}

		public void NotifyRead()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsUnread)));
		}
	}
}
