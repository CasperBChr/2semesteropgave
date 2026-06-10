using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Color
using _2SemesterOpgave.Repositories; // Giver adgang til ColorRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til ColorDTO

namespace _2SemesterOpgave.Services
{
    // Serviceklasse der håndterer logik for farver
    public class ColorServices
    {
        // Repository der bruges til databasekald for farver
        ColorRepository _colorRepository;

        // Dictionary der bruges som cache, så farver kan findes hurtigt ud fra id
        Dictionary<int, Color> _colors = new Dictionary<int, Color>();

        // Constructor der modtager ColorRepository
        public ColorServices(ColorRepository colorRepository)
        {
            // Gemmer ColorRepository, så den kan bruges i metoderne
            _colorRepository = colorRepository;

            // Indlæser farver fra databasen til cache
            LoadCache();
        }

        // Henter alle farver fra repository og gemmer dem i cache
        void LoadCache()
        {
            // Henter alle farve-DTO'er fra repository
            IEnumerable<ColorDTO> dtos = _colorRepository.GetAllColors();

            // Gennemgår alle DTO'er
            foreach (ColorDTO dto in dtos)
            {
                // Opretter en Color-model og gemmer den i dictionary med id som nøgle
                _colors[dto.Id] = new Color
                {
                    // Sætter farvens id
                    Id = dto.Id,

                    // Sætter farvens navn
                    Name = dto.Name
                };
            }
        }

        // Henter alle farver
        public IEnumerable<Color> GetAllColors()
        {
            // Returnerer alle farver fra cache
            return _colors.Values;
        }

        // Henter en farve ud fra id
        public Color? GetById(int id)
        {
            // Prøver at finde farven i dictionary
            _colors.TryGetValue(id, out Color? color);

            // Returnerer farven, eller null hvis den ikke findes
            return color;
        }
    }
}