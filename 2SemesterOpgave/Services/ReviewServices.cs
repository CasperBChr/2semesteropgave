using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
    public class ReviewServices
    {
        ReviewRepository _reviewRepository;
		UserServices _userServices;
		Dictionary<int, Review> _cache = new Dictionary<int, Review>();

        public int? TargetRevieweeId { get; private set; }
        public string TargetRevieweeUsername { get; private set; } = string.Empty;
        public int? TargetRentalId { get; private set; }

        public ReviewServices(ReviewRepository reviewRepository, UserServices userServices)
        {
            _reviewRepository = reviewRepository;
            _userServices = userServices;

			LoadCache();
        }

		private Review Map(ReviewDTO dto)
		{
			return new Review
			{
				Id = dto.Id,
				Rating = dto.Rating,
				Comment = dto.Comment,
				RentalId = dto.RentalId,
				CreatedAt = dto.CreatedAt,
				ReviewerId = dto.ReviewerId,
				RevieweeId = dto.RevieweeId,
				Reviewer = _userServices.GetById(dto.ReviewerId),
				Reviewee = _userServices.GetById(dto.RevieweeId)
			};
		}

		private void LoadCache()
        {
            _cache.Clear();
            List<ReviewDTO> reviews = new List<ReviewDTO>(_reviewRepository.GetAll());

			foreach (ReviewDTO dto in reviews)
			{
				Review review = Map(dto);
				_cache[review.Id] = review;
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
			ObservableCollection<Review> reviews = new ObservableCollection<Review>();
			List<int> keys = new List<int>(_cache.Keys);

			for (int i = 0; i < keys.Count; i++)
			{
				if (_cache[keys[i]].RevieweeId == userId)
				{
					reviews.Add(_cache[keys[i]]);
				}
			}
			return reviews;
		}

		public ObservableCollection<Review> GetReviewsByReviewerId(int userId)
		{
			ObservableCollection<Review> reviews = new ObservableCollection<Review>();
			List<int> keys = new List<int>(_cache.Keys);

			for (int i = 0; i < keys.Count; i++)
			{
				if (_cache[keys[i]].ReviewerId == userId)
				{
					reviews.Add(_cache[keys[i]]);
				}
			}
			return reviews;
		}

		public float GetAverageRating(int revieweeId)
        {
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
            float total = 0; // her plusser vi alle ratings sammen
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
