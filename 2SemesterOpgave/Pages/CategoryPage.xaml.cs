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

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : UserControl
    {
        private ObservableCollection<Category> categories;

        public CategoryPage(ObservableCollection<Category> categories)
        {
            InitializeComponent();
            this.categories = categories;
            CategoriesItemsControl.ItemsSource = categories;
        }
    }
}
