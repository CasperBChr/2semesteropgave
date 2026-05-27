using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for MyAccountPage.xaml
    /// </summary>
    public partial class MyAccountPage : UserControl
    {
        public User CurrentUser { get; private set; }

        public MyAccountPage()
        {
            InitializeComponent();
            CurrentUser = new User();
            DataContext = CurrentUser;
        }

        public MyAccountPage(UserServices userServices)
        {
            InitializeComponent();

            CurrentUser = userServices?.Users?.FirstOrDefault() ?? new User();

            DataContext = CurrentUser;
        }

        public void SetUser(User user)
        {
            if (user == null) return;

            CurrentUser = user;
            DataContext = CurrentUser;
        }
    }
}