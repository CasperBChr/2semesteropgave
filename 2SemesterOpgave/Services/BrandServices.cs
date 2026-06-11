using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx Brand
using _2SemesterOpgave.Repositories; // Giver adgang til BrandRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til BrandDTO

namespace _2SemesterOpgave.Services
{
    // Serviceklasse der håndterer logik for brands
    public class BrandServices
    {

        // Repository der bruges til databasekald for brands
        BrandRepository _brandRepository;

        // Cache der gemmer brands, så de ikke skal hentes fra databasen hver gang
        Dictionary<int, Brand>? _cache;

        // Constructor der modtager BrandRepository
        public BrandServices(BrandRepository brandRepository)
        {
            // Gemmer BrandRepository, så den kan bruges i metoderne
            _brandRepository = brandRepository;
        }


        // Henter brands fra repository og gemmer dem i cache
        void Load()
        {
            // Henter alle brand-DTO'er fra repository
            IEnumerable<BrandDTO> dtos = _brandRepository.GetAll();

            // Opretter en ny cache med brand-id som nøgle
            _cache = new Dictionary<int, Brand>();

            // Gennemgår alle DTO'er
            foreach (BrandDTO dto in dtos)
            {
                // Mapper DTO'en til Brand og gemmer den i cache
                _cache[dto.Id] = MapBrand(dto);
            }
        }

        // Henter et brand ud fra id
        public Brand? GetById(int id)
        {
            // Tjekker om cache ikke er indlæst endnu
            if (_cache == null)
            {
                // Indlæser brands i cache
                Load();
            }

            // Variabel til det brand vi prøver at finde
            Brand? brand;

            // Prøver at finde brandet i cache ud fra id
            bool found = _cache.TryGetValue(id, out brand);

            // Tjekker om brandet ikke blev fundet
            if (!found)
            {
                // Returnerer null hvis brandet ikke findes
                return null;
            }

            // Returnerer det fundne brand
            return brand;
        }

        // Henter alle brands
        public IEnumerable<Brand> GetAllBrands()
        {
            // Tjekker om cache ikke er indlæst endnu
            if (_cache == null)
            {
                // Indlæser brands i cache
                Load();
            }

            // Returnerer en ny liste med alle brands fra cache
            return new List<Brand>(_cache.Values);
        }

        // Mapper en BrandDTO til en Brand-model
        Brand MapBrand(BrandDTO dto)
        {
            // Opretter og returnerer et Brand-objekt med data fra DTO'en
            return new Brand
            {
                // Sætter brandets id
                Id = dto.Id,

                // Sætter brandets navn
                Name = dto.Name,

                // Sætter logo-sti eller tom tekst hvis den er null
                LogoPath = dto.LogoPath ?? string.Empty,

                // Sætter hvornår brandet blev oprettet
                CreatedAt = dto.CreatedAt,

                // Sætter hvornår brandet sidst blev opdateret
                UpdatedAt = dto.UpdatedAt
            };
        }
    }
}