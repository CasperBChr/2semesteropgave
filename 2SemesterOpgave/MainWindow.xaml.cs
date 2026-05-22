using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
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

namespace _2SemesterOpgave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContentControl _pageControl;
        private Router _router;

        ObservableCollection<Article> articles = new ObservableCollection<Article>();
        ObservableCollection<Category> categories = new ObservableCollection<Category>();
        ObservableCollection<SubCategory> subCategories = new ObservableCollection<SubCategory>();
        ObservableCollection<Brand> brands = new ObservableCollection<Brand>();
        ObservableCollection<Designer> designers = new ObservableCollection<Designer>();
        ObservableCollection<Collection> collections = new ObservableCollection<Collection>();
        ObservableCollection<User> users = new ObservableCollection<User>();
        ObservableCollection<Conversation> conversations = new ObservableCollection<Conversation>();
        ObservableCollection<Message> messages = new ObservableCollection<Message>();
        ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();
        //ObservableCollection<Wishlist> wishlists = new ObservableCollection<Wishlist>(); // Favoritter
        ObservableCollection<Rental> rentals = new ObservableCollection<Rental>();
        ObservableCollection<ShippingOption> shippingOptions = new ObservableCollection<ShippingOption>();
        ObservableCollection<InsuranceOption> insuranceOptions = new ObservableCollection<InsuranceOption>();
        ObservableCollection<Accesibility> accesibilities = new ObservableCollection<Accesibility>();
        UserRepository userRepository;
        public MainWindow()
        {
            InitializeComponent();
            _pageControl = PageContentControl;
            _router = new Router(_pageControl, articles, categories);

            string dbpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "db.db");
            Database db = new Database($"Data Source={dbpath}");

            userRepository = new UserRepository(db);

            List<User> allUsers = userRepository.GetAllUsers();

            //for (int i = 0; i < args.Count; i++)
            //{
            //    Debug.WriteLine(args[i]);
            //}

            //Notification notification = new Notification();
            //notification.Message = "Du har modtaget en ny besked!";

            //notification.ReceiveNotification(notification.Message);


            //User user = new User();
            //user.FirstName = "John";
            //user.LastName = "Doe";
            //user.Email = "john.doe@example.com";
            //user.Password = "password123";

            string[] FirstNames = { "Mark", "Torben", "Claus", "Mathias", "Lone", "Lis", "Mads" };
            string[] LastNames = { "Andersen", "Jensen", "Hansen", "Pedersen", "Nielsen", "Larsen", "Møller" };

            List<User> listoffall = new List<User>();

            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                int randomNumber = random.Next(0, FirstNames.Length);
                int randomNumber2 = random.Next(0, LastNames.Length);

                string tempFirstName = FirstNames[randomNumber];
                string tempLastName = LastNames[randomNumber2];

                User user1 = new User();
                user1.FirstName = tempFirstName;
                user1.LastName = tempLastName;
                listoffall.Add(user1);

                //Console.WriteLine($"Navn: {tempFirstName} {tempLastName}");
                //Console.WriteLine(FirstNames.Length);
                //Console.WriteLine(LastNames.Length);

                //Console.WriteLine($"Tilfældigt tal: {randomNumber}");
                //Console.WriteLine($"Tilfældigt tal2: {randomNumber2}");
            }

            for (int i = 0; i < listoffall.Count; i++)
            {
                Console.WriteLine(listoffall[i].FirstName + " " + listoffall[i].LastName);
            }

            for (int i = 0; i < 10; i++)
            {
                Article article = new Article($"Test Artikel {i + 1}", $"Dette er en test artikel {i + 1}", 3500.0f, 150.0f, false, false, false, false);
                Category category = new Category($"Kategori {i + 1}");
                SubCategory subCategory = new SubCategory($"Underkategori {i + 1}", category);
                Brand brand = new Brand($"Mærke {i + 1}", $"Mærke {i + 1}", $"Mærke {i + 1}");
                Designer designer = new Designer($"Designer {i + 1}");
                Collection collection = new Collection($"Kollektion {i + 1},", $"Kollektion {i + 1},", brand, designer, new List<Article>());
                User user = new User($"Bruger {i + 1}", $"bruger{i + 1}@example.com", $"bruger{i + 1}@example.com", i + 2);
                //Conversation conversation = new Conversation(user, $"Samtale {i + 1}");

                userRepository.AddUser(user);
                articles.Add(article);
                categories.Add(category);
                subCategories.Add(subCategory);
                brands.Add(brand);
                designers.Add(designer);
                collections.Add(collection);
                users.Add(user);
                //conversations.Add(conversation);
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuBtnClick(object sender, RoutedEventArgs e)
        {
        
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
    }
}