using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Kode af Camilla. Interaction logic for EditArticlePage.xaml
    /// </summary>
    public partial class EditArticlePage : UserControl
    {
        ArticleRepository _articleRepository;

        private Router _router;
		public Article CurrentArticle { get; private set; }

		private ArticleServices _articleServices;
        private CategoryServices _categoryServices;
        private SizeServices _sizeServices;
        private BrandServices _brandServices;
        private UserServices _userServices;
        private ColorServices _colorServices;

       

        public EditArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices, SizeServices sizeServices, BrandServices brandServices, UserServices userServices, ColorServices colorServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            _userServices = userServices;
            _colorServices = colorServices;

            CurrentArticle = _articleServices.SelectedArticle;
            DataContext = CurrentArticle;

			EditCategoryCombobox.ItemsSource = _categoryServices.GetAllCategories();
            EditSubcategoryCombobox.ItemsSource = _categoryServices.GetAllSubCategories();
            EditSizeComboBox.ItemsSource = _sizeServices.GetAllSizes();
			EditBrandComboBox.ItemsSource = _brandServices.GetAllBrands();
			EditColorComboBox.ItemsSource = _colorServices.GetAllColors();


			EditCategoryCombobox.SelectedItem = CurrentArticle.Category;
			EditSubcategoryCombobox.SelectedItem = CurrentArticle.SubCategory;
			EditSizeComboBox.SelectedItem = CurrentArticle.Size;
			EditBrandComboBox.SelectedItem = CurrentArticle.Brand;
			EditColorComboBox.SelectedItem = CurrentArticle.Color;

            Debug.WriteLine($"Size: {CurrentArticle.Size.Name}");

		}

        public void EditImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image editArticleImage = new Image();

        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {
			if (!string.IsNullOrWhiteSpace(EditTitleTextBox.Text))
			{
				CurrentArticle.Title = EditTitleTextBox.Text;
			}
			else
			{
				MessageBox.Show("Venligst indtast titel");
				return;
			}
			if (!string.IsNullOrWhiteSpace(EditDescriptionTextBox.Text))
			{
				CurrentArticle.Description = EditDescriptionTextBox.Text;
			}
			else
			{
				MessageBox.Show("Venligst indtast beskrivelse");
				return;
			}
			if (EditCategoryCombobox.SelectedItem is Category)
			{
				CurrentArticle.Category = (Category)EditCategoryCombobox.SelectedItem;
			}
			else
			{
				MessageBox.Show("Venligst vælg kategori");
				return;
			}
			if (EditSubcategoryCombobox.SelectedItem is SubCategory)
			{
				CurrentArticle.SubCategory = (SubCategory)EditSubcategoryCombobox.SelectedItem;
			}
			else
			{
				MessageBox.Show("Venligst vælg underkategori");
				return;
			}
			if (EditSizeComboBox.SelectedItem is Models.Size)
			{
				CurrentArticle.Size = (Models.Size)EditSizeComboBox.SelectedItem;
			}
			else
			{
				MessageBox.Show("Venligst vælg størrelse");
				return;
			}


			if (EditColorComboBox.SelectedItem is Models.Color)
			{
				CurrentArticle.Color = (Models.Color)EditColorComboBox.SelectedItem;
			}
			else
			{
				MessageBox.Show("Venligst vælg farve");
				return;
			}

			if (EditBrandComboBox.SelectedItem is Brand)
			{
				CurrentArticle.Brand = (Brand)EditBrandComboBox.SelectedItem;
			}
			else
			{
				MessageBox.Show("Venligst vælg mærke");
				return;
			}

			float dailyPrice = 0.0f;
			bool dailyPriceConverted = float.TryParse(EditPriceTextBox.Text, out dailyPrice);
			if (dailyPriceConverted)
			{
				CurrentArticle.DailyPrice = dailyPrice;
			}
			else
			{
				MessageBox.Show("Indtast venligst gyldigt tal");
				return;
			}

            _articleServices.UpdateArticle(CurrentArticle);


			MessageBox.Show("TRIED TO SAVE");
        }

        private void EditDismissButton_Click(object sender, RoutedEventArgs e)
        {
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyArticlesPage));
			//_router.NavigateTo(Routes.MyArticlesPage);
		}

		private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (EditCategoryCombobox.SelectedItem == null)
			{
				return;
			}

			Category chosenCategory = (Category)EditCategoryCombobox.SelectedItem;
			EditSubcategoryCombobox.ItemsSource = chosenCategory.SubCategories;
		}

		private void EditDeleteButton_Click(object sender, RoutedEventArgs e)
		{
			PromptDialogWindow window = new PromptDialogWindow($"Er du sikker på at du ønkser at slette artiklen '{CurrentArticle.Title}'?");
			window.ShowDialog();

			if(window.DialogResult == true) 
			{
				_articleServices.DeleteArticle(CurrentArticle);
				_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyArticlesPage));
				//_router.NavigateTo(Routes.MyArticlesPage);
			}
		}
	}
}
