using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article, User og Conversation
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ArticleServices, UserServices og Router
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl og Button
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2SemesterOpgave.Pages
{
    // Klassen er en WPF-side, som arver fra UserControl
    /// <summary>
    /// Kodet af Camilla. Interaction logic for ArticlePage.xaml.
    /// </summary>
    public partial class ArticlePage : UserControl
    {
        // Router bruges til at navigere mellem sider
        private Router _router;

        // Indeholder den artikel, som vises på siden
        private Article _currentArticle;

        // Service der håndterer artikler
        private ArticleServices _articleServices;

        // Service der håndterer kategorier
        private CategoryServices _categoryServices;

        // Service der håndterer brugerdata
        UserServices _userServices;

        // Holder styr på om den aktuelle artikel er favorit
        private bool _isFavorite;

        // Service der håndterer samtaler/chat
        ConversationServices _conversationServices;

        //Constructor der tager en Router som parameter for at kunne navigere til andre sider
        public ArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices, UserServices userService, ConversationServices conversationServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer article service, så siden kan bruge den valgte artikel og favoritfunktioner
            _articleServices = articleServices;

            // Gemmer category service, hvis siden skal bruge kategori-data
            _categoryServices = categoryServices;

            // Gemmer conversation service, så siden kan oprette eller hente en samtale
            _conversationServices = conversationServices;

            // Sætter DataContext til den valgte artikel, så XAML kan vise artiklens data
            DataContext = _articleServices.SelectedArticle;

            // Gemmer den valgte artikel som den aktuelle artikel på siden
            _currentArticle = _articleServices.SelectedArticle!;

            // Gemmer user service, så siden kan bruge den aktuelle bruger og artikelens ejer
            _userServices = userService;

            // Kalder SetArticle, så siden bliver sat korrekt op med den aktuelle artikel
            SetArticle(_currentArticle);
        }

        //DataContext for at binde Article-objektet til ArticlePage
        public void SetArticle(Article article)
        {
            // Sætter sidens DataContext til artiklen, så XAML kan vise dens properties
            this.DataContext = article;

            // Gemmer artiklen som den aktuelle artikel
            _currentArticle = article;

            // Tjekker om den aktuelle artikel allerede er markeret som favorit af brugeren
            _isFavorite = _articleServices.IsFavorite(_userServices.CurrentUser, _currentArticle);

            // Opdaterer favoritknappen, så den viser korrekt ikon
            UpdateFavoriteUI();
        }


        //Funktion der navigerer til chat med ejer af artiklen
        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker om artiklen mangler en ejer, eller om routeren mangler
            if (_currentArticle?.Owner == null || _router == null)
            {
                // Stopper metoden, fordi der ikke kan oprettes en chat uden ejer eller router
                return;
            }

            // Henter den bruger, som er logget ind
            User currentUser = _userServices.CurrentUser;

            // Henter ejeren af den aktuelle artikel
            User owner = _currentArticle.Owner;

            // Henter en eksisterende samtale eller opretter en ny samtale mellem brugeren og ejeren
            Conversation conversation = _conversationServices.GetOrCreateConversation(currentUser, owner);

            // Sætter samtalen som den aktuelle samtale, så Messages-siden kan vise den
            _conversationServices.CurrentConversation = conversation;

            // Navigerer til besked-siden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Messages));

            //_router.NavigateTo(Routes.Messages);
        }

        //Metode der navigerer til udlejnings siden for artiklen
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Navigerer til rent-siden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Rent));

            //_router.NavigateTo(Routes.Rent);
        }

        // Metode der kører, når brugeren klikker på ejerens profil
        private void OwnerProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Finder den knap, der blev klikket på
            Button button = (Button)sender;

            // Henter artiklen fra knappens DataContext
            Article article = (Article)button.DataContext;

            // Sætter artikelens ejer som TargetUser, så UserProfile-siden kan vise ejeren
            _userServices.TargetUser = article.Owner;

            // Navigerer til brugerprofilen og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.UserProfile));

            //_router.NavigateTo(Routes.UserProfile);
        }

        // Metode der kører, når brugeren klikker på favorit-knappen
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker om der mangler en artikel eller en aktuel bruger
            if (_currentArticle == null || _userServices.CurrentUser == null)
                return;

            // Tjekker om artiklen allerede er favorit
            if (_isFavorite)
            {
                // Fjerner artiklen fra brugerens favoritter
                _articleServices.RemoveFavorite(_userServices.CurrentUser, _currentArticle);

                // Opdaterer variablen så artiklen ikke længere er favorit
                _isFavorite = false;
            }
            else
            {
                // Tilføjer artiklen til brugerens favoritter
                _articleServices.AddFavorite(_userServices.CurrentUser, _currentArticle);

                // Opdaterer variablen så artiklen nu er favorit
                _isFavorite = true;
            }

            // Opdaterer favoritknappens ikon
            UpdateFavoriteUI();
        }

        // Opdaterer favoritknappen afhængigt af om artiklen er favorit
        private void UpdateFavoriteUI()
        {
            // Tjekker om artiklen er favorit
            if (_isFavorite)
            {
                // Viser et fyldt hjerte hvis artiklen er favorit
                FavoriteButton.Content = "❤️";

                // Stopper metoden, fordi knappen allerede er opdateret
                return;
            }

            // Viser et tomt hjerte hvis artiklen ikke er favorit
            FavoriteButton.Content = "🤍";
        }
    }
}