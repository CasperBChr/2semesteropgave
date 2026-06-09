using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class RentalServices
	{
		RentalRepository _rentalRepository;
		UserServices _userServices;
		ArticleServices _articleServices;
		ShippingOptionServices _shippingOptionServices;
		InsuranceOptionServices _insuranceOptionServices;

		public RentalServices(RentalRepository rentalRepository, UserServices userServices, ArticleServices articleServices, ShippingOptionServices shippingOptionServices, InsuranceOptionServices insuranceOptionServices)
		{
			_rentalRepository = rentalRepository;
			_userServices = userServices;
			_articleServices = articleServices;
			_shippingOptionServices = shippingOptionServices;
			_insuranceOptionServices = insuranceOptionServices;
		}

		public ObservableCollection<Rental> GetByRenter(User renter)
		{
			IEnumerable<RentalDTO> dtos = _rentalRepository.GetByRenterId(renter.Id);
			return MapMany(dtos);
		}

		public ObservableCollection<Rental> GetByRentee(User rentee)
		{
			IEnumerable<RentalDTO> dtos = _rentalRepository.GetByRenteeId(rentee.Id);
			return MapMany(dtos);
		}

		public ObservableCollection<Rental> GetAll()
		{
			IEnumerable<RentalDTO> dtos = _rentalRepository.GetAll();
			return MapMany(dtos);
		}

		public void CreateRental(Rental rental)
		{
			RentalDTO dto = new RentalDTO
			{
				StartDate = rental.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
				EndDate = rental.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
				TotalPrice = (float)rental.TotalPrice,
				IsAccepted = false,
				Status = "active",
				RenterId = rental.Renter.Id,
				RenteeId = rental.Rentee.Id,
				ArticleId = rental.Article.Id,
				ShippingOptionId = rental.ShippingChoice?.Id,
				InsuranceOptionId = rental.InsuranceChoice?.Id
			};

			_rentalRepository.Create(dto);
		}

		public void AcceptRental(Rental rental)
		{
			_rentalRepository.SetAccepted(rental.Id, true);
			rental.IsAccepted = true;
		}

		public void UpdateStatus(Rental rental, string status)
		{
			_rentalRepository.UpdateStatus(rental.Id, status);
			rental.Status = status;
		}

		ObservableCollection<Rental> MapMany(IEnumerable<RentalDTO> dtos)
		{
			ObservableCollection<Rental> rentals = new();

			foreach (RentalDTO dto in dtos)
			{
				Rental? rental = MapToRental(dto);
				if (rental != null)
				{
					rentals.Add(rental);
				}
			}

			return rentals;
		}

		public HashSet<DateOnly> GetBookedDatesForArticle(int articleId)
		{
			HashSet<DateOnly> booked = new();
			foreach (var (start, end) in _rentalRepository.GetBookedDateRangesForArticle(articleId))
			{
				for (DateOnly d = start; d <= end; d = d.AddDays(1))
					booked.Add(d);
			}
			return booked;
		}


		Rental? MapToRental(RentalDTO dto)
		{
			User? renter = _userServices.GetById(dto.RenterId);
			User? rentee = _userServices.GetById(dto.RenteeId);

			Article? article = new List<Article>(_articleServices.GetAllArticles()).FirstOrDefault(article => article.Id == dto.ArticleId);

			ShippingOption? shipping = dto.ShippingOptionId.HasValue ? _shippingOptionServices.GetById(dto.ShippingOptionId.Value) : null;

			InsuranceOption? insurance = dto.InsuranceOptionId.HasValue ? _insuranceOptionServices.GetById(dto.InsuranceOptionId.Value) : null;

			if (renter == null || rentee == null || article == null) return null;

			if (!DateOnly.TryParseExact(dto.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate))
			{
				return null;
			}
			if (!DateOnly.TryParseExact(dto.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate))
			{
				return null;
			}

			return new Rental
			{
				Id = dto.Id,
				StartDate = DateOnly.Parse(dto.StartDate),
				EndDate = DateOnly.Parse(dto.EndDate),
				TotalPrice = (decimal)dto.TotalPrice,
				IsAccepted = dto.IsAccepted,
				Status = dto.Status,
				Renter = renter,
				Rentee = rentee,
				Article = article,
				CreatedAt = dto.CreatedAt,
				ShippingChoice = shipping,
				InsuranceChoice = insurance
			};
		}
	}
}
