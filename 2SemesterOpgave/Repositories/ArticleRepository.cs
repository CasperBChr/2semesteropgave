using _2SemesterOpgave;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Article og User
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ArticleDTO
using _2SemesterOpgave.Services; // Giver adgang til serviceklasser
using Microsoft.Data.Sqlite; // Giver adgang til SQLite
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;

namespace _2SemesterOpgave.Repositories
{
    // Repositoryklasse der håndterer databasekald for artikler
    public class ArticleRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public ArticleRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Sletter en artikel fra databasen
        public void DeleteArticle(Article article)
        {
            // Forsøger at slette artiklen
            try
            {
                // Opretter forbindelse til databasen
                using IDbConnection connection = _db.CreateConnection();

                // Opretter en SQL-kommando på forbindelsen
                using IDbCommand command = connection.CreateCommand();

                // SQL der sletter artiklen ud fra dens id
                command.CommandText = "DELETE FROM Articles WHERE id = @ArticleId;";

                // Opretter parameter til artikelens id
                IDbDataParameter articleParam = command.CreateParameter();

                // Navnet på parameteren i SQL'en
                articleParam.ParameterName = "@ArticleId";

                // Sætter parameterens værdi til artikelens id
                articleParam.Value = article.Id;

                // Tilføjer parameteren til kommandoen
                command.Parameters.Add(articleParam);

                // Kører SQL-kommandoen
                command.ExecuteNonQuery();
            }
            // Fanger fejl hvis sletning går galt
            catch (Exception ex)
            {
                // Skriver fejlen i debug output
                Debug.WriteLine(ex);
            }
        }

        // Opdaterer en eksisterende artikel i databasen
        public void UpdateArticle(Article article)
        {
            // Forsøger at opdatere artiklen
            try
            {
                // Opretter forbindelse til databasen
                using IDbConnection connection = _db.CreateConnection();

                // Opretter en SQL-kommando på forbindelsen
                using IDbCommand command = connection.CreateCommand();

                // SQL der opdaterer artikelens data ud fra artikelens id
                command.CommandText = @"
					UPDATE Articles SET
					name = @name, description = @description, category_id = @categoryId, subcategory_id = @subcategoryId, brand_id = @brandId, color_id = @colorId, size_id = @sizeId, daily_price = @dailyPrice, original_price = @originalPrice, is_rented = @isRented 
					WHERE id = @ArticleId;";

                // Opretter parameter til artikelens id
                IDbDataParameter articleParam = command.CreateParameter();
                articleParam.ParameterName = "@ArticleId";
                articleParam.Value = article.Id;
                command.Parameters.Add(articleParam);

                // Opretter parameter til artikelens navn/titel
                IDbDataParameter nameParam = command.CreateParameter();
                nameParam.ParameterName = "@name";
                nameParam.Value = article.Title;
                command.Parameters.Add(nameParam);

                // Opretter parameter til artikelens beskrivelse
                IDbDataParameter descriptionParam = command.CreateParameter();
                descriptionParam.ParameterName = "@description";
                descriptionParam.Value = article.Description;
                command.Parameters.Add(descriptionParam);

                // Opretter parameter til kategori-id
                IDbDataParameter categoryParam = command.CreateParameter();
                categoryParam.ParameterName = "@categoryId";
                categoryParam.Value = article.Category.Id;
                command.Parameters.Add(categoryParam);

                // Opretter parameter til underkategori-id
                IDbDataParameter subcategoryParam = command.CreateParameter();
                subcategoryParam.ParameterName = "@subcategoryId";
                subcategoryParam.Value = article.SubCategory.Id;
                command.Parameters.Add(subcategoryParam);

                // Opretter parameter til brand-id
                IDbDataParameter brandParam = command.CreateParameter();
                brandParam.ParameterName = "@brandId";
                brandParam.Value = article.Brand.Id;
                command.Parameters.Add(brandParam);

                // Opretter parameter til farve-id
                IDbDataParameter colorParam = command.CreateParameter();
                colorParam.ParameterName = "@colorId";
                colorParam.Value = article.Color.Id;
                command.Parameters.Add(colorParam);

                // Opretter parameter til størrelse-id
                IDbDataParameter sizeParam = command.CreateParameter();
                sizeParam.ParameterName = "@sizeId";
                sizeParam.Value = article.Size.Id;
                command.Parameters.Add(sizeParam);

                // Opretter parameter til dagspris
                IDbDataParameter dailyPriceParam = command.CreateParameter();
                dailyPriceParam.ParameterName = "@dailyPrice";
                dailyPriceParam.Value = article.DailyPrice;
                command.Parameters.Add(dailyPriceParam);

                // Opretter parameter til oprindelig pris
                IDbDataParameter originalPriceParam = command.CreateParameter();
                originalPriceParam.ParameterName = "@originalPrice";
                originalPriceParam.Value = article.OriginalPrice;
                command.Parameters.Add(originalPriceParam);

                // Opretter parameter til om artiklen er udlejet
                IDbDataParameter isRentedParam = command.CreateParameter();
                isRentedParam.ParameterName = "@isRented";
                isRentedParam.Value = article.IsRented;
                command.Parameters.Add(isRentedParam);

                // Kører SQL-kommandoen
                command.ExecuteNonQuery();
            }
            // Fanger fejl hvis opdatering går galt
            catch (Exception ex)
            {
                // Skriver fejlen i debug output
                Debug.WriteLine(ex);
            }
        }

        // Henter alle artikler fra databasen
        public IEnumerable<ArticleDTO> GetAllArticles()
        {
            // Opretter en liste til artikel-DTO'er
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle artikler
            command.CommandText = "SELECT * FROM Articles";

            // Kører kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ArticleDTO
                ArticleDTO articleDTO = CreateDTO(reader);

                // Tilføjer DTO'en til listen
                articleDTOs.Add(articleDTO);
            }

            // Lukker readeren
            reader.Close();

            // Returnerer listen med artikler
            return articleDTOs;
        }


        // Henter de nyeste artikler fra databasen
        public IEnumerable<ArticleDTO> GetNewestArticles()
        {
            // Opretter en liste til artikel-DTO'er
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter de 10 nyeste artikler
            command.CommandText = "SELECT * FROM Articles ORDER BY created_at DESC LIMIT 10";

            // Liste til artikler, men den bruges ikke direkte her
            List<Article> articles = new List<Article>();

            // Kører kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ArticleDTO
                ArticleDTO articleDTO = CreateDTO(reader);

                // Tilføjer DTO'en til listen
                articleDTOs.Add(articleDTO);
            }

            // Lukker readeren
            reader.Close();

            // Returnerer listen med de nyeste artikler
            return articleDTOs;
        }

        // Henter alle artikler der tilhører en bestemt ejer
        public IEnumerable<ArticleDTO> GetAllArticlesByOwner(int ownerId)
        {
            // Opretter en liste til artikel-DTO'er
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter artikler ud fra owner_id
            command.CommandText = "SELECT * FROM Articles WHERE owner_id = @OwnerId";

            // Opretter parameter til ejerens id
            IDbDataParameter ownerParameter = command.CreateParameter();
            ownerParameter.ParameterName = "@OwnerId";
            ownerParameter.DbType = DbType.Int32;
            ownerParameter.Value = ownerId;
            command.Parameters.Add(ownerParameter);

            // Kører kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ArticleDTO
                ArticleDTO articleDTO = CreateDTO(reader);

                // Tilføjer DTO'en til listen
                articleDTOs.Add(articleDTO);
            }

            // Lukker readeren
            reader.Close();

            // Returnerer listen med ejerens artikler
            return articleDTOs;
        }

        // Henter alle artikler som en bestemt bruger har markeret som favorit
        public IEnumerable<ArticleDTO> GetAllFavoritedArticlesByUser(int userId)
        {
            // Opretter en liste til artikel-DTO'er
            List<ArticleDTO> articleDTOs = new();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter artikler fra UserFavorites-tabellen
            command.CommandText = @"
				SELECT a.*
				FROM Articles a
				INNER JOIN UserFavorites uf
					ON uf.article_id = a.id
				WHERE uf.user_id = @UserId";

            // Opretter parameter til brugerens id
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@UserId";
            parameter.DbType = DbType.Int32;
            parameter.Value = userId;

            // Tilføjer parameteren til kommandoen
            command.Parameters.Add(parameter);

            // Kører kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en ArticleDTO og tilføjer den til listen
                articleDTOs.Add(CreateDTO(reader));
            }

            // Returnerer brugerens favoritartikler
            return articleDTOs;
        }

        // Tilføjer en artikel som favorit for en bruger
        public void AddFavorite(int userId, int articleId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter en favorit-relation mellem bruger og artikel
            command.CommandText = @"
				INSERT INTO UserFavorites(user_id, article_id)
				VALUES(@UserId, @ArticleId)";

            // Opretter parameter til brugerens id
            IDbDataParameter userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.Value = userId;
            command.Parameters.Add(userParam);

            // Opretter parameter til artikelens id
            IDbDataParameter articleParam = command.CreateParameter();
            articleParam.ParameterName = "@ArticleId";
            articleParam.Value = articleId;
            command.Parameters.Add(articleParam);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Fjerner en artikel fra en brugers favoritter
        public void RemoveFavorite(int userId, int articleId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der sletter favorit-relationen mellem bruger og artikel
            command.CommandText = @"
				DELETE FROM UserFavorites
				WHERE user_id = @UserId
				AND article_id = @ArticleId";

            // Opretter parameter til brugerens id
            IDbDataParameter userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.Value = userId;
            command.Parameters.Add(userParam);

            // Opretter parameter til artikelens id
            IDbDataParameter articleParam = command.CreateParameter();
            articleParam.ParameterName = "@ArticleId";
            articleParam.Value = articleId;
            command.Parameters.Add(articleParam);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Tjekker om en artikel er favorit for en bestemt bruger
        public bool IsFavorite(int userId, int articleId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der tæller om favoritten findes
            command.CommandText = @"
				SELECT COUNT(*)
				FROM UserFavorites
				WHERE user_id = @UserId
				AND article_id = @ArticleId";

            // Opretter parameter til brugerens id
            IDbDataParameter userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.Value = userId;
            command.Parameters.Add(userParam);

            // Opretter parameter til artikelens id
            IDbDataParameter articleParam = command.CreateParameter();
            articleParam.ParameterName = "@ArticleId";
            articleParam.Value = articleId;
            command.Parameters.Add(articleParam);

            // Returnerer true hvis der findes mindst én favorit
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }


        // Henter artikler ud fra filtrering
        public IEnumerable<ArticleDTO> GetFilteredArticles(FilterCriteria filter)
        {
            // Opretter en liste til artikel-DTO'er
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // Starter SQL'en med en betingelse der altid er sand
            StringBuilder sql = new StringBuilder("SELECT * FROM Articles WHERE 1=1");

            // Tjekker om brugeren har skrevet søgetekst
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                // Tilføjer søgning i navn og beskrivelse
                sql.Append(" AND (name LIKE @search OR description LIKE @search)");

                // Opretter søgeparameter
                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@search";
                parameter.Value = $"%{filter.SearchText}%";
                command.Parameters.Add(parameter);
            }

            // Sætter den færdige SQL-tekst på kommandoen
            command.CommandText = sql.ToString();

            // Kører kommandoen og læser resultatet
            IDataReader reader = command.ExecuteReader();

            // Liste til artikler, men den bruges ikke direkte her
            List<Article> articles = new List<Article>();

            // Looper igennem søgeresultaterne
            while (reader.Read())
            {
                // Omdanner databaserækken til en ArticleDTO
                ArticleDTO articleDTO = CreateDTO(reader);

                // Tilføjer DTO'en til listen
                articleDTOs.Add(articleDTO);
            }

            // Returnerer filtrerede artikler
            return articleDTOs;
        }

        // Opretter en ny artikel i databasen
        public void CreateArticle(Article article, User owner)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando
            using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter en ny artikel i databasen
            command.CommandText = "INSERT INTO Articles (name, description, category_id, subcategory_id, brand_id, color_id, size_id, daily_price, original_price, is_rented, owner_id) VALUES (@name, @description, @categoryId, @subcategoryId, @brandId, @colorId, @sizeId, @dailyPrice, @originalPrice, @isRented, @ownerId)";

            // Opretter parameter til artikelens titel/navn
            IDbDataParameter nameParam = command.CreateParameter();
            nameParam.ParameterName = "@name";
            nameParam.DbType = DbType.String;
            nameParam.Value = article.Title;
            command.Parameters.Add(nameParam);

            // Opretter parameter til artikelens beskrivelse
            IDbDataParameter descriptionParam = command.CreateParameter();
            descriptionParam.DbType = DbType.String;
            descriptionParam.ParameterName = "@description";
            descriptionParam.Value = article.Description;
            command.Parameters.Add(descriptionParam);

            // Opretter parameter til kategori-id
            IDbDataParameter categoryIdParam = command.CreateParameter();
            categoryIdParam.DbType = DbType.Int32;
            categoryIdParam.ParameterName = "@categoryId";
            categoryIdParam.Value = article.Category.Id;
            command.Parameters.Add(categoryIdParam);

            // Opretter parameter til underkategori-id
            IDbDataParameter subcategoryIdParam = command.CreateParameter();
            subcategoryIdParam.DbType = DbType.Int32;
            subcategoryIdParam.ParameterName = "@subcategoryId";
            subcategoryIdParam.Value = article.SubCategory.Id;
            command.Parameters.Add(subcategoryIdParam);

            // Opretter parameter til brand-id
            IDbDataParameter brandIdParam = command.CreateParameter();
            brandIdParam.DbType = DbType.Int32;
            brandIdParam.ParameterName = "@brandId";
            brandIdParam.Value = article.Brand.Id;
            command.Parameters.Add(brandIdParam);

            // Opretter parameter til farve-id
            IDbDataParameter colorIdParam = command.CreateParameter();
            colorIdParam.DbType = DbType.Int32;
            colorIdParam.ParameterName = "@colorId";
            colorIdParam.Value = article.Color.Id;
            command.Parameters.Add(colorIdParam);

            // Opretter parameter til størrelse-id
            IDbDataParameter sizeIdParam = command.CreateParameter();
            sizeIdParam.DbType = DbType.Int32;
            sizeIdParam.ParameterName = "@sizeId";
            sizeIdParam.Value = article.Size.Id;
            command.Parameters.Add(sizeIdParam);

            // Opretter parameter til dagspris
            IDbDataParameter dailyPriceParam = command.CreateParameter();
            dailyPriceParam.DbType = DbType.Single;
            dailyPriceParam.ParameterName = "@dailyPrice";
            dailyPriceParam.Value = article.DailyPrice;
            command.Parameters.Add(dailyPriceParam);

            // Opretter parameter til oprindelig pris
            IDbDataParameter originalPriceParam = command.CreateParameter();
            originalPriceParam.DbType = DbType.Single;
            originalPriceParam.ParameterName = "@originalPrice";
            originalPriceParam.Value = article.OriginalPrice;
            command.Parameters.Add(originalPriceParam);

            // Opretter parameter til om artiklen er udlejet
            IDbDataParameter isRentedParam = command.CreateParameter();
            isRentedParam.DbType = DbType.Int32;
            isRentedParam.ParameterName = "@isRented";
            isRentedParam.Value = article.IsRented ? 1 : 0;
            command.Parameters.Add(isRentedParam);

            // Opretter parameter til ejerens id
            IDbDataParameter ownerParam = command.CreateParameter();
            ownerParam.DbType = DbType.Int32;
            ownerParam.ParameterName = "@ownerId";
            ownerParam.Value = owner.Id;
            command.Parameters.Add(ownerParam);

            // Kører SQL-kommandoen og gemmer artiklen
            command.ExecuteNonQuery();
        }


        // Omdanner en databaserække til en ArticleDTO
        ArticleDTO CreateDTO(IDataReader reader)
        {
            // Finder placeringen af id-kolonnen
            int id = reader.GetOrdinal("id");

            // Finder placeringen af name-kolonnen
            int name = reader.GetOrdinal("name");

            // Finder placeringen af description-kolonnen
            int description = reader.GetOrdinal("description");

            // Finder placeringen af category_id-kolonnen
            int category = reader.GetOrdinal("category_id");

            // Finder placeringen af subcategory_id-kolonnen
            int subcategory = reader.GetOrdinal("subcategory_id");

            // Finder placeringen af brand_id-kolonnen
            int brand = reader.GetOrdinal("brand_id");

            // Finder placeringen af color_id-kolonnen
            int color = reader.GetOrdinal("color_id");

            // Finder placeringen af size_id-kolonnen
            int size = reader.GetOrdinal("size_id");

            // Finder placeringen af owner_id-kolonnen
            int owner = reader.GetOrdinal("owner_id");

            // Finder placeringen af daily_price-kolonnen
            int daily = reader.GetOrdinal("daily_price");

            // Finder placeringen af original_price-kolonnen
            int original = reader.GetOrdinal("original_price");

            // Finder placeringen af is_rented-kolonnen
            int rented = reader.GetOrdinal("is_rented");

            // Finder placeringen af is_clean-kolonnen
            int clean = reader.GetOrdinal("is_clean");

            // Finder placeringen af created_at-kolonnen
            int created = reader.GetOrdinal("created_at");

            // Finder placeringen af updated_at-kolonnen
            int updated = reader.GetOrdinal("updated_at");

            // Opretter et tomt ArticleDTO-objekt
            ArticleDTO dto = new ArticleDTO();

            // Sætter artikelens id
            dto.Id = reader.GetInt32(id);

            // Sætter artikelens titel/navn
            dto.Title = reader.GetString(name);

            // Tjekker om description ikke er null
            if (!reader.IsDBNull(description))
            {
                // Sætter artikelens beskrivelse
                dto.Description = reader.GetString(description);
            }
            else
            {
                // Sætter beskrivelsen til tom tekst hvis feltet er null
                dto.Description = string.Empty;
            }

            // Tjekker om category_id ikke er null
            if (!reader.IsDBNull(category))
            {
                // Sætter kategori-id
                dto.CategoryId = reader.GetInt32(category);
            }

            // Tjekker om subcategory_id ikke er null
            if (!reader.IsDBNull(subcategory))
            {
                // Sætter underkategori-id
                dto.SubcategoryId = reader.GetInt32(subcategory);
            }

            // Tjekker om brand_id ikke er null
            if (!reader.IsDBNull(brand))
            {
                // Sætter brand-id
                dto.BrandId = reader.GetInt32(brand);
            }

            // Tjekker om color_id ikke er null
            if (!reader.IsDBNull(color))
            {
                // Sætter farve-id
                dto.ColorId = reader.GetInt32(color);
            }

            // Tjekker om size_id ikke er null
            if (!reader.IsDBNull(size))
            {
                // Sætter størrelse-id
                dto.SizeId = reader.GetInt32(size);
            }

            // Tjekker om daily_price ikke er null
            if (!reader.IsDBNull(daily))
            {
                // Skriver dagspris i debug output
                Debug.WriteLine($"DTO DailyPrice: {dto.DailyPrice}");

                // Skriver dagspris fra databasen i debug output
                Debug.WriteLine($"reader DailyPrice: {reader.GetFloat(daily)}");

                // Sætter dagsprisen
                dto.DailyPrice = reader.GetFloat(daily);
            }
            else
            {
                // Sætter dagsprisen til 0 hvis feltet er null
                dto.DailyPrice = 0f;
            }

            // Tjekker om original_price ikke er null
            if (!reader.IsDBNull(original))
            {
                // Sætter oprindelig pris
                dto.OriginalPrice = reader.GetFloat(original);
            }
            else
            {
                // Sætter oprindelig pris til 0 hvis feltet er null
                dto.OriginalPrice = 0f;
            }

            // Sætter om artiklen er udlejet
            dto.IsRented = reader.GetInt32(rented) != 0;

            // Sætter om artiklen er ren
            dto.IsClean = reader.GetInt32(clean) != 0;

            // Tjekker om owner_id ikke er null
            if (!reader.IsDBNull(owner))
            {
                // Sætter ejer-id
                dto.OwnerId = reader.GetInt32(owner);
            }

            // Konverterer created_at fra tekst til DateTime
            dto.CreatedAt = DateTime.Parse(reader.GetString(created));

            // Konverterer updated_at fra tekst til DateTime
            dto.UpdatedAt = DateTime.Parse(reader.GetString(updated));

            // Returnerer den færdige DTO
            return dto;
        }
    }
}