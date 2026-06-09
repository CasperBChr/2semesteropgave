using System.Collections.Generic;
using _2SemesterOpgave.Models; // Giver adgang til Review-modellen

namespace _2SemesterOpgave.Repositories.Interfaces
{
    public interface IReviewRepository // Interface der bestemmer hvilke metoder et ReviewRepository skal have
    {
        IEnumerable<Review> GetAll(); // Metode der skal hente alle reviews

        IEnumerable<Review> GetReviewsByRevieweeId(int userId); // Metode der skal hente reviews for en bruger, som er blevet vurderet

        IEnumerable<Review> GetReviewsByReviewerId(int userId); // Metode der skal hente reviews skrevet af en bestemt bruger

        void CreateReview(Review review); // Metode der skal oprette/gemme et nyt review
    }
}