using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class InsuranceOptionServices
	{
		InsuranceOptionRepository _repository;
		Dictionary<int, InsuranceOption> _cache = new();

		public InsuranceOptionServices(InsuranceOptionRepository repository)
		{
			_repository = repository;
			LoadCache();
		}

		void LoadCache()
		{
			foreach (InsuranceOptionDTO dto in _repository.GetAll())
			{
				_cache[dto.Id] = Map(dto);
			}
		}

		public List<InsuranceOption> GetAll()
		{
			return new List<InsuranceOption>(_cache.Values);
		}

		public InsuranceOption? GetById(int id)
		{
			_cache.TryGetValue(id, out InsuranceOption? option);
			return option;
		}

		InsuranceOption Map(InsuranceOptionDTO dto)
		{
			return new InsuranceOption
			{
				Id = dto.Id,
				Name = dto.Name,
				BaseFees = dto.BaseFees
			};
		}
	}
}
