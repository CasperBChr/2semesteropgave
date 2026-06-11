using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class ArticleDTO
	{
        //DTO property for Article
        public int Id { get; set; }

		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public float DailyPrice { get; set; }
		public float OriginalPrice { get; set; }

		public int? CategoryId { get; set; }
		public int? SubcategoryId { get; set; }

		public int? BrandId { get; set; }

		public int? ColorId { get; set; }
		public int? SizeId { get; set; }
		
		public bool IsRented { get; set; }
		public bool IsClean { get; set; }

		public int? OwnerId { get; set; }

		public bool IsFavorite { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

	}
}
