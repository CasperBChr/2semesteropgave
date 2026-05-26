using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
using Microsoft.VisualBasic;
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
using System.Windows.Threading;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for MessagePage.xaml
    /// </summary>
    public partial class MessagePage : UserControl
    {
        UserServices _userServices;
        Conversation _conversation;

        public MessagePage(UserServices userServices)
        {
            InitializeComponent();
            _userServices = userServices;
            ListBoxMessageParticipants.ItemsSource = _userServices.Conversations[0].Participants;

            _conversation = _userServices.Conversations[0];

            MessageItemsControl.ItemsSource = _conversation.Messages;


            _userServices.FakeConversation.OnNewMessage += (conversation) =>
            {
                Dispatcher.Invoke(() =>
                {
                    conversation.Messages.Add(new Message(_userServices.FakeConversation.RandomMessageText(), conversation, conversation.Participants[0], DateTime.Now));
                });
            };
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            _conversation.Messages.Add(new Message(MessageTextBox.Text, _conversation, _conversation.Participants[1], DateTime.Now));
            MessageTextBox.Text = "";
        }
    }
}
