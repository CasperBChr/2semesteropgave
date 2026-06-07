using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;
using _2SemesterOpgave.Services.Interfaces;
using _2SemesterOpgave.Utils;

namespace _2SemesterOpgave.Services
{
	public class AuthServices
	{
		UserRepository _userRepository;
		readonly SessionContext _session;
		UserServices _userServices;
		public User? CurrentUser => _session.CurrentUser;
		
		
		public AuthServices(UserRepository userRepository, UserServices userServices, SessionContext session)
		{
			_userRepository = userRepository;
			_userServices = userServices;
			_session = session;
		}

		/// <summary>
		/// Attempts login and sets session if successful.
		/// </summary>
		//public bool Login(string username, string password)
		//{
		//	var dto = _userRepository.GetUserByUsername(username);

		//	if (dto == null)
		//		return false;

		//	if (dto.Password != password)
		//		return false;

		//	var user = Map(dto);

		//	_session.CurrentUser = user;
		//	return true;
		//}

		public bool Login(string username, string password)
		{
			var dto = _userRepository.GetUserByUsername(username);
			if (dto == null) return false;
			if (dto.Password != password) return false;

			var user = Map(dto);
			_session.CurrentUser = user;
			_userServices.CurrentUser = user;
			return true;
		}

		public void Logout()
		{
			_session.Clear();
		}

		private User Map(UserDTO dto)
		{
			return new User
			{
				Id = dto.Id,
				Username = dto.Username,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = dto.Email,
				Password = dto.Password,
				City = dto.City,
				ProfilePicture = dto.ProfilePicture,
				PhoneNumber = dto.PhoneNumber,
				Description = dto.Description,
				IsVerified = dto.IsVerified,
				RatingScore = dto.RatingScore,
				CreatedAt = dto.CreatedAt,
				UpdatedAt = dto.UpdatedAt,
				FollowersCount = dto.FollowersCount,
				FollowingCount = dto.FollowingCount
			};
		}
	}
}
