using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class DesignerServices
	{
		DesignerRepository _designerRepository;

		Dictionary<int, Designer> _cache = new();

		public DesignerServices(DesignerRepository designerRepository)
		{
			_designerRepository = designerRepository;

			// preload cache én gang
			LoadCache();
		}

		void LoadCache()
		{
			foreach (DesignerDTO dto in _designerRepository.GetAllDesigners())
			{
				_cache[dto.Id] = Map(dto);
			}
		}

		public List<Designer> GetAllDesigners()
		{
			return new List<Designer>(_cache.Values);
		}

		public Designer? GetById(int id)
		{
			Designer? designer;
			bool found = _cache.TryGetValue(id, out designer);
			if(found) 
			{ 
				return designer; 
			}
			return null;
		}

		Designer Map(DesignerDTO dto)
		{
			return new Designer()
			{
				Id = dto.Id,
				Name = dto.Name,
				CreatedAt = dto.CreatedAt,
				UpdatedAt = dto.UpdatedAt,
			};
		}
	}
}
