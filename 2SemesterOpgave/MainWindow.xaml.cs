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
using System.Windows.Shapes;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave.Services;

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
        IUserRepository _userRepository;
		CategoryServices _categoryServices;
        UserServices _userServices;
		ArticleServices _articleServices;
		FilterCriteria _filter = new FilterCriteria();

        Database _db;
        public MainWindow()
        {
            InitializeComponent();
            string dbpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "db.db");
            _pageControl = PageContentControl;


            _db = new Database($"Data Source={dbpath}");
            _userRepository = new UserRepository(_db);
			_categoryServices = new CategoryServices(_db);
            _articleServices = new ArticleServices(_db);
            _userServices = new UserServices(_db);
			_router = new Router(_pageControl, _articles, _categoryServices, _articleServices, _userServices, _filter);
            _categories = _categoryServices.GetAllCategories();
            CategoryCombo.ItemsSource = _categories;
            SubcategoryCombo.ItemsSource = _subCategories;

        }


		private void HomeMenuButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Home);
        }

        private void MyOrderButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.MyOrders);
        }

        private void ExplorerMenuButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Explore);
        }

        private void CategoriMenuButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Categories);
        }

        private void NewsPageButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Announcements);
        }

        private void FavoritPageButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Favorites);
        }

        private void MyAccountButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.MyAccount);
        }

        private void MessagesButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Messages);
        }

        private void SupportButtonClick(object sender, RoutedEventArgs e)
        {
            _router.NavigateTo(Routes.Support);
        }
		private void ForYouButtonClick(object sender, RoutedEventArgs e)
		{
			_router.NavigateTo(Routes.ForYou);
		}
		private void NotificationsButtonClick(object sender, RoutedEventArgs e)
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