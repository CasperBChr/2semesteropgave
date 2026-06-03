using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;

namespace _2SemesterOpgave.Services
{
    public class ReviewServices
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly Dictionary<int, Review> _cache = new();

        public int? TargetRevieweeId { get; private set; }
        public string TargetRevieweeUsername { get; private set; } = string.Empty;
        public int? TargetRentalId { get; private set; }

        public ReviewServices(Database db)
        {
            _reviewRepository = new ReviewRepository(db);
            LoadCache();
        }

        private void LoadCache()
        {
            _cache.Clear();

            foreach (Review review in _reviewRepository.GetAll())
            {
                _cache[review.Id] = review;
            }
        }

        public void SetReviewTarget(User reviewee, int? rentalId = null)
        {
            TargetRevieweeId = reviewee.Id;
            TargetRevieweeUsername = reviewee.Username ?? string.Empty;
            TargetRentalId = rentalId;
        }

        public void ClearReviewTarget()
        {
            TargetRevieweeId = null;
            TargetRevieweeUsername = string.Empty;
            TargetRentalId = null;
        }

        public List<Review> GetAll()
        {
            return new List<Review>(_cache.Values);
        }

        public ObservableCollection<Review> GetReviewsByRevieweeId(int userId)
        {
            List<Review> reviews = _cache.Values
                .Where(r => r.RevieweeId == userId)
                .ToList();

            return new ObservableCollection<Review>(reviews);
        }

        public ObservableCollection<Review> GetReviewsByReviewerId(int userId)
        {
            List<Review> reviews = _cache.Values
                .Where(r => r.ReviewerId == userId)
                .ToList();

            return new ObservableCollection<Review>(reviews);
        }

        public float GetAverageRating(int revieweeId)
        {
            List<Review> reviews = _cache.Values
                .Where(r => r.RevieweeId == revieweeId)
                .ToList();

            if (reviews.Count == 0)
            {
                return 0;
            }

            return reviews.Average(r => (float)r.Rating);
        }

        public void CreateReview(Review review)
        {
            _reviewRepository.CreateReview(review);
            LoadCache();
        }
    }
}
