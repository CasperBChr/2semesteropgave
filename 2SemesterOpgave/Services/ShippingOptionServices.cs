using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx ShippingOption
using _2SemesterOpgave.Repositories; // Giver adgang til ShippingOptionRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ShippingOptionDTO

namespace _2SemesterOpgave.Services
{
    // Serviceklasse der håndterer logik for fragtmuligheder
    public class ShippingOptionServices
    {
        // Repository der bruges til databasekald for fragtmuligheder
        ShippingOptionRepository _repository;

        // Dictionary der bruges som cache, så fragtmuligheder kan findes hurtigt ud fra id
        Dictionary<int, ShippingOption> _cache = new();

        // Constructor der modtager ShippingOptionRepository
        public ShippingOptionServices(ShippingOptionRepository repository)
        {
            // Gemmer repository, så det kan bruges i metoderne
            _repository = repository;

            // Indlæser fragtmuligheder fra databasen til cache
            LoadCache();
        }

        // Henter alle fragtmuligheder fra repository og gemmer dem i cache
        void LoadCache()
        {
            // Gennemgår alle ShippingOptionDTO'er fra repository
            foreach (ShippingOptionDTO dto in _repository.GetAll())
            {
                // Mapper DTO'en til ShippingOption og gemmer den i cache med id som nøgle
                _cache[dto.Id] = Map(dto);
            }
        }

        // Henter alle fragtmuligheder
        public List<ShippingOption> GetAll()
        {
            // Returnerer en ny liste med alle fragtmuligheder fra cache
            return new List<ShippingOption>(_cache.Values);
        }

        // Henter en fragtmulighed ud fra id
        public ShippingOption? GetById(int id)
        {
            // Prøver at finde fragtmuligheden i cache
            _cache.TryGetValue(id, out ShippingOption? option);

            // Returnerer fragtmuligheden, eller null hvis den ikke findes
            return option;
        }

        // Mapper en ShippingOptionDTO til en ShippingOption-model
        ShippingOption Map(ShippingOptionDTO dto)
        {
            // Opretter og returnerer en ShippingOption med data fra DTO'en
            return new ShippingOption
            {
                // Sætter fragtmulighedens id
                Id = dto.Id,

                // Sætter fragtmulighedens navn
                Name = dto.Name,

                // Sætter grundprisen for fragten
                BaseFee = dto.BaseFee,

                // Sætter antal dage leveringen tager
                DeliveryTimeDays = (byte)dto.DeliveryTimeDays
            };
        }
    }
}