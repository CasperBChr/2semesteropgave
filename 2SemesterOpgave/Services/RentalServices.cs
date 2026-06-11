using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Rental, User og Article
using _2SemesterOpgave.Repositories; // Giver adgang til RentalRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til RentalDTO

namespace _2SemesterOpgave.Services
{
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	// Serviceklasse der håndterer logik for lejeaftaler
	public class RentalServices
    {
        // Repository der bruges til databasekald for lejeaftaler
        RentalRepository _rentalRepository;

        // Service der bruges til at hente brugere
        UserServices _userServices;

        // Service der bruges til at hente artikler
        ArticleServices _articleServices;

        // Service der bruges til at hente fragtmuligheder
        ShippingOptionServices _shippingOptionServices;

        // Service der bruges til at hente forsikringsmuligheder
        InsuranceOptionServices _insuranceOptionServices;

        // Constructor der modtager de repositories og services klassen skal bruge
        public RentalServices(RentalRepository rentalRepository, UserServices userServices, ArticleServices articleServices, ShippingOptionServices shippingOptionServices, InsuranceOptionServices insuranceOptionServices)
        {
            // Gemmer RentalRepository, så den kan bruges i metoderne
            _rentalRepository = rentalRepository;

            // Gemmer UserServices, så brugere kan hentes
            _userServices = userServices;

            // Gemmer ArticleServices, så artikler kan hentes
            _articleServices = articleServices;

            // Gemmer ShippingOptionServices, så fragtvalg kan hentes
            _shippingOptionServices = shippingOptionServices;

            // Gemmer InsuranceOptionServices, så forsikringsvalg kan hentes
            _insuranceOptionServices = insuranceOptionServices;
        }

        // Henter lejeaftaler hvor brugeren er lejer
        public ObservableCollection<Rental> GetByRenter(User renter)
        {
            // Henter RentalDTO'er fra repository ud fra renter id
            IEnumerable<RentalDTO> dtos = _rentalRepository.GetByRenterId(renter.Id);

            // Mapper DTO'erne til Rental-modeller og returnerer dem
            return MapMany(dtos);
        }

        // Henter lejeaftaler hvor brugeren er udlejer/ejer
        public ObservableCollection<Rental> GetByRentee(User rentee)
        {
            // Henter RentalDTO'er fra repository ud fra rentee id
            IEnumerable<RentalDTO> dtos = _rentalRepository.GetByRenteeId(rentee.Id);

            // Mapper DTO'erne til Rental-modeller og returnerer dem
            return MapMany(dtos);
        }

        // Henter alle lejeaftaler
        public ObservableCollection<Rental> GetAll()
        {
            // Henter alle RentalDTO'er fra repository
            IEnumerable<RentalDTO> dtos = _rentalRepository.GetAll();

            // Mapper DTO'erne til Rental-modeller og returnerer dem
            return MapMany(dtos);
        }

        // Opretter en ny lejeaftale
        public void CreateRental(Rental rental)
        {
            // Mapper Rental-modellen til en RentalDTO, så den kan gemmes i databasen
            RentalDTO dto = new RentalDTO
            {
                // Konverterer startdato til tekst i databaseformat
                StartDate = rental.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),

                // Konverterer slutdato til tekst i databaseformat
                EndDate = rental.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),

                // Sætter totalprisen
                TotalPrice = (float)rental.TotalPrice,

                // Nye lejeaftaler starter som ikke accepteret
                IsAccepted = false,

                // Nye lejeaftaler får status active
                Status = "active",

                // Sætter id på brugeren der lejer artiklen
                RenterId = rental.Renter.Id,

                // Sætter id på brugeren der ejer/udlejer artiklen
                RenteeId = rental.Rentee.Id,

                // Sætter id på artiklen der lejes
                ArticleId = rental.Article.Id,

                // Sætter valgt fragtmulighed, hvis der er valgt en
                ShippingOptionId = rental.ShippingChoice?.Id,

                // Sætter valgt forsikring, hvis der er valgt en
                InsuranceOptionId = rental.InsuranceChoice?.Id
            };

            // Sender DTO'en videre til repository, som gemmer den i databasen
            _rentalRepository.Create(dto);
        }

        // Accepterer en lejeaftale
        public void AcceptRental(Rental rental)
        {
            // Opdaterer databasen så lejeaftalen er accepteret
            _rentalRepository.SetAccepted(rental.Id, true);

            // Opdaterer objektet i programmet
            rental.IsAccepted = true;
        }

        // Opdaterer status på en lejeaftale
        public void UpdateStatus(Rental rental, string status)
        {
            // Opdaterer status i databasen
            _rentalRepository.UpdateStatus(rental.Id, status);

            // Opdaterer status på objektet i programmet
            rental.Status = status;
        }

        // Mapper flere RentalDTO'er til Rental-modeller
        ObservableCollection<Rental> MapMany(IEnumerable<RentalDTO> dtos)
        {
            // Opretter en ObservableCollection, så UI kan vise lejeaftalerne
            ObservableCollection<Rental> rentals = new ObservableCollection<Rental>();

            // Gennemgår alle DTO'er
            foreach (RentalDTO dto in dtos)
            {
                // Mapper DTO'en til en Rental-model
                Rental? rental = MapToRental(dto);

                // Tjekker om mappingen lykkedes
                if (rental != null)
                {
                    // Tilføjer lejeaftalen til listen
                    rentals.Add(rental);
                }
            }

            // Returnerer listen med lejeaftaler
            return rentals;
        }

        // Henter alle datoer hvor en bestemt artikel allerede er booket
        public HashSet<DateOnly> GetBookedDatesForArticle(int articleId)
        {
            // HashSet bruges så samme dato kun gemmes én gang
            HashSet<DateOnly> booked = new HashSet<DateOnly>();

            // Gennemgår alle bookede datointervaller for artiklen
            foreach (var (start, end) in _rentalRepository.GetBookedDateRangesForArticle(articleId))
            {
                // Går igennem alle datoer fra start til slut
                for (DateOnly d = start; d <= end; d = d.AddDays(1))
                {
                    // Tilføjer datoen til listen over bookede datoer
                    booked.Add(d);
                }
            }

            // Returnerer alle bookede datoer
            return booked;
        }


        // Mapper en RentalDTO til en Rental-model
        Rental? MapToRental(RentalDTO dto)
        {
            // Henter brugeren der lejer artiklen
            User? renter = _userServices.GetById(dto.RenterId);

            // Henter brugeren der ejer/udlejer artiklen
            User? rentee = _userServices.GetById(dto.RenteeId);

            // Finder artiklen ud fra artikelens id
            Article? article = new List<Article>(_articleServices.GetAllArticles()).FirstOrDefault(article => article.Id == dto.ArticleId);

            // Henter fragtmuligheden hvis der er valgt en
            ShippingOption? shipping = dto.ShippingOptionId.HasValue ? _shippingOptionServices.GetById(dto.ShippingOptionId.Value) : null;

            // Henter forsikringsmuligheden hvis der er valgt en
            InsuranceOption? insurance = dto.InsuranceOptionId.HasValue ? _insuranceOptionServices.GetById(dto.InsuranceOptionId.Value) : null;

            // Stopper mappingen hvis vigtige data mangler
            if (renter == null || rentee == null || article == null) return null;

            // Tjekker om startdatoen kan læses korrekt
            if (!DateOnly.TryParseExact(dto.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate))
            {
                // Returnerer null hvis startdatoen er ugyldig
                return null;
            }

            // Tjekker om slutdatoen kan læses korrekt
            if (!DateOnly.TryParseExact(dto.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate))
            {
                // Returnerer null hvis slutdatoen er ugyldig
                return null;
            }

            // Opretter og returnerer en Rental-model med data fra DTO'en
            return new Rental
            {
                // Sætter lejeaftalens id
                Id = dto.Id,

                // Sætter startdatoen
                StartDate = DateOnly.Parse(dto.StartDate),

                // Sætter slutdatoen
                EndDate = DateOnly.Parse(dto.EndDate),

                // Sætter totalprisen
                TotalPrice = (decimal)dto.TotalPrice,

                // Sætter om lejeaftalen er accepteret
                IsAccepted = dto.IsAccepted,

                // Sætter status på lejeaftalen
                Status = dto.Status,

                // Sætter brugeren der lejer artiklen
                Renter = renter,

                // Sætter brugeren der ejer/udlejer artiklen
                Rentee = rentee,

                // Sætter artiklen der lejes
                Article = article,

                // Sætter hvornår lejeaftalen blev oprettet
                CreatedAt = dto.CreatedAt,

                // Sætter valgt fragtmulighed
                ShippingChoice = shipping,

                // Sætter valgt forsikringsmulighed
                InsuranceChoice = insurance
            };
        }
    }
}