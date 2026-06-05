using _2SemesterOpgave.Algoritme;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;
using _2SemesterOpgave.Services;
using _2SemesterOpgave.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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


		UserRepository _userRepository;
		ArticleRepository _articleRepository;
		CategoryRepository _categoryRepository;
		BrandRepository _brandRepository;
		CollectionRepository _collectionRepository;
		ColorRepository _colorRepository;
		SizeRepository _sizeRepository;
		DesignerRepository _designerRepository;
		RentalRepository _rentalRepository;
		ShippingOptionRepository _shippingOptionRepository;
		InsuranceOptionRepository _insuranceOptionRepository;

		UserServices _userServices;
		ArticleServices _articleServices;
		CategoryServices _categoryServices;
		BrandServices _brandServices;
		CollectionServices _collectionServices;
		ColorServices _colorServices;
		SizeServices _sizeServices;
		DesignerServices _designerServices;
		RentalServices _rentalServices;
		ShippingOptionServices _shippingOptionServices;
		InsuranceOptionServices _insuranceOptionServices;
		ReviewServices _reviewServices;
        AuthServices _authServices;

        FilterCriteria _filter = new FilterCriteria();

		Database _db;
        UserProfile? UserProfile;

        public MainWindow(Database db, UserServices userServices)
		{
			InitializeComponent();
			_pageControl = PageContentControl;

			_db = db;
			_userServices = userServices;
			
			//_db = new Database($"Data Source={dbpath}");
			_userRepository = new UserRepository(_db);
			_categoryRepository = new CategoryRepository(_db);
			_brandRepository = new BrandRepository(_db);
			_collectionRepository = new CollectionRepository(_db);
			_colorRepository = new ColorRepository(_db);
			_sizeRepository = new SizeRepository(_db);
			_articleRepository = new ArticleRepository(_db);
			_designerRepository = new DesignerRepository(_db);
			_shippingOptionRepository = new ShippingOptionRepository(_db);
			_insuranceOptionRepository = new InsuranceOptionRepository(_db);
			_rentalRepository = new RentalRepository(_db);

			_categoryServices = new CategoryServices(_categoryRepository);
			_brandServices = new BrandServices(_brandRepository);
			//_userServices = new UserServices(_userRepository);
			_designerServices = new DesignerServices(_designerRepository);
			_collectionServices = new CollectionServices(_collectionRepository, _brandServices, _designerServices);
			_colorServices = new ColorServices(_colorRepository);
			_sizeServices = new SizeServices(_sizeRepository);
			_articleServices = new ArticleServices(_articleRepository, _userServices, _brandServices, _categoryServices, _collectionServices, _colorServices, _sizeServices);
			_shippingOptionServices = new ShippingOptionServices(_shippingOptionRepository);
			_insuranceOptionServices = new InsuranceOptionServices(_insuranceOptionRepository);
			_rentalServices = new RentalServices(_rentalRepository, _userServices, _articleServices, _shippingOptionServices, _insuranceOptionServices);
            //_authServices = new AuthServices(_userRepository ,_userServices);

        


            // Services der får _db ??
            _reviewServices = new ReviewServices(_db);

			_router = new Router(_pageControl, _articles, _categoryServices, _articleServices, _userServices, _rentalServices, _shippingOptionServices, _insuranceOptionServices, _sizeServices, _brandServices, _filter, _colorServices, _reviewServices, _userServices.CurrentUser, _authServices);
			_categories = new ObservableCollection<Category>(_categoryServices.GetAllCategories());

			CategoryCombo.ItemsSource = _categories;
			SubcategoryCombo.ItemsSource = _subCategories;
			InitializeAlgorithm(_userServices.CurrentUser);

        }

		//Algoritme
		public void InitializeAlgorithm(User user)
        {
            //Liste der henter kategorier
            IEnumerable<Category> categories = _categoryServices.GetAllCategories();

			//Opretter en liste af features baseret på kategorierne, som bruges til at oprette brugerprofilen
			List<string> features = new List<string>();
            foreach (Category category in categories)
            {
                features.Add(category.Name);
            }

			//Katalog af elementer
			List<ItemProfile> catalog = new List<ItemProfile>();
			List<Article> articles = new List<Article>(_articleServices.GetAllArticles());

			for (int i = 0; i < articles.Count; i++)
			{
				Article article = articles[i];

				if (article.Category == null)
				{
					continue;
				}
				ItemProfile itemProfile = new ItemProfile
				{
					ArticleID = article.Id,
					Article = article,
					Features = new Dictionary<string, double>
						{
							{ article.Category.Name, 1.0 }
						}
				};

				article.ItemProfile = itemProfile;
				catalog.Add(itemProfile);
			}

			//Opret en ny brugerprofil til algoritme
			_userServices.UserProfile = new UserProfile(user.Id.ToString(), features);
            _userServices.UserProfile.User = _userServices.GetById(user.Id);
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
            if (CategoryCombo.SelectedItem == null)
            {
                return;
            }

            _filter.Category = (Category)CategoryCombo.SelectedItem;
            _filter.SubCategory = null;

            Category chosenCategory = (Category)CategoryCombo.SelectedItem;
            SubcategoryCombo.ItemsSource = chosenCategory.SubCategories;

            _router.NavigateTo(Routes.Overview);
        }

        private void SubcategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubcategoryCombo.SelectedItem == null)
            {
                return;
            }

            _filter.SubCategory = (SubCategory)SubcategoryCombo.SelectedItem;

            _router.NavigateTo(Routes.Overview);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filter.SearchText = ((TextBox)sender).Text;

            _router.NavigateTo(Routes.Overview);
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
			_router.NavigateTo(Routes.UserProfile);
        }
    }
}