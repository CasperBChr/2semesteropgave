using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class CollectionServices
	{
		CollectionRepository _collectionRepository;
		BrandServices _brandServices;
		DesignerServices _designerServices;
		Dictionary<int, Collection> _collections = new Dictionary<int, Collection>();
		public CollectionServices(CollectionRepository collectionRepository, BrandServices brandServices, DesignerServices designerServices)
		{
			_collectionRepository = collectionRepository;
			_brandServices = brandServices;
			_designerServices = designerServices;
				
			LoadCache();
		}

		void LoadCache()
		{
			IEnumerable<CollectionDTO> dtos = _collectionRepository.GetAllCollections();

			foreach (CollectionDTO dto in dtos)
			{
				Collection collection = new Collection
				{
					Id = dto.Id,
					Name = dto.Name,
					CreatedAt = dto.CreatedAt
				};

				if (dto.BrandId.HasValue)
				{
					collection.Brand = _brandServices.GetById(dto.BrandId.Value);
				}

				if (dto.DesignerId.HasValue)
				{
					collection.Designer = _designerServices.GetById(dto.DesignerId.Value);
				}

				_collections[collection.Id] = collection;
			}
		}

		public Collection? GetById(int id)
		{
			_collections.TryGetValue(id, out Collection? collection);
			return collection;
		}

		public IEnumerable<Collection> GetAll()
		{
			return _collections.Values;
		}
	}
}
