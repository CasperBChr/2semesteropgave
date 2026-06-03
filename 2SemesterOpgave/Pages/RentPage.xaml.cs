using _2SemesterOpgave.Services;
using _2SemesterOpgave.Models;
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

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Camilla. Interaction logic for RentPage.xaml.
    /// </summary>
    public partial class RentPage : UserControl
    {
        private Router _router;
        private Models.Article _currentArticle;
        private ArticleServices _articleServices;
        private UserServices _userServices;
        private ShippingOptionServices _shippingOptionServices;
        private InsuranceOptionServices _insuranceOptionServices;
        public RentPage(Router router, ArticleServices articleServices, UserServices userServices, ShippingOptionServices shippingOptionServices, InsuranceOptionServices insuranceOptionServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _userServices = userServices;
            _shippingOptionServices = shippingOptionServices;
            _insuranceOptionServices = insuranceOptionServices;
            ShippingComboBox.ItemsSource = _shippingOptionServices.GetAll();
            InsuranceComboBox.ItemsSource = _insuranceOptionServices.GetAll();
        }
        //Knap til at bekræfte leje af en artikel, og navigere derefter tilbage til oversigten
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Tillykke! Din leje er bekræftet.", "Bekræft leje", MessageBoxButton.OK);

            //Navigerer tilbage til oversigten
            if (result == MessageBoxResult.OK)
            {
                _router.NavigateTo(Routes.Overview);
            }
        }
        //Knap til at annuller leje og navigerer derefter tilbage til oversigten
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på, at du vil annullere lejen?", "Anuller leje", MessageBoxButton.YesNo);

            //Navigerer tilbage til oversigten
            if (result == MessageBoxResult.Yes)
            {
                _router.NavigateTo(Routes.Overview);
            }
        }
    }
}
