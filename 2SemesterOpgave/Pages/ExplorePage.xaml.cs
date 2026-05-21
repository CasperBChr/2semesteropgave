using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for ExplorePage.xaml
    /// </summary>
    public partial class ExplorePage : UserControl
    {

        ObservableCollection<Article> articles;

        public ExplorePage(ObservableCollection<Article> articles)
        {
            InitializeComponent();
            this.articles = articles;


            ArticlesItemsControl.ItemsSource = articles;


        }
    }
}
