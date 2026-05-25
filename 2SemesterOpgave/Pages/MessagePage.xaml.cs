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
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
	/// <summary>
	/// Interaction logic for MessagePage.xaml
	/// </summary>
	public partial class MessagePage : UserControl
	{
		UserServices _userServices;

		public MessagePage(UserServices userServices)
		{
			InitializeComponent();
			_userServices = userServices;
			ListBoxMessageParticipants.ItemsSource = _userServices.Conversations[0].Participants;
		}
	}
}
