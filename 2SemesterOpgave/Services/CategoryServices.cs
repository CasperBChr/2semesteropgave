using _2SemesterOpgave.Repositories; // Giver adgang til CategoryRepository
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Category og SubCategory

namespace _2SemesterOpgave.Services
{
    // Serviceklasse der håndterer logik for kategorier og underkategorier
    public class CategoryServices
    {
        // Repository der bruges til databasekald for kategorier
        CategoryRepository _categoryRepository;

        // Dictionary der bruges til hurtigt at finde en kategori ud fra id
        Dictionary<int, Category> _categoryLookup = new Dictionary<int, Category>();

        // Dictionary der bruges til hurtigt at finde en underkategori ud fra id
        Dictionary<int, SubCategory> _subCategoryLookup = new Dictionary<int, SubCategory>();

        // Constructor der modtager CategoryRepository
        public CategoryServices(CategoryRepository categoryRepository)
        {
            // Gemmer CategoryRepository, så den kan bruges i metoderne
            _categoryRepository = categoryRepository;

            // Indlæser kategorier og underkategorier i cache
            InitializeCache();
        }

        // Indlæser alle kategorier og underkategorier i dictionaries
        void InitializeCache()
        {
            // Henter alle kategorier fra repository
            IEnumerable<Category> categories = _categoryRepository.GetAllCategories();

            // Gennemgår alle kategorier
            foreach (Category category in categories)
            {
                // Gemmer kategorien i lookup med kategoriens id som nøgle
                _categoryLookup[category.Id] = category;

                // Gennemgår alle underkategorier i kategorien
                foreach (SubCategory sub in category.SubCategories)
                {
                    // Gemmer underkategorien i lookup med underkategoriens id som nøgle
                    _subCategoryLookup[sub.Id] = sub;
                }
            }
        }

        // Henter et tilfældigt antal kategorier
        public IEnumerable<Category> GetRandomCategories(int amount)
        {
            // Henter alle kategorier og laver dem om til en liste
            List<Category> categories = new List<Category>(GetAllCategories());

            // HashSet bruges for at undgå dubletter, når der er nok kategorier
            HashSet<Category> randomCategories = new HashSet<Category>();

            // Random bruges til at vælge tilfældige kategorier
            Random random = new Random();

            // Tjekker om der er færre kategorier end det ønskede antal
            if (categories.Count < amount)
            {
                // Tilføjer tilfældige kategorier, selvom der kan komme dubletter
                for (int i = 0; i < amount; i++)
                {
                    // Vælger en tilfældig kategori og tilføjer den
                    randomCategories.Add(categories[random.Next(0, categories.Count)]);
                }
            }
            else
            {
                // Fortsætter indtil der er nok unikke tilfældige kategorier
                for (int i = 0; randomCategories.Count < amount; i++)
                {
                    // Vælger en tilfældig kategori og tilføjer den
                    randomCategories.Add(categories[random.Next(0, categories.Count)]);
                }
            }

            // Returnerer de tilfældige kategorier
            return randomCategories;
        }

        // Henter alle kategorier
        public IEnumerable<Category> GetAllCategories()
        {
            // Returnerer alle kategorier fra lookup
            return _categoryLookup.Values;
        }

        // Henter alle underkategorier
        public IEnumerable<SubCategory> GetAllSubCategories()
        {
            // Returnerer alle underkategorier fra lookup
            return _subCategoryLookup.Values;
        }

        // Henter en kategori ud fra id
        public Category? GetCategoryById(int id)
        {
            // Prøver at finde kategorien i lookup
            _categoryLookup.TryGetValue(id, out Category? category);

            // Returnerer kategorien, eller null hvis den ikke findes
            return category;
        }

        // Henter en underkategori ud fra id
        public SubCategory? GetSubCategoryById(int id)
        {
            // Prøver at finde underkategorien i lookup
            _subCategoryLookup.TryGetValue(id, out SubCategory? subCategory);

            // Returnerer underkategorien, eller null hvis den ikke findes
            return subCategory;
        }
    }
}