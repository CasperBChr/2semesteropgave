using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User
using _2SemesterOpgave.Repositories; // Giver adgang til UserRepository
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til UserDTO
using _2SemesterOpgave.Utils; // Giver adgang til SessionContext

namespace _2SemesterOpgave.Services
{
	// Serviceklasse der håndterer login, logout og nuværende bruger
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	public class AuthServices
    {
        // Repository der bruges til databasekald for brugere
        UserRepository _userRepository;

        // SessionContext gemmer den bruger der er logget ind
        readonly SessionContext _session;

        // Service der bruges til brugerlogik og CurrentUser
        UserServices _userServices;

        // Returnerer den nuværende bruger fra sessionen
        public User? CurrentUser => _session.CurrentUser;

        // Constructor der modtager repository, user service og session
        public AuthServices(UserRepository userRepository, UserServices userServices, SessionContext session)
        {
            // Gemmer UserRepository, så den kan bruges til at hente brugere
            _userRepository = userRepository;

            // Gemmer UserServices, så CurrentUser også kan sættes der
            _userServices = userServices;

            // Gemmer sessionen, så login-status kan gemmes
            _session = session;
        }

        // Forsøger at logge en bruger ind med brugernavn og adgangskode
        public bool Login(string username, string password)
        {
            // Henter brugerens DTO ud fra brugernavn
            UserDTO? dto = _userRepository.GetUserByUsername(username);

            // Returnerer false hvis brugeren ikke findes
            if (dto == null) return false;

            // Returnerer false hvis adgangskoden ikke matcher
            if (dto.Password != password) return false;

            // Mapper UserDTO til en User-model
            User user = Map(dto);

            // Gemmer brugeren som den nuværende bruger i sessionen
            _session.CurrentUser = user;

            // Gemmer også brugeren som CurrentUser i UserServices
            _userServices.CurrentUser = user;

            // Returnerer true fordi login lykkedes
            return true;
        }

        // Logger brugeren ud
        public void Logout()
        {
            // Rydder sessionen, så der ikke længere er en bruger logget ind
            _session.Clear();
        }

        // Mapper en UserDTO til en User-model
        private User Map(UserDTO dto)
        {
            // Opretter og returnerer en User med data fra DTO'en
            return new User
            {
                // Sætter brugerens id
                Id = dto.Id,

                // Sætter brugernavn
                Username = dto.Username,

                // Sætter fornavn
                FirstName = dto.FirstName,

                // Sætter efternavn
                LastName = dto.LastName,

                // Sætter email
                Email = dto.Email,

                // Sætter adgangskode
                Password = dto.Password,

                // Sætter by
                City = dto.City,

                // Sætter profilbillede
                ProfilePicture = dto.ProfilePicture,

                // Sætter telefonnummer
                PhoneNumber = dto.PhoneNumber,

                // Sætter profilbeskrivelse
                Description = dto.Description,

                // Sætter om brugeren er verificeret
                IsVerified = dto.IsVerified,

                // Sætter hvornår brugeren blev oprettet
                CreatedAt = dto.CreatedAt,

                // Sætter hvornår brugeren sidst blev opdateret
                UpdatedAt = dto.UpdatedAt,

                // Henter og sætter antal followers
                FollowersCount = _userRepository.GetUserFollowerCount(dto.Id),

                // Henter og sætter antal brugere som brugeren følger
                FollowingCount = _userRepository.GetUserFollowingCount(dto.Id)
            };
        }
    }
}