using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class SizeServices
	{
		SizeRepository _sizeRepository;
		Dictionary<int, Size> _cache = new Dictionary<int, Size>();

		public SizeServices(SizeRepository sizeRepository)
		{
			_sizeRepository = sizeRepository;

			LoadCache();
		}

		void LoadCache()
		{
			foreach (SizeDTO dto in _sizeRepository.GetAllSizes())
			{
				_cache[dto.Id] = Map(dto);
			}
		}

		public List<Size> GetAllSizes()
		{
			return new List<Size>(_cache.Values);
		}

		public Size? GetById(int id)
		{
			Size? size;

			bool found = _cache.TryGetValue(id, out size);
			if (found) { return size; }
			return null;
		}

		private Size Map(SizeDTO dto)
		{
			return new Size
			{
				Id = dto.Id,
				Name = dto.Name
			};
		}
	}
}
