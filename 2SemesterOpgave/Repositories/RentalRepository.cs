using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til RentalDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
    // Repositoryklasse der håndterer databasekald for lejeaftaler
    public class RentalRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public RentalRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Henter alle lejeaftaler fra databasen
        public IEnumerable<RentalDTO> GetAll()
        {
            // Opretter en liste til RentalDTO'er
            List<RentalDTO> dtos = new List<RentalDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle lejeaftaler
            command.CommandText = "SELECT * FROM Rentals";

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en RentalDTO og tilføjer den til listen
                dtos.Add(CreateDTO(reader));
            }

            // Lukker readeren
            reader.Close();

            // Returnerer listen med lejeaftaler
            return dtos;
        }

        // Henter lejeaftaler hvor en bestemt bruger er lejer
        public IEnumerable<RentalDTO> GetByRenterId(int renterId)
        {
            // Opretter en liste til RentalDTO'er
            List<RentalDTO> dtos = new List<RentalDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter lejeaftaler ud fra renter_id
            command.CommandText = "SELECT * FROM Rentals WHERE renter_id = @renterId";

            // Opretter parameter til renter id
            IDbDataParameter param = command.CreateParameter();

            // Navnet på parameteren i SQL'en
            param.ParameterName = "@renterId";

            // Fortæller at parameteren er et heltal
            param.DbType = DbType.Int32;

            // Sætter værdien til det renterId metoden modtager
            param.Value = renterId;

            // Tilføjer parameteren til kommandoen
            command.Parameters.Add(param);

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en RentalDTO og tilføjer den til listen
                dtos.Add(CreateDTO(reader));
            }

            // Lukker readeren
            reader.Close();

            // Returnerer lejeaftalerne
            return dtos;
        }

        // Henter lejeaftaler hvor en bestemt bruger er udlejer/ejer
        public IEnumerable<RentalDTO> GetByRenteeId(int renteeId)
        {
            // Opretter en liste til RentalDTO'er
            List<RentalDTO> dtos = new List<RentalDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter lejeaftaler ud fra rentee_id
            command.CommandText = "SELECT * FROM Rentals WHERE rentee_id = @renteeId";

            // Opretter parameter til rentee id
            IDbDataParameter param = command.CreateParameter();

            // Navnet på parameteren i SQL'en
            param.ParameterName = "@renteeId";

            // Fortæller at parameteren er et heltal
            param.DbType = DbType.Int32;

            // Sætter værdien til det renteeId metoden modtager
            param.Value = renteeId;

            // Tilføjer parameteren til kommandoen
            command.Parameters.Add(param);

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en RentalDTO og tilføjer den til listen
                dtos.Add(CreateDTO(reader));
            }

            // Lukker readeren
            reader.Close();

            // Returnerer lejeaftalerne
            return dtos;
        }

        // Henter bookede datointervaller for en bestemt artikel
        public IEnumerable<(DateOnly Start, DateOnly End)> GetBookedDateRangesForArticle(int articleId)
        {
            // Opretter en liste til start- og slutdatoer
            List<(DateOnly, DateOnly)> ranges = new();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter datoer for lejeaftaler, som ikke er annulleret
            command.CommandText = @"
				SELECT start_date, end_date FROM Rentals
				WHERE article_id = @articleId
				AND status != 'cancelled'";

            // Tilføjer artikel-id som parameter
            AddParameter(command, "@articleId", DbType.Int32, articleId);

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle bookede perioder
            while (reader.Read())
            {
                // Konverterer startdato fra tekst til DateOnly
                DateOnly start = DateOnly.ParseExact(reader.GetString(0), "yyyy-MM-dd");

                // Konverterer slutdato fra tekst til DateOnly
                DateOnly end = DateOnly.ParseExact(reader.GetString(1), "yyyy-MM-dd");

                // Tilføjer datointervallet til listen
                ranges.Add((start, end));
            }

            // Returnerer alle bookede datointervaller
            return ranges;
        }

        // Opretter en ny lejeaftale i databasen
        public void Create(RentalDTO dto)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter en ny lejeaftale
            command.CommandText = @"
					INSERT INTO Rentals 
                    (start_date, end_date, total_price, is_accepted, renter_id, rentee_id, article_id, status, shipping_option_id, insurance_option_id)
                    VALUES 
                    (@startDate, @endDate, @totalPrice, @isAccepted, @renterId, @renteeId, @articleId, @status, @shippingOptionId, @insuranceOptionId)";

            // Tjekker om startdatoen har korrekt format
            if (!DateOnly.TryParseExact(dto.StartDate, "yyyy-MM-dd", out _))
            {
                // Kaster en fejl hvis startdatoen er ugyldig
                throw new Exception($"Invalid StartDate format: {dto.StartDate}");
            }

            // Tjekker om slutdatoen har korrekt format
            if (!DateOnly.TryParseExact(dto.EndDate, "yyyy-MM-dd", out _))
            {
                // Kaster en fejl hvis slutdatoen er ugyldig
                throw new Exception($"Invalid EndDate format: {dto.EndDate}");
            }

            // Tilføjer startdato som parameter
            AddParameter(command, "@startDate", DbType.String, dto.StartDate);

            // Tilføjer slutdato som parameter
            AddParameter(command, "@endDate", DbType.String, dto.EndDate);

            // Tilføjer totalpris som parameter
            AddParameter(command, "@totalPrice", DbType.Double, dto.TotalPrice);

            // Tilføjer om lejeaftalen er accepteret som parameter
            AddParameter(command, "@isAccepted", DbType.Int32, dto.IsAccepted ? 1 : 0);

            // Tilføjer renter id som parameter
            AddParameter(command, "@renterId", DbType.Int32, dto.RenterId);

            // Tilføjer rentee id som parameter
            AddParameter(command, "@renteeId", DbType.Int32, dto.RenteeId);

            // Tilføjer artikel id som parameter
            AddParameter(command, "@articleId", DbType.Int32, dto.ArticleId);

            // Tilføjer status som parameter
            AddParameter(command, "@status", DbType.String, dto.Status);

            // Tilføjer fragtmulighed-id, eller database-null hvis den mangler
            AddParameter(command, "@shippingOptionId", DbType.Int32, dto.ShippingOptionId.HasValue ? dto.ShippingOptionId.Value : DBNull.Value);

            // Tilføjer forsikringsmulighed-id, eller database-null hvis den mangler
            AddParameter(command, "@insuranceOptionId", DbType.Int32, dto.InsuranceOptionId.HasValue ? dto.InsuranceOptionId.Value : DBNull.Value);

            // Kører SQL-kommandoen og gemmer lejeaftalen
            command.ExecuteNonQuery();
        }

        // Opdaterer status på en lejeaftale
        public void UpdateStatus(int id, string status)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der opdaterer status ud fra lejeaftalens id
            command.CommandText = "UPDATE Rentals SET status = @status WHERE id = @id";

            // Tilføjer status som parameter
            AddParameter(command, "@status", DbType.String, status);

            // Tilføjer id som parameter
            AddParameter(command, "@id", DbType.Int32, id);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Sætter om en lejeaftale er accepteret
        public void SetAccepted(int id, bool isAccepted)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der opdaterer is_accepted ud fra lejeaftalens id
            command.CommandText = "UPDATE Rentals SET is_accepted = @isAccepted WHERE id = @id";

            // Tilføjer isAccepted som parameter
            AddParameter(command, "@isAccepted", DbType.Int32, isAccepted ? 1 : 0);

            // Tilføjer id som parameter
            AddParameter(command, "@id", DbType.Int32, id);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Hjælpemetode så vi undgår at gentage parameter-oprettelse
        void AddParameter(IDbCommand command, string name, DbType type, object value)
        {
            // Opretter en parameter til SQL-kommandoen
            IDbDataParameter param = command.CreateParameter();

            // Sætter parameterens navn
            param.ParameterName = name;

            // Sætter parameterens datatype
            param.DbType = type;

            // Sætter parameterens værdi
            param.Value = value;

            // Tilføjer parameteren til kommandoen
            command.Parameters.Add(param);
        }

        // Omdanner en databaserække til en RentalDTO
        RentalDTO CreateDTO(IDataReader reader)
        {
            // Finder placeringen af id-kolonnen
            int id = reader.GetOrdinal("id");

            // Finder placeringen af start_date-kolonnen
            int startDate = reader.GetOrdinal("start_date");

            // Finder placeringen af end_date-kolonnen
            int endDate = reader.GetOrdinal("end_date");

            // Finder placeringen af total_price-kolonnen
            int totalPrice = reader.GetOrdinal("total_price");

            // Finder placeringen af is_accepted-kolonnen
            int isAccepted = reader.GetOrdinal("is_accepted");

            // Finder placeringen af status-kolonnen
            int status = reader.GetOrdinal("status");

            // Finder placeringen af renter_id-kolonnen
            int renterId = reader.GetOrdinal("renter_id");

            // Finder placeringen af rentee_id-kolonnen
            int renteeId = reader.GetOrdinal("rentee_id");

            // Finder placeringen af article_id-kolonnen
            int articleId = reader.GetOrdinal("article_id");

            // Finder placeringen af shipping_option_id-kolonnen
            int shippingOptionId = reader.GetOrdinal("shipping_option_id");

            // Finder placeringen af insurance_option_id-kolonnen
            int insuranceOptionId = reader.GetOrdinal("insurance_option_id");

            // Finder placeringen af created_at-kolonnen
            int createdAt = reader.GetOrdinal("created_at");

            // Opretter og returnerer en RentalDTO med data fra databasen
            return new RentalDTO
            {
                // Sætter lejeaftalens id
                Id = reader.GetInt32(id),

                // Sætter startdatoen
                StartDate = reader.GetString(startDate),

                // Sætter slutdatoen
                EndDate = reader.GetString(endDate),

                // Sætter totalprisen
                TotalPrice = reader.GetFloat(totalPrice),

                // Sætter om lejeaftalen er accepteret
                IsAccepted = reader.GetInt32(isAccepted) == 1,

                // Sætter status
                Status = reader.GetString(status),

                // Sætter id på brugeren der lejer artiklen
                RenterId = reader.GetInt32(renterId),

                // Sætter id på brugeren der ejer/udlejer artiklen
                RenteeId = reader.GetInt32(renteeId),

                // Sætter id på artiklen der lejes
                ArticleId = reader.GetInt32(articleId),

                // Sætter id på fragtmuligheden
                ShippingOptionId = reader.GetInt32(shippingOptionId),

                // Sætter id på forsikringsmuligheden
                InsuranceOptionId = reader.GetInt32(insuranceOptionId),

                // Konverterer created_at fra tekst til DateTime
                CreatedAt = DateTime.Parse(reader.GetString(createdAt))
            };
        }
    }
}