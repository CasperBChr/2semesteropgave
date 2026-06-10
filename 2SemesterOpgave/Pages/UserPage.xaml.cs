using System.ComponentModel;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl og Button
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media; // Giver adgang til Brushes, som bruges til at ændre knappens farve
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User og Article
using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx UserServices, ArticleServices, ReviewServices og Router

namespace _2SemesterOpgave.Pages
{

    /// <summary>
    /// Interaction logic for UserPage.xaml === Kodet af Casper
    /// </summary>


    // Klassen er en WPF-side, som arver fra UserControl
    public partial class UserPage : UserControl
    {
        // Service der håndterer brugerdata
        UserServices _userServices;

        // Indeholder den bruger, som vises på siden
        public User CurrentUser { get; private set; }

        // Indeholder brugerens gennemsnitlige rating
        public float UserRating { get; private set; }

        // Service der håndterer artikler
        ArticleServices _articleServices;

        // Router bruges til at navigere mellem sider
        Router _router;

        // Service der håndterer reviews/anmeldelser
        ReviewServices _reviewServices;

        // Constructor der modtager de services siden skal bruge
        public UserPage(UserServices userServices, ArticleServices articleServices, Router router, ReviewServices reviewServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer user service, så siden kan hente CurrentUser og TargetUser
            _userServices = userServices;

            // Gemmer article service, så siden kan hente brugerens artikler
            _articleServices = articleServices;

            // Gemmer review service, så siden kan hente rating og sætte review target
            _reviewServices = reviewServices;

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Tjekker om der ikke er valgt en bruger
            if (_userServices.TargetUser == null)
            {
                // Viser en fejlbesked hvis brugeren ikke findes
                MessageBox.Show("Kan ikke finde brugeren.");
            }

            // Tjekker om den aktuelle bruger er den samme som profilen der vises
            if (_userServices.CurrentUser == _userServices.TargetUser)
            {
                // Skjuler følg-knappen, fordi man ikke skal kunne følge sig selv
                FollowButton.Visibility = Visibility.Collapsed;
            }

            // Sætter CurrentUser til den brugerprofil der vises
            CurrentUser = _userServices.TargetUser;

            // Henter gennemsnitsratingen for brugeren der vises
            UserRating = _reviewServices.GetAverageRating(CurrentUser.Id);

            //DataContext = CurrentUser;

            // Sætter datakonteksten til siden selv, så XAML kan binde til både CurrentUser og UserRating
            DataContext = this;

            // Sætter ItemsSource til den valgte brugers artikler, så de vises i UI
            ArticlesItemsControl.ItemsSource = _articleServices.GetAllArticlesByOwner(_userServices.TargetUser);

            // Opdaterer følg-knappen, så den viser korrekt tekst og farve
            UpdateFollowButton();
        }

        // Metode der kører, når brugeren klikker på knappen for at anmelde en bruger
        private void ReviewUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker om TargetUser mangler et gyldigt id
            if (_userServices.TargetUser.Id == 0)
            {
                // Viser en fejlbesked hvis brugeren ikke findes
                MessageBox.Show("Kan ikke finde brugeren.");

                // Stopper metoden, fordi der ikke er en gyldig bruger at vurdere
                return;
            }

            // Sætter den valgte bruger som den bruger der skal vurderes
            _reviewServices.SetReviewTarget(_userServices.TargetUser);

            // Navigerer til review-siden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Reviews));

            //_router.NavigateTo(Routes.Reviews);
        }

        // Opdaterer følg-knappen afhængigt af om brugeren allerede følger profilen
        private void UpdateFollowButton()
        {
            // Tjekker om brugeren er på sin egen profil
            if (_userServices.CurrentUser.Id == _userServices.TargetUser.Id)
            {
                // Skjuler følg-knappen, fordi man ikke kan følge sig selv
                FollowButton.Visibility = Visibility.Collapsed;

                // Stopper metoden, fordi knappen ikke skal opdateres mere
                return;
            }

            // Viser følg-knappen, hvis profilen ikke er brugerens egen
            FollowButton.Visibility = Visibility.Visible;

            // Tjekker om den aktuelle bruger allerede følger den viste bruger
            bool isFollowing = _userServices.IsFollowing(_userServices.CurrentUser, _userServices.TargetUser);

            // Hvis brugeren allerede følger profilen
            if (isFollowing)
            {
                // Ændrer knappens tekst til "Følger"
                FollowButton.Content = "Følger";

                // Ændrer knappens baggrundsfarve til rød
                FollowButton.Background = Brushes.Red;
            }
            else
            {
                // Ændrer knappens tekst til "Følg"
                FollowButton.Content = "Følg";

                // Ændrer knappens baggrundsfarve til turkis
                FollowButton.Background = Brushes.Turquoise;
            }
        }

        // Metode der kører, når brugeren klikker på følg-knappen
        private void FollowButton_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker om CurrentUser eller TargetUser mangler
            if (_userServices.CurrentUser == null || _userServices.TargetUser == null)
                return;

            // Tjekker om den aktuelle bruger allerede følger den viste bruger
            bool isFollowing = _userServices.IsFollowing(_userServices.CurrentUser, _userServices.TargetUser);

            // Hvis brugeren allerede følger profilen
            if (isFollowing)
            {
                // Fjerner CurrentUser som følger af TargetUser
                _userServices.RemoveFollower(_userServices.CurrentUser, _userServices.TargetUser);

                // Trækker én fra TargetUsers antal følgere
                _userServices.TargetUser.FollowersCount--;

                // Trækker én fra CurrentUsers antal profiler de følger
                _userServices.CurrentUser.FollowingCount--;
            }
            else
            {
                // Tilføjer CurrentUser som følger af TargetUser
                _userServices.AddFollower(_userServices.CurrentUser, _userServices.TargetUser);

                // Lægger én til TargetUsers antal følgere
                _userServices.TargetUser.FollowersCount++;

                // Lægger én til CurrentUsers antal profiler de følger
                _userServices.CurrentUser.FollowingCount++;
            }

            // Opdaterer knappen efter follow/unfollow
            UpdateFollowButton();
        }

        // Event handler for når en artikelknap klikkes, som navigerer til ArticlePage med den valgte artikel
        private void ArticlePageButton_Click(object sender, RoutedEventArgs e)
        {
            // (Button) er typecasting, det fortæller kompileren at "sender" er en Button
            Button button = (Button)sender;

            // Her sætter vi den valgte artikel i ArticleServices, så den kan bruges på ArticlePage
            _articleServices.SelectedArticle = (Article)button.DataContext;

            // Navigerer til ArticlePage ved at kalde NavigateTo gennem Command Pattern NavigateCommand der indeholder Router med den relevante rute
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Article));

            // Navigerer til ArticlePage ved at kalde NavigateTo på Router med den relevante rute
            //_router.NavigateTo(Routes.Article);
        }
    }
}