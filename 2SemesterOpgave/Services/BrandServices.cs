using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class BrandServices
	{

		BrandRepository _brandRepository;

		Dictionary<int, Brand>? _cache;
		
		public BrandServices(BrandRepository brandRepository) 
		{ 
			_brandRepository = brandRepository;
		}


		void Load()
		{
			IEnumerable<BrandDTO> dtos = _brandRepository.GetAll();
			_cache = new Dictionary<int, Brand>();
			foreach (BrandDTO dto in dtos)
			{
				_cache[dto.Id] = MapBrand(dto);
			}
		}

		public Brand? GetById(int id)
		{
			if (_cache == null)
			{
				Load();
			}
			Brand? brand;
			bool found = _cache.TryGetValue(id, out brand);
			if(!found) 
			{
				return null;
			}
			return brand;
		}

		public IEnumerable<Brand> GetAllBrands()
		{
			if (_cache == null)
			{
				Load();
			}
			return new List<Brand>(_cache.Values);
		}

		Brand MapBrand(BrandDTO dto)
		{
			return new Brand
			{
				Id = dto.Id,
				Name = dto.Name,
				LogoPath = dto.LogoPath ?? string.Empty,
				CreatedAt = dto.CreatedAt,
				UpdatedAt = dto.UpdatedAt
			};
		}
	}
}
