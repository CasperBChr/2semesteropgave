using _2SemesterOpgave.Models;
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
using System.Data.Common;
using System.Diagnostics;

namespace _2SemesterOpgave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContentControl _pageControl;
        private Router _router;
        public MainWindow()
        {
            InitializeComponent();
            _pageControl = PageContentControl;
            _router = new Router(_pageControl);
            User user = new User();
            user.Username = "Mads";

            string dbpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "db.db");
            Database db = new Database($"Data Source={dbpath}");

            db.Open();
            DbCommand command = db.Connection.CreateCommand();
            command.CommandText = "SELECT Username FROM User";
            List<string> args = new List<string>();
            DbDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                args.Add(reader.GetString(0));
            }

            reader.Close();
            db.Close();

            for (int i = 0; i < args.Count; i++)
            {
                Debug.WriteLine(args[i]);
            }

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