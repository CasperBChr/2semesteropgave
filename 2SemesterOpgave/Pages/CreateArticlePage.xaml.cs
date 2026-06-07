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
        private ColorServices _colorServices;
        private UserServices _userServices;

        public CreateArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices, SizeServices sizeServices, BrandServices brandServices, ColorServices colorServices, UserServices userServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            _colorServices = colorServices;
            _userServices = userServices;
            CreateCategoryCombobox.ItemsSource = _categoryServices.GetAllCategories();
            CreateSubcategoryCombobox.ItemsSource = _categoryServices.GetAllSubCategories();
            CreateSizeComboBox.ItemsSource = _sizeServices.GetAllSizes();
			CreateColorComboBox.ItemsSource = _colorServices.GetAllColors();
            CreateBrandComboBox.ItemsSource = _brandServices.GetAllBrands();
		}

        public void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image createArticleImage = new Image();         

        }

        //Metode til at gemme den oprettede artikel og navigere tilbage til oversigten
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            Article article = new Article();

            string title = string.Empty;
            if(!string.IsNullOrWhiteSpace(CreateTitleTextBox.Text))
            {
                title = CreateTitleTextBox.Text;
            }
            else{
                MessageBox.Show("Venligst indtast titel");
                return;
            }
            string description = string.Empty;
            if (!string.IsNullOrWhiteSpace(CreateDescriptionTextBox.Text))
            {
                description = CreateDescriptionTextBox.Text;
            }
            else
            {
                MessageBox.Show("Venligst indtast beskrivelse");
                return;
            }
            Category category;
            if (CreateCategoryCombobox.SelectedItem is Category)
            {
                category = (Category)CreateCategoryCombobox.SelectedItem;
			}
            else 
            {
				MessageBox.Show("Venligst vælg kategori");
                return;
			}
            SubCategory subCategory;
            if(CreateSubcategoryCombobox.SelectedItem is SubCategory)
            {
				subCategory = (SubCategory)CreateSubcategoryCombobox.SelectedItem;
			}
            else
            {
				MessageBox.Show("Venligst vælg underkategori");
				return;
			}
			Models.Size size;
			if (CreateSizeComboBox.SelectedItem is Models.Size)
			{
				size = (Models.Size)CreateSizeComboBox.SelectedItem;
			}
			else
			{
				MessageBox.Show("Venligst vælg størrelse");
				return;
			}

            float dailyPrice = 0.0f;
            bool dailyPriceConverted = float.TryParse(CreatePriceTextBox.Text, out dailyPrice);
            if(!dailyPriceConverted) 
            {
				MessageBox.Show("Indtast venligst gyldigt tal");
				return;
            }

            Models.Color color;
            if(CreateColorComboBox.SelectedItem is Models.Color)
            {
                color = (Models.Color)CreateColorComboBox.SelectedItem;
			}
			else
			{
				MessageBox.Show("Venligst vælg farve");
				return;
			}

			Brand brand;
            if(CreateBrandComboBox.SelectedItem is Brand)
            {
                brand = (Brand)CreateBrandComboBox.SelectedItem;
            }
			else
			{
				MessageBox.Show("Venligst vælg mærke");
				return;
			}

            article.Title = title;
            article.Description = description;
            article.Category = category;
            article.SubCategory = subCategory;
            article.Size = size;
            article.DailyPrice = dailyPrice;
            article.Color = color;
            article.Brand = brand;

            _articleServices.CreateArticle(article, _userServices.CurrentUser);

            _router.NavigateTo(Routes.Home);
        }

        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            //Gemmer ikke den oprettede artikel og navigerer tilbage til oversigten
            _router.NavigateTo(Routes.Home);
        }

		private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (CreateCategoryCombobox.SelectedItem == null)
			{
				return;
			}

			Category chosenCategory = (Category)CreateCategoryCombobox.SelectedItem;
			CreateSubcategoryCombobox.ItemsSource = chosenCategory.SubCategories;
		}
	}
}
