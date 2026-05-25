using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
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
    /// Interaction logic for OverviewPage.xaml
    /// </summary>
    /// 
    public partial class OverviewPage : UserControl
    {

		ArticleServices _articleServices;
		FilterCriteria _filter;
		Router _router;

		public OverviewPage(Router router, ArticleServices articleServices, FilterCriteria filter)
		{
			InitializeComponent();
			_router = router;
			_articleServices = articleServices;
			_filter = filter;

			LoadArticles();
		}

		private void LoadArticles()
		{
			var articles = _articleServices.GetFilteredArticles(_filter);
			ArticlesItemsControl.ItemsSource = articles;
		}

		private void FilterChanged(object sender, EventArgs e)
		{
			if (ColorFilter.SelectedItem is ComboBoxItem colorItem && colorItem.Content.ToString() != "Alle")
			{
				_filter.Color = colorItem.Content.ToString();
			}
			else
			{
				_filter.Color = null;
			}

			if (SizeFilter.SelectedItem is ComboBoxItem sizeItem && sizeItem.Content.ToString() != "Alle")
			{
				_filter.Size = sizeItem.Content.ToString();
			}
			else
			{
				_filter.Size = null;
			}

			if (float.TryParse(MinPriceBox.Text, out float min))
			{
				_filter.MinPrice = min;
			}
			else 
			{
				_filter.MinPrice = null;
			}

			if (float.TryParse(MaxPriceBox.Text, out float max))
			{
				_filter.MaxPrice = max;
			}
			else
			{
				_filter.MaxPrice = null;
			}

			LoadArticles();
		}

		private void ArticleButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Article);
		}
	}
}
