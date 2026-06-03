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
    public partial class CreateArticlePage : UserControl
    {
        private Router _router;
        private Models.Article _currentArticle;
        private ArticleServices _articleServices;
        private CategoryServices _categoryServices;
        private SizeServices _sizeServices;
        private BrandServices _brandServices;

        public CreateArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices, SizeServices sizeServices, BrandServices brandServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            CreateCategoryCombobox.ItemsSource = _categoryServices.GetAllCategories();
            CreateSubcategoryCombobox.ItemsSource = _categoryServices.GetAllSubCategories();
            CreateSizeComboBox.ItemsSource = _sizeServices.GetAllSizes();
            CreateBrandComboBox.ItemsSource = _brandServices.GetAllBrands();
        }
        //Metode til at sætte en titel på en artikel som oprettes
        public void CreateTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string userInput = CreateTitleTextBox.Text;
            CreateTitleTextBox.Text = "New Value";  
        }
        //Metode til at sætte en beskrivelse på en artikel som oprettes
        public void CreateDescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string userInput = CreateDescriptionTextBox.Text;
            CreateDescriptionTextBox.Text = "New Value";  
        }
        //Metode til at sætte en pris på en artikel som oprettes
        public void CreatePriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string userInput = CreatePriceTextBox.Text;
            CreatePriceTextBox.Text = "New Value";  
        }
        //Metode til at sætte en farve på en artikel som oprettes
        public void CreateColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string userInput = CreateColorTextBox.Text;
            CreateColorTextBox.Text = "New Value";  
        }

        //Metode til at gemme den oprettede artikel og navigere tilbage til oversigten
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            //GEM I DATABASE


            _router.NavigateTo(Routes.Overview);
        }

        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            //Gemmer ikke den oprettede artikel og navigerer tilbage til oversigten
            _router.NavigateTo(Routes.Overview);
        }   
    }
}
