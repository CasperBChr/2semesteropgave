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

        public CreateArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
            CreateCategoryCombobox.ItemsSource = _categoryServices.GetAllCategories();
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
        public void CreatePriceTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            string userInput = CreatePriceTextBlock.Text;
            CreatePriceTextBlock.Text = "New Value";  
        }
        //Metode til at sætte en farve på en artikel som oprettes
        public void CreateColorTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            string userInput = CreateColorTextBlock.Text;
            CreateColorTextBlock.Text = "New Value";  
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            //Gemmer ikke den oprettede artikel og navigerer tilbage til oversigten
            _router.NavigateTo(Routes.Overview);
        }   
    }
}
