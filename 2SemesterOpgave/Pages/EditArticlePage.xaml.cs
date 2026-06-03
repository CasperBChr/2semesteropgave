using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Services;
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
    /// Kode af Camilla. Interaction logic for EditArticlePage.xaml
    /// </summary>
    public partial class EditArticlePage : UserControl
    {
        ArticleRepository _articleRepository;

        private Router _router;
        private Models.Article _currentArticle;
        private ArticleServices _articleServices;
        private CategoryServices _categoryServices;
        private SizeServices _sizeServices;
        private BrandServices _brandServices;

        public EditArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices, SizeServices sizeServices, BrandServices brandServices)
        {
            InitializeComponent();
            _router = router;
            _articleServices = articleServices;
            _categoryServices = categoryServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            EditCategoryCombobox.ItemsSource = _categoryServices.GetAllCategories();
            EditSubcategoryCombobox.ItemsSource = _categoryServices.GetAllSubCategories();
            EditSizeComboBox.ItemsSource = _sizeServices.GetAllSizes();
            EditBrandComboBox.ItemsSource = _brandServices.GetAllBrands();
        }

        public void EditImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image editArticleImage = new Image();

        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditDismissButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
