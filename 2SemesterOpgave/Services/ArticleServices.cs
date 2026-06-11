using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Algoritme; // Giver adgang til algoritme-klasser, fx ItemProfile
using _2SemesterOpgave.Data; // Giver adgang til data-laget
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article og User
using _2SemesterOpgave.Repositories; // Giver adgang til repositories, fx ArticleRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til DTO-klasser, fx ArticleDTO

namespace _2SemesterOpgave.Services
{
	// Serviceklasse der håndterer logik for artikler
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	public class ArticleServices
    {
        // Repository der bruges til databasekald for artikler
        ArticleRepository _articleRepository;

        // Service der bruges til at hente brugere
        UserServices _userServices;

        // Service der bruges til at hente brands
        BrandServices _brandServices;

        // Service der bruges til at hente kategorier og underkategorier
        CategoryServices _categoryServices;

        // Service der bruges til at hente farver
        ColorServices _colorServices;

        // Service der bruges til at hente størrelser
        SizeServices _sizeServices;

        // Gemmer den valgte artikel
        public Article? SelectedArticle { get; set; }

        // Constructor der modtager de repositories og services klassen skal bruge
        public ArticleServices(ArticleRepository articleRepository, UserServices userService, BrandServices brandService, CategoryServices categoryServices, ColorServices colorServices, SizeServices sizeServices)
        {
            // Gemmer ArticleRepository, så den kan bruges i metoderne
            _articleRepository = articleRepository;

            // Gemmer BrandServices, så brand-data kan hentes
            _brandServices = brandService;

            // Gemmer CategoryServices, så kategori-data kan hentes
            _categoryServices = categoryServices;

            // Gemmer ColorServices, så farve-data kan hentes
            _colorServices = colorServices;

            // Gemmer SizeServices, så størrelse-data kan hentes
            _sizeServices = sizeServices;

            // Gemmer UserServices, så bruger-data kan hentes
            _userServices = userService;
        }

        // Opretter en ny artikel for en bruger
        public void CreateArticle(Article article, User user)
        {
            // Sender artiklen videre til repository, som gemmer den i databasen
            _articleRepository.CreateArticle(article, user);
        }

        // Sletter en artikel
        public void DeleteArticle(Article article)
        {
            // Sender artiklen videre til repository, som sletter den fra databasen
            _articleRepository.DeleteArticle(article);
        }

        // Henter alle artikler
        public IEnumerable<Article> GetAllArticles()
        {
            // Henter alle artikel-DTO'er fra repository
            IEnumerable<ArticleDTO> dtos = _articleRepository.GetAllArticles();

            // Opretter en liste til Article-objekter
            ObservableCollection<Article> articles = new ObservableCollection<Article>();

            // Gennemgår alle DTO'er
            foreach (ArticleDTO dto in dtos)
            {
                // Mapper DTO'en til et Article-objekt og tilføjer det til listen
                articles.Add(MapToArticle(dto));
            }

            // Returnerer listen med artikler
            return articles;
        }

        // Henter alle artikler der tilhører en bestemt ejer
        public IEnumerable<Article> GetAllArticlesByOwner(User owner)
        {
            // Henter artikel-DTO'er ud fra ejerens id
            IEnumerable<ArticleDTO> dtos = _articleRepository.GetAllArticlesByOwner(owner.Id);

            // Opretter en liste til Article-objekter
            ObservableCollection<Article> articles = new ObservableCollection<Article>();

            // Gennemgår alle DTO'er
            foreach (ArticleDTO dto in dtos)
            {
                // Mapper DTO'en til et Article-objekt og tilføjer det til listen
                articles.Add(MapToArticle(dto));
            }

            // Returnerer listen med ejerens artikler
            return articles;
        }

        // Henter de nyeste artikler
        public IEnumerable<Article> GetNewestArticles()
        {
            // Henter de nyeste artikel-DTO'er fra repository
            IEnumerable<ArticleDTO> dtos = _articleRepository.GetNewestArticles();

            // Opretter en liste til Article-objekter
            ObservableCollection<Article> articles = new ObservableCollection<Article>();

            // Gennemgår alle DTO'er
            foreach (ArticleDTO dto in dtos)
            {
                // Mapper DTO'en til et Article-objekt og tilføjer det til listen
                articles.Add(MapToArticle(dto));
            }

            // Returnerer listen med de nyeste artikler
            return articles;
        }

        // Henter et tilfældigt antal artikler
        public IEnumerable<Article> GetRandomArticles(int amount)
        {
            // Henter alle artikler og laver dem om til en liste
            List<Article> articles = new List<Article>(GetAllArticles());

            // HashSet bruges for at undgå dubletter, når der er nok artikler
            HashSet<Article> randomArticles = new HashSet<Article>();

            // Random bruges til at vælge tilfældige artikler
            Random random = new Random();

            // Tjekker om der er færre artikler end det ønskede antal
            if (articles.Count < amount)
            {
                // Tilføjer tilfældige artikler, selvom der kan komme dubletter
                for (int i = 0; i < amount; i++)
                {
                    // Vælger en tilfældig artikel og tilføjer den
                    randomArticles.Add(articles[random.Next(0, articles.Count)]);
                }
            }
            else
            {
                // Fortsætter indtil der er nok unikke tilfældige artikler
                for (int i = 0; randomArticles.Count < amount; i++)
                {
                    // Vælger en tilfældig artikel og tilføjer den
                    randomArticles.Add(articles[random.Next(0, articles.Count)]);
                }
            }

            // Returnerer de tilfældige artikler
            return randomArticles;
        }

        // Henter artikler ud fra et filter
        public IEnumerable<Article> GetFilteredArticles(FilterCriteria filter)
        {
            // Henter filtrerede artikel-DTO'er fra repository
            IEnumerable<ArticleDTO> dtos = _articleRepository.GetFilteredArticles(filter);

            // Opretter en liste til Article-objekter
            ObservableCollection<Article> articles = new ObservableCollection<Article>();

            // Gennemgår alle DTO'er
            foreach (ArticleDTO dto in dtos)
            {
                // Mapper DTO'en til et Article-objekt og tilføjer det til listen
                articles.Add(MapToArticle(dto));
            }

            // Returnerer de filtrerede artikler
            return articles;
        }

        // Opdaterer en artikel
        public void UpdateArticle(Article article)
        {
            // Sender artiklen videre til repository, som opdaterer den i databasen
            _articleRepository.UpdateArticle(article);
        }

        // VIRKER IKKE LÆNGERE
        // Henter artikler ud fra kategori-id
        public IEnumerable<Article> GetArticlesByCategory(int categoryId)
        {
            // Henter alle artikler
            IEnumerable<Article> allArticles = GetAllArticles();

            // Opretter en liste til de filtrerede artikler
            List<Article> filteredArticles = new List<Article>();

            // Gennemgår alle artikler
            foreach (Article article in allArticles)
            {
                // Tjekker om artiklen har en kategori, og om kategori-id matcher
                if (article.Category != null && article.Category.Id == categoryId)
                {
                    // Tilføjer artiklen til listen
                    filteredArticles.Add(article);
                }
            }

            // Returnerer artiklerne i den valgte kategori
            return filteredArticles;
        }

        // VIRKER IKKE LÆNGERE
        // Henter artikler ud fra ejer-id
        public IEnumerable<Article> GetArticlesByOwner(int ownerId)
        {
            // Henter alle artikler
            IEnumerable<Article> allArticles = GetAllArticles();

            // Opretter en liste til de filtrerede artikler
            List<Article> filteredArticles = new List<Article>();

            // Gennemgår alle artikler
            foreach (Article article in allArticles)
            {
                // Tjekker om artiklen har en ejer, og om ejer-id matcher
                if (article.Owner != null && article.Owner.Id == ownerId)
                {
                    // Tilføjer artiklen til listen
                    filteredArticles.Add(article);
                }
            }

            // Returnerer artiklerne fra den valgte ejer
            return filteredArticles;
        }



        // Henter alle artikler som en bestemt bruger har markeret som favorit
        public IEnumerable<Article> GetAllFavoritedArticlesByUser(int userId)
        {
            // Henter favorit-artikel-DTO'er fra repository
            IEnumerable<ArticleDTO> dtos = _articleRepository.GetAllFavoritedArticlesByUser(userId);

            // Opretter en liste til Article-objekter
            List<Article> articles = new List<Article>();

            // Gennemgår alle DTO'er
            foreach (ArticleDTO dto in dtos)
            {
                // Mapper DTO'en til et Article-objekt og tilføjer det til listen
                articles.Add(MapToArticle(dto));
            }

            // Returnerer brugerens favoritartikler
            return articles;
        }

        // Tilføjer en artikel til en brugers favoritter
        public void AddFavorite(User user, Article article)
        {
            // Tjekker om user mangler
            if (user == null)
            {
                // Kaster en fejl hvis user er null
                throw new Exception(nameof(user));
            }

            // Tjekker om article mangler
            if (article == null)
            {
                // Kaster en fejl hvis article er null
                throw new Exception(nameof(article));
            }

            // Tilføjer favorit-relationen i repository
            _articleRepository.AddFavorite(user.Id, article.Id);
        }

        // Fjerner en artikel fra en brugers favoritter
        public void RemoveFavorite(User user, Article article)
        {
            // Tjekker om user mangler
            if (user == null)
            {
                // Kaster en fejl hvis user er null
                throw new Exception(nameof(user));
            }

            // Tjekker om article mangler
            if (article == null)
            {
                // Kaster en fejl hvis article er null
                throw new Exception(nameof(article));
            }

            // Fjerner favorit-relationen i repository
            _articleRepository.RemoveFavorite(user.Id, article.Id);
        }

        // Tjekker om en artikel er favorit for en bruger
        public bool IsFavorite(User user, Article article)
        {
            // Tjekker om user mangler
            if (user == null)
            {
                // Kaster en fejl hvis user er null
                throw new Exception(nameof(user));
            }

            // Tjekker om article mangler
            if (article == null)
            {
                // Kaster en fejl hvis article er null
                throw new Exception(nameof(article));
            }

            // Returnerer om artiklen er favorit
            return _articleRepository.IsFavorite(user.Id, article.Id);
        }

        // Skifter favorit-status for en artikel
        public bool ToggleFavorite(User user, Article article)
        {
            // Tjekker om artiklen allerede er favorit
            if (IsFavorite(user, article))
            {
                // Fjerner artiklen fra favoritter
                RemoveFavorite(user, article);

                // Returnerer false fordi artiklen ikke længere er favorit
                return false;
            }

            // Tilføjer artiklen som favorit
            AddFavorite(user, article);

            // Returnerer true fordi artiklen nu er favorit
            return true;
        }

        // Mapper en ArticleDTO til et Article-objekt
        Article MapToArticle(ArticleDTO dto)
        {
            // Opretter en Article med de simple værdier fra DTO'en
            Article article = new Article
            {
                // Sætter artikelens id
                Id = dto.Id,

                // Sætter artikelens titel
                Title = dto.Title,

                // Sætter artikelens beskrivelse
                Description = dto.Description,

                // Sætter dagspris
                DailyPrice = dto.DailyPrice,

                // Sætter oprindelig pris
                OriginalPrice = dto.OriginalPrice,

                // Sætter om artiklen er udlejet
                IsRented = dto.IsRented,

                // Sætter om artiklen er ren
                IsClean = dto.IsClean,

                // Sætter hvornår artiklen blev oprettet
                CreatedAt = dto.CreatedAt,

                // Sætter hvornår artiklen blev opdateret
                UpdatedAt = dto.UpdatedAt
            };

            // Tjekker om DTO'en har et brand-id
            if (dto.BrandId.HasValue)
            {
                // Henter brandet ud fra id og sætter det på artiklen
                article.Brand = _brandServices.GetById(dto.BrandId.Value);
            }

            // Tjekker om DTO'en har et kategori-id
            if (dto.CategoryId.HasValue)
            {
                // Henter kategorien ud fra id og sætter den på artiklen
                article.Category = _categoryServices.GetCategoryById(dto.CategoryId.Value);
            }

            // Tjekker om DTO'en har et underkategori-id
            if (dto.SubcategoryId.HasValue)
            {
                // Henter underkategorien ud fra id og sætter den på artiklen
                article.SubCategory = _categoryServices.GetSubCategoryById(dto.SubcategoryId.Value);
            }

            //if (dto.CollectionId.HasValue)
            //{
            //	article.collection = _collectionServices.GetById(dto.CollectionId.Value);
            //}

            // Tjekker om DTO'en har et farve-id
            if (dto.ColorId.HasValue)
            {
                // Henter farven ud fra id og sætter den på artiklen
                article.Color = _colorServices.GetById(dto.ColorId.Value);
            }

            // Tjekker om DTO'en har et størrelse-id
            if (dto.SizeId.HasValue)
            {
                // Henter størrelsen ud fra id og sætter den på artiklen
                article.Size = _sizeServices.GetById(dto.SizeId.Value);
            }

            // Tjekker om DTO'en har et ejer-id
            if (dto.OwnerId.HasValue)
            {
                // Henter ejeren ud fra id og sætter den på artiklen
                article.Owner = _userServices.GetById(dto.OwnerId.Value);
            }

            // SKAL BRUGES TIL CONTENT BASES ALGORITHM, FOR AT ALLE FÅR DET PÅ!
            // Tjekker om artiklen har en kategori
            if (article.Category != null)
            {
                // Opretter en ItemProfile til content-based algoritmen
                article.ItemProfile = new ItemProfile
                {
                    // Sætter artikelens id i profilen
                    ArticleID = article.Id,

                    // Sætter selve artiklen i profilen
                    Article = article,

                    // Opretter features til algoritmen
                    Features = new Dictionary<string, double>
                    {
                        // Bruger kategoriens navn som feature med vægten 1.0
						{ article.Category.Name, 1.0 }
                    }
                };
            }

            // Returnerer den færdige Article-model
            return article;
        }
    }
}