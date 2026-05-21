using _2SemesterOpgave.Models;
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

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Interaction logic for OverViewPage.xaml
    /// </summary>
    /// 
    public partial class OverViewPage : UserControl
    {

        ObservableCollection<Article> articles;
        public OverViewPage(ObservableCollection<Article> articles)
        {
            InitializeComponent();
            this.articles = articles;
        }
    }
}
