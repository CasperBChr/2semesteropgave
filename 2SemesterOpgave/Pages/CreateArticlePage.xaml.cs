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
using _2SemesterOpgave.Repositories;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Camilla. Interaction logic for CreateArticlePage.xaml.
    /// </summary>
    public partial class CreateArticlePage : UserControl
    {
        ArticleRepository _articleRepository;

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

        public void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image createArticleImage = new Image();         

        }

        //Metode til at sætte en titel på en artikel som oprettes
        public void CreateTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // User input is stored in CreateTitleTextBox.Text
        }
        //Metode til at sætte en beskrivelse på en artikel som oprettes
        public void CreateDescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // User input is stored in CreateDescriptionTextBox.Text
        }
        //Metode til at sætte en pris på en artikel som oprettes
        public void CreatePriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // User input is stored in CreatePriceTextBox.Text
        }
        //Metode til at sætte en farve på en artikel som oprettes
        public void CreateColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // User input is stored in CreateColorTextBox.Text
        }

        //Metode til at gemme den oprettede artikel og navigere tilbage til oversigten
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
                //Article article = new Article();
                //string titel = CreateTitleTextBox.Text;
                //string description = CreateDescriptionTextBox.Text;
                //Category category = (Category)CreateCategoryCombobox.SelectedItem;
                //SubCategory subcategory = (SubCategory)CreateSubcategoryCombobox.SelectedItem;
                //Models.Size size = (Models.Size)CreateSizeComboBox.SelectedItem;
                //double prize = Convert.ToDouble(CreatePriceTextBox.Text);
                //string colorText = CreateColorTextBox.Text;
                //Brand brand = (Brand)CreateBrandComboBox.SelectedItem;

                //if (string.IsNullOrWhiteSpace(titel))
                //{
                //    MessageBox.Show("Husk titel!");
                //    return;
                //}
                                
                //article.Title = titel;
                //article.Description = description;
                //article.Category = category;
                //article.SubCategory = subcategory;
                //article.Size = size;
                //article.DailyPrice = (float)prize;
                //article.Color = colorText;
                //article.Brand = brand;

                //ArticleRepository createArticle = _articleServices.CreateArticle(article);

                //_router.NavigateTo(Routes.Overview);
            
          
        }

        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            //Gemmer ikke den oprettede artikel og navigerer tilbage til oversigten
            _router.NavigateTo(Routes.Overview);
        } 
    }
}
