using _2SemesterOpgave.Algoritme;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Services;
using System;
using System.Collections.Generic;
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

namespace _2SemesterOpgave.Pages
{
    //Kodet af Camilla
    public partial class ForYouPage : UserControl
    {
        public Router Router;
        public UserServices UserServices;
        public ArticleServices ArticleServices;
        public CategoryServices CategoryServices;
        public ForYouPage(Router router, UserServices userService, ArticleServices articleService, CategoryServices categoryService)
        {
            InitializeComponent();
            Router = router;
            UserServices = userService;
            ArticleServices = articleService;
            CategoryServices = categoryService;
        }

        public void InitializeAlgorithm(User user)
        {
            //Liste der henter kategorier
            var categories = CategoryServices.GetAllCategories();

            //Opretter en liste af features baseret på kategorierne, som bruges til at oprette brugerprofilen
            var features = new List<string>();
            foreach (var category in categories)
            {
                features.Add(category.Name);
            }

            //Katalog af elementer
            var catalog = new List<ItemProfile>
            {

            };

            //Opret en ny bruger
            UserProfile newUser = new UserProfile(user.Id.ToString(), features);

            //Console.WriteLine("--- Ny bruger registreret (Alt er vægtet til 0) ---");
            //PrintRecommendations(newUser, catalog);
      
            //newUser.UpdateUserProfileView(catalog[0]);
          
            PrintRecommendations(newUser, catalog);
        }

        static void PrintRecommendations(UserProfile user, List<ItemProfile> catalog)
        {
            //Henter og printer anbefalingerne for brugeren
            var recs = ContentBasedAlgorithm.GetRecommendations(user, catalog);

            //Printer anbefalingerne i konsollen
            foreach (var rec in recs)
            {
                //Udskriver anbefalingerne i konsollen
                Console.WriteLine($"- {rec.Item.Name} (Match Score: {rec.Score:F2})");
            }

        }
    }
}
