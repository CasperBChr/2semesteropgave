using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.Interfaces;

namespace _2SemesterOpgave.Services
{
    public class ReviewServices
    {
        private IReviewRepository _reviewRepository;
        private Dictionary<int, Review> _cache = new Dictionary<int, Review>();

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
            List<Review> reviews = new List<Review>(_reviewRepository.GetAll());

			for (int i = 0; i < reviews.Count; i++)
            {
				_cache[reviews[i].Id] = reviews[i];
			}
        }

        public void SetReviewTarget(User reviewee, int? rentalId = null)
        {
            TargetRevieweeId = reviewee.Id;
            TargetRevieweeUsername = reviewee.Username;
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
			//List<Review> reviews = _cache.Values.Where(r => r.RevieweeId == userId).ToList();
			ObservableCollection<Review> reviews = new ObservableCollection<Review>();
            List<int> integers = new List<int>(_cache.Keys);

            for(int i = 0; i < _cache.Count; i++)
            {
                if(_cache[integers[i]].RevieweeId == userId)
                {
                    reviews.Add(_cache[integers[i]]);
                }
            }
			//return new ObservableCollection<Review>(reviews);
			return reviews;
		}

        public ObservableCollection<Review> GetReviewsByReviewerId(int userId)
        {
            //List<Review> reviews = _cache.Values.Where(r => r.ReviewerId == userId).ToList();
			ObservableCollection<Review> reviews = new ObservableCollection<Review>();
			List<int> integers = new List<int>(_cache.Keys);

			for (int i = 0; i < _cache.Count; i++)
			{
				if (_cache[integers[i]].ReviewerId == userId)
				{
					reviews.Add(_cache[integers[i]]);
				}
			}
			//return new ObservableCollection<Review>(reviews);
			return reviews;
        }

        public float GetAverageRating(int revieweeId)
        {
            //List<Review> reviews = _cache.Values.Where(r => r.RevieweeId == revieweeId).ToList();

			List<Review> reviews = new List<Review>();
			List<int> integers = new List<int>(_cache.Keys);

			for (int i = 0; i < _cache.Count; i++)
			{
				if (_cache[integers[i]].RevieweeId == revieweeId)
				{
					reviews.Add(_cache[integers[i]]);
				}
			}
			if (reviews.Count == 0)
            {
                return 0;
            }
			//return reviews.Average(r => (float)r.Rating); 

			// returnerer gennemsnittet af alle reviews - vores egen funktion
			return CalculateAverage(reviews); 
		}

        public float CalculateAverage(List<Review> reviews)
        {
            int total = 0; // her plusser vi alle ratings sammen
            float average = 0.0f; // her bliver gennemsnittet sat
            for(int i = 0; i < reviews.Count; i++) // vi looper igennem alle reviews
            {
                total += reviews[i].Rating; // Hver reviews rating bliver lagt til totalen
            }
            average = total / reviews.Count; // gennemsnittet er nu udregnet
            return average; // returner float med gennemsnittet
        }

        public void CreateReview(Review review)
        {
            _reviewRepository.CreateReview(review);
            LoadCache();
        }
    }
}
