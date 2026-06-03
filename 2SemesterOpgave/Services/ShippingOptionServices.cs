using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class ShippingOptionServices
	{
		ShippingOptionRepository _repository;
		Dictionary<int, ShippingOption> _cache = new();

		public ShippingOptionServices(ShippingOptionRepository repository)
		{
			_repository = repository;
			LoadCache();
		}

		void LoadCache()
		{
			foreach (ShippingOptionDTO dto in _repository.GetAll())
			{
				_cache[dto.Id] = Map(dto);
			}
		}

		public List<ShippingOption> GetAll()
		{
			return new List<ShippingOption>(_cache.Values);
		}

		public ShippingOption? GetById(int id)
		{
			_cache.TryGetValue(id, out ShippingOption? option);
			return option;
		}

		ShippingOption Map(ShippingOptionDTO dto)
		{
			return new ShippingOption
			{
				Id = dto.Id,
				Name = dto.Name,
				BaseFee = dto.BaseFee,
				DeliveryTimeDays = (byte)dto.DeliveryTimeDays
				// DeliveryDays findes ikke i databasen, så den forbliver en tom liste
			};
		}
	}
}
