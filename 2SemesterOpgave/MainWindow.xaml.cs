using _2SemesterOpgave.Algoritme; // Giver adgang til algoritme-klasser, fx ItemProfile og UserProfile
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article, User og Category
using _2SemesterOpgave.Repositories; // Giver adgang til repositories
using _2SemesterOpgave.Services; // Giver adgang til services
using _2SemesterOpgave.Utils; // Giver adgang til hjælpeklasser, fx Router og NavigateCommand
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som Window og ContentControl
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media; // Giver adgang til farver og brushes
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace _2SemesterOpgave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    // MainWindow er hovedvinduet i WPF-programmet
    public partial class MainWindow : Window
    {
        // ContentControl bruges til at vise de forskellige sider i programmet
        ContentControl _pageControl;

        // Router bruges til at navigere mellem sider
        Router _router;

        // Liste med artikler som kan bruges af UI'et
        ObservableCollection<Article> _articles = new ObservableCollection<Article>();

        // Repository der håndterer databasekald for artikler
        ArticleRepository _articleRepository;

        // Repository der håndterer databasekald for kategorier
        CategoryRepository _categoryRepository;

        // Repository der håndterer databasekald for brands
        BrandRepository _brandRepository;

        // Repository der håndterer databasekald for farver
        ColorRepository _colorRepository;

        // Repository der håndterer databasekald for størrelser
        SizeRepository _sizeRepository;

        // Repository der håndterer databasekald for lejeaftaler
        RentalRepository _rentalRepository;

        // Repository der håndterer databasekald for fragtmuligheder
        ShippingOptionRepository _shippingOptionRepository;

        // Repository der håndterer databasekald for forsikringsmuligheder
        InsuranceOptionRepository _insuranceOptionRepository;

        // Repository der håndterer databasekald for samtaler
        ConversationRepository _conversationRepository;

        // Repository der håndterer databasekald for reviews
        ReviewRepository _reviewRepository;

        // Service der håndterer brugerlogik
        UserServices _userServices;

        // Service der håndterer artikellogik
        ArticleServices _articleServices;

        // Service der håndterer kategorilogik
        CategoryServices _categoryServices;

        // Service der håndterer brandlogik
        BrandServices _brandServices;

        // Service der håndterer farvelogik
        ColorServices _colorServices;

        // Service der håndterer størrelseslogik
        SizeServices _sizeServices;

        // Service der håndterer lejeaftaler
        RentalServices _rentalServices;

        // Service der håndterer fragtmuligheder
        ShippingOptionServices _shippingOptionServices;

        // Service der håndterer forsikringsmuligheder
        InsuranceOptionServices _insuranceOptionServices;

        // Service der håndterer reviews
        ReviewServices _reviewServices;

        // Service der håndterer login og session
        AuthServices _authServices;

        // Service der håndterer samtaler og beskeder
        ConversationServices _conversationServices;

        // Service der håndterer badge for ulæste beskeder
        UnreadBadgeServices _unreadBadgeService;

        // Filter der bruges til søgning og filtrering af artikler
        FilterCriteria _filter = new FilterCriteria();

        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database, user service og review service
        public MainWindow(IDatabaseFactory db, UserServices userServices, ReviewServices reviewServices)
        {
            // Initialiserer XAML-designet og gør UI-elementerne klar
            InitializeComponent();

            // Gemmer PageContentControl, så routeren kan skifte side i den
            _pageControl = PageContentControl;

            // Gemmer database-factory
            _db = db;

            // Gemmer UserServices
            _userServices = userServices;

            // Opretter repository til kategorier
            _categoryRepository = new CategoryRepository(_db);

            // Opretter repository til brands
            _brandRepository = new BrandRepository(_db);

            // Opretter repository til farver
            _colorRepository = new ColorRepository(_db);

            // Opretter repository til størrelser
            _sizeRepository = new SizeRepository(_db);

            // Opretter repository til artikler
            _articleRepository = new ArticleRepository(_db);

            // Opretter repository til fragtmuligheder
            _shippingOptionRepository = new ShippingOptionRepository(_db);

            // Opretter repository til forsikringsmuligheder
            _insuranceOptionRepository = new InsuranceOptionRepository(_db);

            // Opretter repository til lejeaftaler
            _rentalRepository = new RentalRepository(_db);

            // Opretter repository til samtaler
            _conversationRepository = new ConversationRepository(_db);

            // Opretter repository til reviews
            _reviewRepository = new ReviewRepository(_db);

            // Opretter service til kategorier
            _categoryServices = new CategoryServices(_categoryRepository);

            // Opretter service til brands
            _brandServices = new BrandServices(_brandRepository);

            // Opretter service til farver
            _colorServices = new ColorServices(_colorRepository);

            // Opretter service til størrelser
            _sizeServices = new SizeServices(_sizeRepository);

            // Opretter service til artikler
            _articleServices = new ArticleServices(_articleRepository, _userServices, _brandServices, _categoryServices, _colorServices, _sizeServices);

            // Opretter service til fragtmuligheder
            _shippingOptionServices = new ShippingOptionServices(_shippingOptionRepository);

            // Opretter service til forsikringsmuligheder
            _insuranceOptionServices = new InsuranceOptionServices(_insuranceOptionRepository);

            // Opretter service til lejeaftaler
            _rentalServices = new RentalServices(_rentalRepository, _userServices, _articleServices, _shippingOptionServices, _insuranceOptionServices);

            // Opretter service til samtaler
            _conversationServices = new ConversationServices(_conversationRepository, userServices);

            // Opretter service til unread badge
            _unreadBadgeService = new UnreadBadgeServices(_conversationServices, _userServices);

            // Gemmer ReviewServices
            _reviewServices = reviewServices;

            // Opretter routeren med alle nødvendige services
            _router = new Router(_pageControl, _articles, _categoryServices, _articleServices, _userServices, _rentalServices, _shippingOptionServices, _insuranceOptionServices, _sizeServices, _brandServices, _filter, _colorServices, _reviewServices, _authServices, _conversationServices, _unreadBadgeService);

            // Initialiserer algoritmen for den nuværende bruger
            InitializeAlgorithm(_userServices.CurrentUser);

            // Starter fake bots til samtaler
            _userServices.StartFakeBots(_conversationServices);

            // Lytter efter nye fake beskeder
            _userServices.FakeConversation.OnNewMessage += (conversation) =>
            {
                // Opdaterer unread badge når der kommer en ny besked
                _unreadBadgeService.Refresh();
            };

            // Lytter efter ændringer i antal ulæste beskeder
            _unreadBadgeService.UnreadCountChanged += OnUnreadCountChanged;

            // Opdaterer unread badge første gang
            _unreadBadgeService.Refresh();

            // Navigerer til forsiden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Home));

            //_router.NavigateTo(Routes.Home);
        }

        // Kaldes når antallet af ulæste beskeder ændrer sig
        private void OnUnreadCountChanged(int count)
        {
            // Sørger for at UI-opdateringen sker på UI-tråden
            Dispatcher.Invoke(() =>
            {
                // Tjekker om der er ulæste beskeder
                if (count > 0)
                {
                    // Viser badge
                    MessagesBadgeBorder.Visibility = Visibility.Visible;

                    // Sætter badge-teksten til antal ulæste beskeder
                    MessagesAmountTextBlock.Text = count.ToString();
                }
                else
                {
                    // Skjuler badge hvis der ikke er ulæste beskeder
                    MessagesBadgeBorder.Visibility = Visibility.Collapsed;
                }
            });
        }

        //Algoritme, kodet af Camilla

        // Initialiserer algoritmen for en bruger
        public void InitializeAlgorithm(User user)
        {
            //Liste der henter kategorier
            IEnumerable<Category> categories = _categoryServices.GetAllCategories();

            //Opretter en liste af features baseret på kategorierne, som bruges til at oprette brugerprofilen
            List<string> features = new List<string>();

            // Gennemgår alle kategorier
            foreach (Category category in categories)
            {
                // Tilføjer kategoriens navn som en feature
                features.Add(category.Name);
            }

            //Katalog af elementer
            List<ItemProfile> catalog = new List<ItemProfile>();

            // Henter alle artikler fra ArticleServices
            List<Article> articles = new List<Article>(_articleServices.GetAllArticles());

            // Gennemgår alle artikler
            for (int i = 0; i < articles.Count; i++)
            {
                // Henter artiklen på den aktuelle plads i listen
                Article article = articles[i];

                // Springer artiklen over hvis den ikke har en kategori
                if (article.Category == null)
                {
                    continue;
                }

                // Opretter en ItemProfile til artiklen
                ItemProfile itemProfile = new ItemProfile
                {
                    // Sætter artikelens id
                    ArticleID = article.Id,

                    // Sætter selve artiklen
                    Article = article,

                    // Opretter features for artiklen
                    Features = new Dictionary<string, double>
                        {
							// Bruger artikelens kategori som feature med vægt 1.0
							{ article.Category.Name, 1.0 }
                        }
                };

                // Gemmer itemProfile på artiklen
                article.ItemProfile = itemProfile;

                // Tilføjer itemProfile til kataloget
                catalog.Add(itemProfile);
            }

            //Opret en ny brugerprofil til algoritme
            _userServices.UserProfile = new UserProfile(user.Id.ToString(), features);

            // Sætter brugeren på brugerprofilen
            _userServices.UserProfile.User = _userServices.GetById(user.Id);
        }

        // Navigerer til forsiden når Home-knappen klikkes
        private void HomeMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til Home og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Home));

            //_router.NavigateTo(Routes.Home);
        }

        // Navigerer til mine ordrer når knappen klikkes
        private void MyOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til MyOrders og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyOrders));

            //_router.NavigateTo(Routes.MyOrders);
        }

        // Navigerer til Explore-siden når knappen klikkes
        private void ExplorerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til Explore og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Explore));

            //_router.NavigateTo(Routes.Explore);
        }

        // Navigerer til kategori-siden når knappen klikkes
        private void CategoriMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til Categories og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Categories));

            //_router.NavigateTo(Routes.Categories);
        }

        // Navigerer til favoritsiden når knappen klikkes
        private void FavoritPageButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til Favorites og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Favorites));

            //_router.NavigateTo(Routes.Favorites);
        }

        // Navigerer til min konto når knappen klikkes
        private void MyAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // Sætter TargetUser til den nuværende bruger
            _userServices.TargetUser = _userServices.CurrentUser;

            // Navigerer til MyAccount og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyAccount));

            //_router.NavigateTo(Routes.MyAccount);
        }

        // Navigerer til beskeder når knappen klikkes
        private void MessagesButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til Messages og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Messages));

            //	_router.NavigateTo(Routes.Messages);
        }

        // Navigerer til opret artikel når knappen klikkes
        private void CreateArticleButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til CreateArticle og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.CreateArticle));

            //_router.NavigateTo(Routes.CreateArticle);
        }

        // Navigerer til For You-siden når knappen klikkes
        private void ForYouButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til ForYou og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.ForYou));

            //_router.NavigateTo(Routes.ForYou);
        }

        // Navigerer til mine artikler når knappen klikkes
        private void MyArticlePageButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til MyArticlesPage og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.MyArticlesPage));

            //_router.NavigateTo(Routes.MyArticlesPage);
        }

        // Kaldes når søgefeltet får fokus
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Tjekker om søgefeltet viser placeholder-tekst
            if (SearchBox.Text == "Søg...")
            {
                // Rydder søgefeltet
                SearchBox.Text = string.Empty;

                // Skifter tekstfarven til sort
                SearchBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        // Kaldes når søgefeltet mister fokus
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Tjekker om søgefeltet er tomt
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                // Sætter placeholder-tekst tilbage
                SearchBox.Text = "Søg...";

                // Skifter tekstfarven til grå
                SearchBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        // Kaldes når teksten i søgefeltet ændrer sig
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Stopper hvis teksten kun er placeholder-teksten
            if (SearchBox.Text == "Søg...")
            {
                return;
            }

            // Gemmer søgeteksten i filteret
            _filter.SearchText = SearchBox.Text;

            // Navigerer til overview-siden med søgningen
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Overview));

            //_router.NavigateTo(Routes.Overview);
        }

        // Går tilbage i navigationen når tilbage-knappen klikkes
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Bruger routeren til at gå tilbage
            _router.GoBack();
        }
    }
}