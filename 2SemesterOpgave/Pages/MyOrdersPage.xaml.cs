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
using System.Collections.ObjectModel;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for MyOrdersPage.xaml
    /// </summary>
    public partial class MyOrdersPage : UserControl
    {
        public User CurrentUser { get; private set; }
        public ObservableCollection<Rental> Rentals { get; private set; }

        UserServices _userServices;
        ArticleServices _articleServices;
        RentalServices _rentalServices;
        ObservableCollection<Rental> rentals;


		//public MyOrdersPage()
  //      {
  //          InitializeComponent();
  //          CurrentUser = new User();
  //          Rentals = CreateDemoRentals();
  //          DataContext = this;
  //      }

        public MyOrdersPage(UserServices userServices, ArticleServices articlesServices, RentalServices rentalServices)
        {
            InitializeComponent();
            _userServices = userServices;
            _articleServices = articlesServices;
            _rentalServices = rentalServices;
            CurrentUser = userServices?.CurrentUser ?? new User();
            rentals = new ObservableCollection<Rental>(_rentalServices.GetAll());
            Rentals = rentals;
            DataContext = this;
        }
    }
}
