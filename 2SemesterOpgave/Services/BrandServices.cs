using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;

namespace _2SemesterOpgave.Services
{
	public class BrandServices
	{

		BrandRepository _brandRepository;
		
		public BrandServices(BrandRepository brandRepository) 
		{ 
			_brandRepository = brandRepository;
		}

		public List<Brand> GetAllBrands() 
		{
			return new List<Brand>();
		}
	}
}
