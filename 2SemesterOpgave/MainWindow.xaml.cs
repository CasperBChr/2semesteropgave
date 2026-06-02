using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave.Services;
using _2SemesterOpgave.Utils;

namespace _2SemesterOpgave
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ContentControl _pageControl;
		private Router _router;

		ObservableCollection<Article> _articles = new ObservableCollection<Article>();
		ObservableCollection<Category> _categories = new ObservableCollection<Category>();
		ObservableCollection<SubCategory> _subCategories = new ObservableCollection<SubCategory>();
		ObservableCollection<Brand> _brands = new ObservableCollection<Brand>();
		ObservableCollection<Designer> _designers = new ObservableCollection<Designer>();
		ObservableCollection<Collection> _collections = new ObservableCollection<Collection>();
		ObservableCollection<User> _users = new ObservableCollection<User>();
		ObservableCollection<Conversation> _conversations = new ObservableCollection<Conversation>();
		ObservableCollection<Message> _messages = new ObservableCollection<Message>();
		ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();
		//ObservableCollection<Wishlist> _wishlists = new ObservableCollection<Wishlist>(); // Favoritter
		ObservableCollection<Rental> _rentals = new ObservableCollection<Rental>();
		ObservableCollection<ShippingOption> _shippingOptions = new ObservableCollection<ShippingOption>();
		ObservableCollection<InsuranceOption> _insuranceOptions = new ObservableCollection<InsuranceOption>();
		ObservableCollection<Accesibility> _accesibilities = new ObservableCollection<Accesibility>();


		UserRepository _userRepository;
		ArticleRepository _articleRepository;
		CategoryRepository _categoryRepository;
		BrandRepository _brandRepository;
		CollectionRepository _collectionRepository;
		ColorRepository _colorRepository;
		SizeRepository _sizeRepository;
		DesignerRepository _designerRepository;

		UserServices _userServices;
		ArticleServices _articleServices;
		CategoryServices _categoryServices;
		BrandServices _brandServices;
		CollectionServices _collectionServices;
		ColorServices _colorServices;
		SizeServices _sizeServices;
		DesignerServices _designerServices;

		FilterCriteria _filter = new FilterCriteria();

		Database _db;

		public MainWindow()
		{
			InitializeComponent();
			_pageControl = PageContentControl;


			string dbpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "db.db");
			_db = Database.GetInstance($"Data Source={dbpath}");
			//_db = new Database($"Data Source={dbpath}");


			_userRepository = new UserRepository(_db);
			_categoryRepository = new CategoryRepository(_db);
			_brandRepository = new BrandRepository(_db);
			_collectionRepository = new CollectionRepository(_db);
			_colorRepository = new ColorRepository(_db);
			_sizeRepository = new SizeRepository(_db);
			_articleRepository = new ArticleRepository(_db);
			_designerRepository = new DesignerRepository(_db);
			


			_categoryServices = new CategoryServices(_categoryRepository);
			_brandServices = new BrandServices(_brandRepository);
			_userServices = new UserServices(_userRepository);
			_designerServices = new DesignerServices(_designerRepository);
			_collectionServices = new CollectionServices(_collectionRepository, _brandServices, _designerServices);
			_colorServices = new ColorServices(_colorRepository);
			_sizeServices = new SizeServices(_sizeRepository);
			_articleServices = new ArticleServices(_articleRepository, _userServices, _brandServices, _categoryServices, _collectionServices, _colorServices, _sizeServices);

			_router = new Router(_pageControl, _articles, _categoryServices, _articleServices, _userServices, _filter);
			_categories = new ObservableCollection<Category>(_categoryServices.GetAllCategories());

			CategoryCombo.ItemsSource = _categories;
			SubcategoryCombo.ItemsSource = _subCategories;

		}


		private void HomeMenuButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Home);
		}

		private void MyOrderButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.MyOrders);
		}

		private void ExplorerMenuButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Explore);
		}

		private void CategoriMenuButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Categories);
		}

		private void NewsPageButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Announcements);
		}

		private void FavoritPageButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Favorites);
		}

		private void MyAccountButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.MyAccount);
		}

		private void MessagesButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Messages);
		}

		private void CreateArticleButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.CreateArticle);
		}
		private void ForYouButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.ForYou);
		}
		private void NotificationsButton_Click(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.Notifications);
		}

		private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_filter.Category = (Category)CategoryCombo.SelectedItem;
			_filter.SubCategory = null;

			ComboBox comboBox = (ComboBox)sender;
			Category chosenCategory = (Category)comboBox.SelectedItem;

			SubcategoryCombo.ItemsSource = chosenCategory.SubCategories;

			_router.SetFilter(_filter);
			_router.NavigateTo(Routes.Overview);
		}

		private void SubcategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_filter.SubCategory = (SubCategory)SubcategoryCombo.SelectedItem;
			_router.SetFilter(_filter);
			_router.NavigateTo(Routes.Overview);
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_filter.SearchText = ((TextBox)sender).Text;
			_router.SetFilter(_filter);
			_router.NavigateTo(Routes.Overview);
		}

	}
}