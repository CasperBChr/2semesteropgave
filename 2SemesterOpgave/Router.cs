using System.Collections.ObjectModel;
using System.Windows.Controls; // Giver adgang til WPF controls, fx ContentControl
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article, Rental og Category
using _2SemesterOpgave.Pages; // Giver adgang til alle siderne i programmet
using _2SemesterOpgave.Services; // Giver adgang til serviceklasserne

namespace _2SemesterOpgave
{
    // Router-klassen håndterer navigation mellem sider i programmet
    public class Router
    {
        // Gemmer hvilken side der er aktiv lige nu
        Routes _currentPage;

        // Property der returnerer den aktuelle side
        public Routes CurrentPage { get { return _currentPage; } } // Property: gemmer den aktuelle side, som standard er sat til Home

        // ContentControl bruges til at vise den aktuelle side i MainWindow
        ContentControl _pageControl;

        // Gemmer den valgte kategori, fx når man sorterer artikler efter kategori
        Category _selectedCategory;

        // Liste med artikler som kan bruges på siderne
        ObservableCollection<Article> _articles;

        // Liste med lejeaftaler
        ObservableCollection<Rental> _rentals;

        // Service der håndterer kategorier
        CategoryServices _categoryServices;

        // Service der håndterer artikler
        ArticleServices _articleServices;

        // Service der håndterer brugere
        UserServices _userServices;

        // Service der håndterer lejeaftaler
        RentalServices _rentalServices;

        // Service der håndterer fragtmuligheder
        ShippingOptionServices _shippingOptionServices;

        // Service der håndterer forsikringsmuligheder
        InsuranceOptionServices _insuranceOptionServices;

        // Service der håndterer størrelser
        SizeServices _sizeServices;

        // Gemmer det aktuelle filter til artikler
        FilterCriteria _currentFilter;

        // Service der håndterer brands
        BrandServices _brandServices;

        // Service der håndterer farver
        ColorServices _colorServices;

        // Service der håndterer reviews
        ReviewServices _reviewServices;

        // Service der håndterer login og session
        AuthServices _authServices;

        // Service der håndterer samtaler og beskeder
        ConversationServices _conversationServices;

        // Service der håndterer badge for ulæste beskeder
        UnreadBadgeServices _unreadBadgeService;

        // Liste af Commands (Command Pattern) så vi kan huske tidligere navigationer og gå tilbage
        Stack<ICommand> _history = new Stack<ICommand>();

        // Constructor der modtager alle services og data, som routeren skal bruge
        public Router(ContentControl pageControl, ObservableCollection<Article> articles, CategoryServices categoryServices, ArticleServices articleServices, UserServices userServices, RentalServices rentalServices, ShippingOptionServices shippingOptionServices, InsuranceOptionServices insuranceOptionServices, SizeServices sizeServices, BrandServices brandServices, FilterCriteria filterCriteria, ColorServices colorServices, ReviewServices reviewServices, AuthServices authServices, ConversationServices conversationServices, UnreadBadgeServices unreadBadgeServices)
        {
            // Sætter startsiden til Home
            _currentPage = Routes.Home;

            // Gemmer ContentControl, så routeren kan skifte indholdet
            _pageControl = pageControl;

            // Gemmer artikellisten
            _articles = articles;

            // Gemmer CategoryServices
            _categoryServices = categoryServices;

            // Gemmer ArticleServices
            _articleServices = articleServices;

            // Gemmer ReviewServices
            _reviewServices = reviewServices;

            // Gemmer UserServices
            _userServices = userServices;

            // Gemmer ColorServices
            _colorServices = colorServices;

            // Gemmer SizeServices
            _sizeServices = sizeServices;

            // Gemmer BrandServices
            _brandServices = brandServices;

            // Gemmer ShippingOptionServices
            _shippingOptionServices = shippingOptionServices;

            // Gemmer InsuranceOptionServices
            _insuranceOptionServices = insuranceOptionServices;

            // Gemmer RentalServices
            _rentalServices = rentalServices;

            // Gemmer det aktuelle filter
            _currentFilter = filterCriteria;

            // Gemmer AuthServices
            _authServices = authServices;

            // Gemmer ConversationServices
            _conversationServices = conversationServices;

            // Gemmer UnreadBadgeServices
            _unreadBadgeService = unreadBadgeServices;

            // Henter alle lejeaftaler og gemmer dem
            _rentals = _rentalServices.GetAll();
        }

        // Udfører en kommando og gemmer den i historikken
        public void ExecuteAndRecord(ICommand command)
        {
            // Gemmer kommandoen i historikken
            _history.Push(command);

            // Udfører kommandoen
            command.Execute();
        }

        // Går tilbage til forrige side
        public void GoBack()
        {
            // Tjekker om der er mere end én side i historikken
            if (_history.Count > 1)
            {
                // Fjerner den nuværende side fra historikken
                _history.Pop();

                // Udfører den forrige kommando igen
                _history.Peek().Execute();
            }
        }

        // Sætter det aktuelle filter
        public void SetFilter(FilterCriteria filter)
        {
            // Gemmer filteret
            _currentFilter = filter;
        }

        // Sætter den valgte kategori
        public void SetSelectedCategory(Category category)
        {
            // Gemmer kategorien, så den kan bruges ved navigation
            _selectedCategory = category;
        }

        // Navigerer til en bestemt side ud fra route
        public void NavigateTo(Routes route)
        {
            // Tjekker hvilken route der skal navigeres til
            switch (route)
            {
                // Navigerer til forsiden
                case Routes.Home:
                    // Viser HomePage i PageContentControl
                    _pageControl.Content = new HomePage(this, _articleServices, _userServices, _categoryServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Home;
                    break;

                // Navigerer til Explore-siden
                case Routes.Explore:
                    // Viser ExplorePage
                    _pageControl.Content = new ExplorePage(this, _articleServices, _userServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Explore;
                    break;

                // Navigerer til kategorisiden
                case Routes.Categories:
                    // Viser CategoryPage
                    _pageControl.Content = new CategoryPage(_categoryServices, this, _articleServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Categories;
                    break;

                // Navigerer til favoritsiden
                case Routes.Favorites:
                    // Viser FavoritesPage
                    _pageControl.Content = new FavoritesPage(this, _userServices, _articleServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Favorites;
                    break;

                // Navigerer til mine ordrer
                case Routes.MyOrders:
                    // Viser MyOrdersPage
                    _pageControl.Content = new MyOrdersPage(this, _userServices, _reviewServices, _rentalServices, _articleServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.MyOrders;
                    break;

                // Navigerer til min konto
                case Routes.MyAccount:
                    // Viser MyAccountPage
                    _pageControl.Content = new MyAccountPage(this, _userServices, _reviewServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.MyAccount;
                    break;

                // Navigerer til beskeder
                case Routes.Messages:
                    // Viser MessagesPage
                    _pageControl.Content = new MessagesPage(this, _userServices, _conversationServices, _unreadBadgeService);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Messages;
                    break;

                // Navigerer til overview-siden
                case Routes.Overview:
                    // Viser OverviewPage med det aktuelle filter
                    _pageControl.Content = new OverviewPage(this, _articleServices, _userServices, _currentFilter);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Overview;
                    break;

                // Navigerer til artikelsiden
                case Routes.Article:
                    // Viser ArticlePage
                    _pageControl.Content = new ArticlePage(this, _articleServices, _categoryServices, _userServices, _conversationServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Article;
                    break;

                // Navigerer til For You-siden
                case Routes.ForYou:
                    // Viser ForYouPage
                    _pageControl.Content = new ForYouPage(this, _userServices, _articleServices, _categoryServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.ForYou;
                    break;

                // Navigerer til en brugerprofil
                case Routes.UserProfile:
                    // Viser UserPage
                    _pageControl.Content = new UserPage(_userServices, _articleServices, this, _reviewServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.UserProfile;
                    break;

                // Navigerer til siden hvor man kan oprette en artikel
                case Routes.CreateArticle:
                    // Viser CreateArticlePage
                    _pageControl.Content = new CreateArticlePage(this, _articleServices, _categoryServices, _sizeServices, _brandServices, _colorServices, _userServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.CreateArticle;
                    break;

                // Navigerer til lejesiden
                case Routes.Rent:
                    // Viser RentPage
                    _pageControl.Content = new RentPage(this, _articleServices, _userServices, _shippingOptionServices, _insuranceOptionServices, _rentalServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Rent;
                    break;

                // Navigerer til side med artikler sorteret efter kategori
                case Routes.ArticleSortByCategory:
                    // Viser ArticleSortByCategory med den valgte kategori
                    _pageControl.Content = new ArticleSortByCategory(_selectedCategory, _articleServices, _categoryServices, this, _userServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.ArticleSortByCategory;
                    break;

                // Navigerer til reviews
                case Routes.Reviews:
                    // Viser ReviewsPage
                    _pageControl.Content = new ReviewsPage(this, _reviewServices, _userServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.Reviews;
                    break;

                // Navigerer til rediger artikel-siden
                case Routes.EditArticlePage:
                    // Viser EditArticlePage
                    _pageControl.Content = new EditArticlePage(this, _articleServices, _categoryServices, _sizeServices, _brandServices, _userServices, _colorServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.EditArticlePage;
                    break;

                // Navigerer til mine artikler
                case Routes.MyArticlesPage:
                    // Viser MyArticlesPage
                    _pageControl.Content = new MyArticlesPage(_articleServices, _categoryServices, this, _userServices, _authServices);

                    // Opdaterer den aktuelle side
                    _currentPage = Routes.MyArticlesPage;
                    break;
            }
        }
    }

    // Enum der indeholder alle sider/routes i programmet
    public enum Routes
    {
        // Forsiden
        Home = 0,

        // Explore-siden
        Explore = 1,

        // Kategorisiden
        Categories = 2,

        // Favoritsiden
        Favorites = 3,

        // Mine ordrer
        MyOrders = 4,

        // Min konto
        MyAccount = 5,

        // Beskeder
        Messages = 6,

        // Oversigt/søgeresultat
        Overview = 7,

        // Artikelside
        Article = 8,

        // For You-side
        ForYou = 9,

        // Brugerprofil
        UserProfile = 10,

        // Opret artikel-side
        CreateArticle = 11,

        // Lejeside
        Rent = 12,

        // Review-side
        Reviews = 13,

        // Artikler sorteret efter kategori
        ArticleSortByCategory = 14,

        // Rediger artikel-side
        EditArticlePage = 15,

        // Mine artikler-side
        MyArticlesPage = 16
    }
}