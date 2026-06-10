using _2SemesterOpgave.Services; // Giver adgang til vores serviceklasser, fx ArticleServices, CategoryServices og Router
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article, Category og Brand
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls; // Giver adgang til WPF controls som UserControl, Button, ComboBox og Image
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2SemesterOpgave.Repositories; // Giver adgang til repositories, fx ArticleRepository

namespace _2SemesterOpgave.Pages
{
    /// <summary>
    /// Kodet af Camilla. Interaction logic for CreateArticlePage.xaml.
    /// </summary>

    // Klassen er en WPF-side, som arver fra UserControl
    public partial class CreateArticlePage : UserControl
    {
        // Repository der håndterer artikler direkte i databasen
        ArticleRepository _articleRepository;

        // Router bruges til at navigere mellem sider
        private Router _router;

        // Indeholder den artikel der arbejdes med
        private Models.Article _currentArticle;

        // Service der håndterer artikler
        private ArticleServices _articleServices;

        // Service der håndterer kategorier og underkategorier
        private CategoryServices _categoryServices;

        // Service der håndterer størrelser
        private SizeServices _sizeServices;

        // Service der håndterer brands/mærker
        private BrandServices _brandServices;

        // Service der håndterer farver
        private ColorServices _colorServices;

        // Service der håndterer brugerdata
        private UserServices _userServices;

        // Constructor der modtager de services siden skal bruge
        public CreateArticlePage(Router router, ArticleServices articleServices, CategoryServices categoryServices, SizeServices sizeServices, BrandServices brandServices, ColorServices colorServices, UserServices userServices)
        {
            // Initialiserer XAML-designet og gør sidens UI-elementer klar
            InitializeComponent();

            // Gemmer routeren, så siden kan navigere til andre sider
            _router = router;

            // Gemmer article service, så siden kan oprette artikler
            _articleServices = articleServices;

            // Gemmer category service, så siden kan hente kategorier og underkategorier
            _categoryServices = categoryServices;

            // Gemmer size service, så siden kan hente størrelser
            _sizeServices = sizeServices;

            // Gemmer brand service, så siden kan hente mærker
            _brandServices = brandServices;

            // Gemmer color service, så siden kan hente farver
            _colorServices = colorServices;

            // Gemmer user service, så artiklen kan kobles til den aktuelle bruger
            _userServices = userServices;

            // Fylder kategori-comboboxen med alle kategorier
            CreateCategoryCombobox.ItemsSource = _categoryServices.GetAllCategories();

            // Fylder underkategori-comboboxen med alle underkategorier
            CreateSubcategoryCombobox.ItemsSource = _categoryServices.GetAllSubCategories();

            // Fylder størrelse-comboboxen med alle størrelser
            CreateSizeComboBox.ItemsSource = _sizeServices.GetAllSizes();

            // Fylder farve-comboboxen med alle farver
            CreateColorComboBox.ItemsSource = _colorServices.GetAllColors();

            // Fylder brand-comboboxen med alle mærker
            CreateBrandComboBox.ItemsSource = _brandServices.GetAllBrands();
        }

        // Metode der kører, når brugeren klikker på upload billede-knappen
        public void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Opretter et nyt Image-objekt
            Image createArticleImage = new Image();
        }

        //Metode til at gemme den oprettede artikel og navigere tilbage til oversigten
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Opretter et nyt Article-objekt
            Article article = new Article();

            // Starter med en tom titel
            string title = string.Empty;

            // Tjekker om titel-feltet ikke er tomt
            if (!string.IsNullOrWhiteSpace(CreateTitleTextBox.Text))
            {
                // Gemmer teksten fra titel-feltet
                title = CreateTitleTextBox.Text;
            }
            else
            {
                // Viser fejlbesked hvis brugeren ikke har skrevet en titel
                MessageBox.Show("Venligst indtast titel");

                // Stopper metoden, fordi titel mangler
                return;
            }

            // Starter med en tom beskrivelse
            string description = string.Empty;

            // Tjekker om beskrivelse-feltet ikke er tomt
            if (!string.IsNullOrWhiteSpace(CreateDescriptionTextBox.Text))
            {
                // Gemmer teksten fra beskrivelse-feltet
                description = CreateDescriptionTextBox.Text;
            }
            else
            {
                // Viser fejlbesked hvis brugeren ikke har skrevet en beskrivelse
                MessageBox.Show("Venligst indtast beskrivelse");

                // Stopper metoden, fordi beskrivelse mangler
                return;
            }

            // Variabel til den valgte kategori
            Category category;

            // Tjekker om der er valgt en Category i comboboxen
            if (CreateCategoryCombobox.SelectedItem is Category)
            {
                // Gemmer den valgte kategori
                category = (Category)CreateCategoryCombobox.SelectedItem;
            }
            else
            {
                // Viser fejlbesked hvis der ikke er valgt kategori
                MessageBox.Show("Venligst vælg kategori");

                // Stopper metoden, fordi kategori mangler
                return;
            }

            // Variabel til den valgte underkategori
            SubCategory subCategory;

            // Tjekker om der er valgt en SubCategory i comboboxen
            if (CreateSubcategoryCombobox.SelectedItem is SubCategory)
            {
                // Gemmer den valgte underkategori
                subCategory = (SubCategory)CreateSubcategoryCombobox.SelectedItem;
            }
            else
            {
                // Viser fejlbesked hvis der ikke er valgt underkategori
                MessageBox.Show("Venligst vælg underkategori");

                // Stopper metoden, fordi underkategori mangler
                return;
            }

            // Variabel til den valgte størrelse
            Models.Size size;

            // Tjekker om der er valgt en Size i comboboxen
            if (CreateSizeComboBox.SelectedItem is Models.Size)
            {
                // Gemmer den valgte størrelse
                size = (Models.Size)CreateSizeComboBox.SelectedItem;
            }
            else
            {
                // Viser fejlbesked hvis der ikke er valgt størrelse
                MessageBox.Show("Venligst vælg størrelse");

                // Stopper metoden, fordi størrelse mangler
                return;
            }

            // Starter dagsprisen på 0
            float dailyPrice = 0.0f;

            // Prøver at konvertere teksten fra prisfeltet til et kommatal
            bool dailyPriceConverted = float.TryParse(CreatePriceTextBox.Text, out dailyPrice);

            // Tjekker om prisen ikke kunne konverteres
            if (!dailyPriceConverted)
            {
                // Viser fejlbesked hvis prisen ikke er et gyldigt tal
                MessageBox.Show("Indtast venligst gyldigt tal");

                // Stopper metoden, fordi prisen er ugyldig
                return;
            }

            // Variabel til den valgte farve
            Models.Color color;

            // Tjekker om der er valgt en Color i comboboxen
            if (CreateColorComboBox.SelectedItem is Models.Color)
            {
                // Gemmer den valgte farve
                color = (Models.Color)CreateColorComboBox.SelectedItem;
            }
            else
            {
                // Viser fejlbesked hvis der ikke er valgt farve
                MessageBox.Show("Venligst vælg farve");

                // Stopper metoden, fordi farve mangler
                return;
            }

            // Variabel til det valgte mærke
            Brand brand;

            // Tjekker om der er valgt et Brand i comboboxen
            if (CreateBrandComboBox.SelectedItem is Brand)
            {
                // Gemmer det valgte mærke
                brand = (Brand)CreateBrandComboBox.SelectedItem;
            }
            else
            {
                // Viser fejlbesked hvis der ikke er valgt mærke
                MessageBox.Show("Venligst vælg mærke");

                // Stopper metoden, fordi mærke mangler
                return;
            }

            // Sætter artiklens titel
            article.Title = title;

            // Sætter artiklens beskrivelse
            article.Description = description;

            // Sætter artiklens kategori
            article.Category = category;

            // Sætter artiklens underkategori
            article.SubCategory = subCategory;

            // Sætter artiklens størrelse
            article.Size = size;

            // Sætter artiklens dagspris
            article.DailyPrice = dailyPrice;

            // Sætter artiklens farve
            article.Color = color;

            // Sætter artiklens mærke
            article.Brand = brand;

            // Opretter artiklen gennem ArticleServices og kobler den til den aktuelle bruger
            _articleServices.CreateArticle(article, _userServices.CurrentUser);

            // Navigerer til forsiden og gemmer navigationen i historikken
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Home));

            //_router.NavigateTo(Routes.Home);
        }

        // Metode der kører, når brugeren klikker på annuller/luk-knappen
        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            //Gemmer ikke den oprettede artikel og navigerer tilbage til oversigten
            _router.ExecuteAndRecord(new NavigateCommand(_router, Routes.Home));

            //_router.NavigateTo(Routes.Home);
        }

        // Metode der kører, når brugeren vælger en kategori
        private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Tjekker om der ikke er valgt en kategori
            if (CreateCategoryCombobox.SelectedItem == null)
            {
                // Stopper metoden, fordi der ikke er en kategori at hente underkategorier fra
                return;
            }

            // Henter den valgte kategori fra comboboxen
            Category chosenCategory = (Category)CreateCategoryCombobox.SelectedItem;

            // Opdaterer underkategori-comboboxen med underkategorier fra den valgte kategori
            CreateSubcategoryCombobox.ItemsSource = chosenCategory.SubCategories;
        }
    }
}