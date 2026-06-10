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
using _2SemesterOpgave.Models;
using System.Collections.ObjectModel;
using _2SemesterOpgave.Services;

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Camilla. Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : UserControl
    {
        private CategoryServices _services;
        private Router _router;
        private ArticleServices _articlesServices;

        public CategoryPage(CategoryServices categoryService, Router router, ArticleServices articleServices)
        {
            InitializeComponent();
            _services = categoryService;
            _router = router;   
            _articlesServices = articleServices;
            CategoriesItemsControl.ItemsSource = _services.GetAllCategories();
        }

        //Metode der navigerer til kategoriens artikler ved klik. 
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Category selectedCategory = (Category)button.DataContext;
            _router.SetSelectedCategory(selectedCategory);
			_router.ExecuteAndRecord(new NavigateCommand(_router, Routes.ArticleSortByCategory));
			//_router.NavigateTo(Routes.ArticleSortByCategory);                
            
        }
    }
}
