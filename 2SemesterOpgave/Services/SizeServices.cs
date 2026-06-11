using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Size
using _2SemesterOpgave.Repositories; // Giver adgang til SizeRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til SizeDTO

namespace _2SemesterOpgave.Services
{
    // Serviceklasse der håndterer logik for størrelser
    public class SizeServices
    {
        // Repository der bruges til databasekald for størrelser
        SizeRepository _sizeRepository;

        // Dictionary der bruges som cache, så størrelser kan findes hurtigt ud fra id
        Dictionary<int, Size> _cache = new Dictionary<int, Size>();

        // Constructor der modtager SizeRepository
        public SizeServices(SizeRepository sizeRepository)
        {
            // Gemmer SizeRepository, så den kan bruges i metoderne
            _sizeRepository = sizeRepository;

            // Indlæser størrelser fra databasen til cache
            LoadCache();
        }

        // Henter alle størrelser fra repository og gemmer dem i cache
        void LoadCache()
        {
            // Gennemgår alle SizeDTO'er fra repository
            foreach (SizeDTO dto in _sizeRepository.GetAllSizes())
            {
                // Mapper DTO'en til Size og gemmer den i cache med id som nøgle
                _cache[dto.Id] = Map(dto);
            }
        }

        // Henter alle størrelser
        public List<Size> GetAllSizes()
        {
            // Returnerer en ny liste med alle størrelser fra cache
            return new List<Size>(_cache.Values);
        }

        // Henter en størrelse ud fra id
        public Size? GetById(int id)
        {
            // Variabel til den størrelse vi prøver at finde
            Size? size;

            // Prøver at finde størrelsen i cache
            bool found = _cache.TryGetValue(id, out size);

            // Returnerer størrelsen hvis den blev fundet
            if (found) { return size; }

            // Returnerer null hvis størrelsen ikke findes
            return null;
        }

        // Mapper en SizeDTO til en Size-model
        private Size Map(SizeDTO dto)
        {
            // Opretter og returnerer en Size med data fra DTO'en
            return new Size
            {
                // Sætter størrelsens id
                Id = dto.Id,

                // Sætter størrelsens navn
                Name = dto.Name
            };
        }
    }
}