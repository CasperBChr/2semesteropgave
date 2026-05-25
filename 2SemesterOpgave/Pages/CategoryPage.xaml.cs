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
    /// Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : UserControl
    {
        CategoryServices _services;

        public CategoryPage(CategoryServices categoryService)
        {
            InitializeComponent();
            _services = categoryService;
            CategoriesItemsControl.ItemsSource = _services.GetAllCategories();
        }
    }
}
