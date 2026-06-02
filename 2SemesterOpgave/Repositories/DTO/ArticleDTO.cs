using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class ArticleDTO
	{
		public int Id { get; set; }

		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public float DailyPrice { get; set; }
		public float OriginalPrice { get; set; }

		public int? CategoryId { get; set; }
		public int? SubcategoryId { get; set; }

		public int? BrandId { get; set; }
		public int? CollectionId { get; set; }

		public int? ColorId { get; set; }
		public int? SizeId { get; set; }
		
		public bool IsRented { get; set; }
		public bool IsSmoked { get; set; }
		public bool IsAnimal { get; set; }
		public bool IsClean { get; set; }

		public int? OwnerId { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		//public ArticleDTO(int id, string title, string description, float dailyPrice, float originalPrice, int categoryId, int subcategoryId, int brandId, int collectionId, int colorId, int sizeId, bool isRented, bool isSmoked, bool isAnimal, bool isClean, int ownerId, string createdAt, string updatedAt) 
		//{
		//	Id = id; 
		//	Title = title; 
		//	Description = description; 
		//	DailyPrice = dailyPrice; 
		//	OriginalPrice = originalPrice;	
		//	CategoryId = categoryId;	
		//	SubcategoryId = subcategoryId; 
		//	BrandId = brandId; 
		//	CollectionId = collectionId; 
		//	ColorId = colorId; 
		//	SizeId = sizeId; 
		//	IsRented = isRented;
		//	IsSmoked = isSmoked;
		//	IsAnimal = isAnimal;
		//	IsClean = isClean;
		//	OwnerId = ownerId;
		//	CreatedAt = DateTime.ParseExact(createdAt, "yyyy-MM-dd HH:mm:ss", null);
		//	UpdatedAt = DateTime.ParseExact(updatedAt, "yyyy-MM-dd HH:mm:ss", null);
		//}

	}
}
