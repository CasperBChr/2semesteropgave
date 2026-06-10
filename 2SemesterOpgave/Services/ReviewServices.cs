using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls; // Giver adgang til WPF controls, men bruges ikke direkte i denne fil
using _2SemesterOpgave.Data; // Giver adgang til Database, men bruges ikke direkte i denne version
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User og Review
using _2SemesterOpgave.Repositories; // Giver adgang til ReviewRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ReviewDTO, som bruges til data fra repository

namespace _2SemesterOpgave.Services
{
    public class ReviewServices // Serviceklasse der håndterer logik omkring anmeldelser/reviews
    {
        ReviewRepository _reviewRepository; // Repository der bruges til at hente og gemme reviews i databasen
        UserServices _userServices; // Service der bruges til at hente brugere ud fra deres id
        Dictionary<int, Review> _cache = new Dictionary<int, Review>(); // Cache der gemmer reviews i memory med reviewets id som key

        public int? TargetRevieweeId { get; private set; } // Id på den bruger der skal vurderes
        public string TargetRevieweeUsername { get; private set; } = string.Empty; // Brugernavn på den bruger der skal vurderes
        public int? TargetRentalId { get; private set; } // Id på den lejeaftale reviewet eventuelt hører til

        public ReviewServices(ReviewRepository reviewRepository, UserServices userServices) // Constructor der modtager review repository og user service
        {
            _reviewRepository = reviewRepository; // Gemmer repository, så servicen kan hente og gemme reviews
            _userServices = userServices; // Gemmer user service, så servicen kan koble brugere på reviews

            LoadCache(); // Henter alle reviews fra repository og gemmer dem i cachen
        }

        private Review Map(ReviewDTO dto) // Omdanner en ReviewDTO fra databasen til et Review-objekt
        {
            return new Review
            {
                Id = dto.Id, // Sætter reviewets id
                Rating = dto.Rating, // Sætter reviewets rating
                Comment = dto.Comment, // Sætter reviewets kommentar
                RentalId = dto.RentalId, // Sætter id på lejeaftalen, hvis der er en
                CreatedAt = dto.CreatedAt, // Sætter tidspunktet hvor reviewet blev oprettet
                ReviewerId = dto.ReviewerId, // Sætter id på brugeren der skrev reviewet
                RevieweeId = dto.RevieweeId, // Sætter id på brugeren der blev vurderet
                Reviewer = _userServices.GetById(dto.ReviewerId), // Henter brugeren der skrev reviewet
                Reviewee = _userServices.GetById(dto.RevieweeId) // Henter brugeren der blev vurderet
            };
        }

        private void LoadCache() // Henter alle reviews og opdaterer cachen
        {
            _cache.Clear(); // Tømmer cachen, så gamle data ikke ligger dobbelt
            List<ReviewDTO> reviews = new List<ReviewDTO>(_reviewRepository.GetAll()); // Henter alle reviews fra repository som DTO'er

            foreach (ReviewDTO dto in reviews) // Gennemgår alle DTO'er fra databasen
            {
                Review review = Map(dto); // Omdanner DTO'en til et Review-objekt
                _cache[review.Id] = review; // Gemmer reviewet i cachen med reviewets id som key
            }
        }

        public void SetReviewTarget(User reviewee, int? rentalId = null) // Sætter hvilken bruger der skal vurderes
        {
            TargetRevieweeId = reviewee.Id; // Gemmer id'et på brugeren der skal vurderes
            TargetRevieweeUsername = reviewee.Username; // Gemmer brugernavnet på brugeren der skal vurderes
            TargetRentalId = rentalId; // Gemmer rental id, hvis reviewet hører til en bestemt lejeaftale
        }

        public void ClearReviewTarget() // Nulstiller den valgte review-modtager
        {
            TargetRevieweeId = null; // Fjerner id'et på brugeren der skulle vurderes
            TargetRevieweeUsername = string.Empty; // Nulstiller brugernavnet til tom tekst
            TargetRentalId = null; // Fjerner rental id
        }

        public List<Review> GetAll() // Returnerer alle reviews fra cachen
        {
            return new List<Review>(_cache.Values); // Laver en ny liste med alle review-objekter fra cachen
        }

        public ObservableCollection<Review> GetReviewsByRevieweeId(int userId) // Henter reviews som en bestemt bruger har modtaget
        {
            ObservableCollection<Review> reviews = new ObservableCollection<Review>(); // Opretter en samling til de reviews der skal vises i UI
            List<int> keys = new List<int>(_cache.Keys); // Laver en liste med alle keys fra cachen

            for (int i = 0; i < keys.Count; i++) // Looper igennem alle keys i cachen
            {
                if (_cache[keys[i]].RevieweeId == userId) // Tjekker om reviewet er til den valgte bruger
                {
                    reviews.Add(_cache[keys[i]]); // Tilføjer reviewet til listen
                }
            }

            return reviews; // Returnerer alle reviews brugeren har modtaget
        }

        public ObservableCollection<Review> GetReviewsByReviewerId(int userId) // Henter reviews som en bestemt bruger har skrevet
        {
            ObservableCollection<Review> reviews = new ObservableCollection<Review>(); // Opretter en samling til de reviews der skal vises i UI
            List<int> keys = new List<int>(_cache.Keys); // Laver en liste med alle keys fra cachen

            for (int i = 0; i < keys.Count; i++) // Looper igennem alle keys i cachen
            {
                if (_cache[keys[i]].ReviewerId == userId) // Tjekker om reviewet er skrevet af den valgte bruger
                {
                    reviews.Add(_cache[keys[i]]); // Tilføjer reviewet til listen
                }
            }

            return reviews; // Returnerer alle reviews brugeren har skrevet
        }

        public float GetAverageRating(int revieweeId) // Udregner gennemsnitsratingen for en bestemt bruger
        {
            List<Review> reviews = new List<Review>(); // Liste til de reviews som brugeren har modtaget
            List<int> integers = new List<int>(_cache.Keys); // Laver en liste med alle keys fra cachen

            for (int i = 0; i < _cache.Count; i++) // Looper igennem alle reviews i cachen
            {
                if (_cache[integers[i]].RevieweeId == revieweeId) // Tjekker om reviewet er til den bruger vi vil finde gennemsnittet for
                {
                    reviews.Add(_cache[integers[i]]); // Tilføjer reviewet til listen
                }
            }

            if (reviews.Count == 0) // Tjekker om brugeren ikke har nogen reviews
            {
                return 0; // Returnerer 0 hvis der ikke er nogen reviews at beregne gennemsnit ud fra
            }

            //return reviews.Average(r => (float)r.Rating); 

            // returnerer gennemsnittet af alle reviews - vores egen funktion
            return CalculateAverage(reviews);
        }

        public float CalculateAverage(List<Review> reviews) // Beregner gennemsnittet af en liste med reviews
        {
            float total = 0; // her plusser vi alle ratings sammen
            float average = 0.0f; // her bliver gennemsnittet sat

            for (int i = 0; i < reviews.Count; i++) // vi looper igennem alle reviews
            {
                total += reviews[i].Rating; // Hver reviews rating bliver lagt til totalen
            }

            average = total / reviews.Count; // gennemsnittet er nu udregnet
            return average; // returner float med gennemsnittet
        }

        public void CreateReview(Review review) // Opretter et nyt review
        {
            _reviewRepository.CreateReview(review); // Gemmer reviewet i databasen gennem repository
            LoadCache(); // Opdaterer cachen, så det nye review også kommer med
        }
    }
}