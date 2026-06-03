using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class ReviewServices
	{
		ReviewRepository _repository;
		UserServices _userServices;
		Dictionary<int, Review> _cache = new();

		public ReviewServices(ReviewRepository repository, UserServices userServices)
		{
			_repository = repository;
			_userServices = userServices;
			LoadCache();
		}

		void LoadCache()
		{
			foreach (ReviewDTO dto in _repository.GetAll())
			{
				_cache[dto.Id] = Map(dto);
			}
		}

		public List<Review> GetAll()
		{
			return new List<Review>(_cache.Values);
		}

		public float GetAverageRating(int revieweeId)
		{
			List<Review> reviews = GetByReviewee(revieweeId);

			if (reviews.Count == 0)
			{
				return 0;
			}

			return reviews.Average(r => (float)r.Rating);
		}

		public List<Review> GetByReviewee(int revieweeId)
		{
			return _cache.Values.Where(r => r.Reviewee?.Id == revieweeId).ToList();
		}

		public void Create(Review review)
		{
			ReviewDTO dto = new ReviewDTO
			{
				Rating = review.Rating,
				Comment = review.Comment,
				RentalId = review.RentalId,
				ReviewerId = review.Reviewer?.Id ?? 0,
				RevieweeId = review.Reviewee?.Id ?? 0
			};

			_repository.Create(dto);
			LoadCache();
		}

		Review Map(ReviewDTO dto)
		{
			return new Review
			{
				Id = dto.Id,
				Rating = dto.Rating,
				Comment = dto.Comment,
				CreatedAt = dto.CreatedAt,
				RentalId = dto.RentalId,
				Reviewer = _userServices.GetById(dto.ReviewerId),
				Reviewee = _userServices.GetById(dto.RevieweeId)
			};
		}
	}
}
