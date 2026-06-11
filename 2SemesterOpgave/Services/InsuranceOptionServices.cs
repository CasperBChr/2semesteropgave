using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx InsuranceOption
using _2SemesterOpgave.Repositories; // Giver adgang til InsuranceOptionRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til InsuranceOptionDTO

namespace _2SemesterOpgave.Services
{
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	// Serviceklasse der håndterer logik for forsikringsmuligheder
	public class InsuranceOptionServices
    {
        // Repository der bruges til databasekald for forsikringsmuligheder
        InsuranceOptionRepository _repository;

        // Dictionary der bruges som cache, så forsikringsmuligheder kan findes hurtigt ud fra id
        Dictionary<int, InsuranceOption> _cache = new Dictionary<int, InsuranceOption>();

        // Constructor der modtager InsuranceOptionRepository
        public InsuranceOptionServices(InsuranceOptionRepository repository)
        {
            // Gemmer repository, så det kan bruges i metoderne
            _repository = repository;

            // Indlæser forsikringsmuligheder fra databasen til cache
            LoadCache();
        }

        // Henter alle forsikringsmuligheder fra repository og gemmer dem i cache
        void LoadCache()
        {
            // Gennemgår alle InsuranceOptionDTO'er fra repository
            foreach (InsuranceOptionDTO dto in _repository.GetAll())
            {
                // Mapper DTO'en til InsuranceOption og gemmer den i cache med id som nøgle
                _cache[dto.Id] = Map(dto);
            }
        }

        // Henter alle forsikringsmuligheder
        public List<InsuranceOption> GetAll()
        {
            // Returnerer en ny liste med alle forsikringsmuligheder fra cache
            return new List<InsuranceOption>(_cache.Values);
        }

        // Henter en forsikringsmulighed ud fra id
        public InsuranceOption? GetById(int id)
        {
            // Prøver at finde forsikringsmuligheden i cache
            _cache.TryGetValue(id, out InsuranceOption? option);

            // Returnerer forsikringsmuligheden, eller null hvis den ikke findes
            return option;
        }

        // Mapper en InsuranceOptionDTO til en InsuranceOption-model
        InsuranceOption Map(InsuranceOptionDTO dto)
        {
            // Opretter og returnerer en InsuranceOption med data fra DTO'en
            return new InsuranceOption
            {
                // Sætter forsikringsmulighedens id
                Id = dto.Id,

                // Sætter forsikringsmulighedens navn
                Name = dto.Name,

                // Sætter grundprisen for forsikringen
                BaseFees = dto.BaseFees
            };
        }
    }
}