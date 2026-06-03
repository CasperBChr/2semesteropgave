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
    //Kodet af Camilla
    public partial class RentPage : UserControl
    {
        private Router _router;
        private Models.Article _currentArticle;
        private ArticleServices _articleServices;
        private UserServices _userServices;
        public RentPage(Router router, ArticleServices articleServices, UserServices userServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _userServices = userServices;
            
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
