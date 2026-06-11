using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	// DTO-klasse der bruges til at transportere kategori-data fra databasen
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class CategoryDTO
    {
        // Kategoriens id i databasen
        public int Id { get; set; }

        // Kategoriens navn
        public string Name { get; set; } = string.Empty;

        // Id på underkategori, hvis der findes en
        public int? SubId { get; set; }

        // Navn på underkategori, hvis der findes en
        public string? SubName { get; set; }
    }
}
