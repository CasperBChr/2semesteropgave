using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	// DTO-klasse der bruges til at transportere brugerdata fra databasen
	/// <summary>
	/// Vi har alle kodet på denne
	/// </summary>
	public class UserDTO
    {
        // Brugerens id i databasen
        public int Id { get; set; }

        // Brugerens brugernavn
        public string Username { get; set; } = string.Empty;

        // Brugerens fornavn
        public string FirstName { get; set; } = string.Empty;

        // Brugerens efternavn
        public string LastName { get; set; } = string.Empty;

        // Brugerens email
        public string Email { get; set; } = string.Empty;

        // Brugerens adgangskode
        public string Password { get; set; } = string.Empty;

        // Brugerens by
        public string City { get; set; } = string.Empty;

        // Sti eller link til brugerens profilbillede
        public string ProfilePicture { get; set; } = string.Empty;

        // Brugerens telefonnummer
        public string PhoneNumber { get; set; } = string.Empty;

        // Brugerens profilbeskrivelse
        public string Description { get; set; } = string.Empty;

        // Fortæller om brugeren er verificeret
        public bool IsVerified { get; set; }

        // Dato og tidspunkt for hvornår brugeren blev oprettet
        public DateTime CreatedAt { get; set; }

        // Dato og tidspunkt for hvornår brugeren sidst blev opdateret
        public DateTime UpdatedAt { get; set; }
    }
}