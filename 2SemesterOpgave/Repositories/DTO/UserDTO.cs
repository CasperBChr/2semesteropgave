using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Repositories.DTO
{
	public class UserDTO
	{
		public int Id { get; set; }

		public string Username { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string ProfilePicture { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public bool IsVerified { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
