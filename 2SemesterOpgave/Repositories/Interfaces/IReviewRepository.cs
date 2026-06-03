using System.Collections.Generic;
using _2SemesterOpgave.Models;

namespace _2SemesterOpgave.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetAll();

        IEnumerable<Review> GetReviewsByRevieweeId(int userId);

        IEnumerable<Review> GetReviewsByReviewerId(int userId);

        void CreateReview(Review review);
    }
}