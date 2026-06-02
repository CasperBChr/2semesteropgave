using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;
using _2SemesterOpgave.Repositories.Interfaces;

namespace _2SemesterOpgave.Repositories
{
    public class UserRepository
    {
        private static Database _db;

        public UserRepository(Database db)
        { 
            _db = db;
        }

        // Mapper en database-række til et User-objekt, så alle felter kommer med fra db.
        private static User MapUser(DbDataReader reader)
        {
            User user = new User();

            user.Id = reader.GetInt32(reader.GetOrdinal("ID"));
            user.Username = reader["Username"]?.ToString() ?? string.Empty;
            user.FirstName = reader["FirstName"]?.ToString() ?? string.Empty;
            user.LastName = reader["LastName"]?.ToString() ?? string.Empty;
            user.Email = reader["Email"]?.ToString() ?? string.Empty;
            user.Password = reader["Password"]?.ToString() ?? string.Empty;
            user.City = reader["City"]?.ToString() ?? string.Empty;
            user.ProfilePicture = reader["ProfilePicture"]?.ToString() ?? string.Empty;
            user.PhoneNumber = reader["PhoneNumber"]?.ToString() ?? string.Empty;
            user.Description = reader["Description"]?.ToString() ?? string.Empty;

            user.IsVerified = !reader.IsDBNull(reader.GetOrdinal("IsVerified")) && Convert.ToInt32(reader["IsVerified"]) == 1;
            user.RatingScore = reader.IsDBNull(reader.GetOrdinal("RatingScore")) ? 0 : Convert.ToSingle(reader["RatingScore"]);

            if (!reader.IsDBNull(reader.GetOrdinal("SignupTime")))
            {
                user.CreatedAt = Convert.ToDateTime(reader["SignupTime"]);
            }

			user.FollowersCount = reader.IsDBNull(reader.GetOrdinal("FollowersCount")) ? 0 : Convert.ToInt32(reader["FollowersCount"]);

			user.FollowingCount = reader.IsDBNull(reader.GetOrdinal("FollowingCount")) ? 0 : Convert.ToInt32(reader["FollowingCount"]);




			//user.FollowersCount = reader.IsDBNull(reader.GetOrdinal("Followers")) ? 0 : Convert.ToInt32(reader["Followers"]);
			//         user.FollowingCount = reader.IsDBNull(reader.GetOrdinal("Following")) ? 0 : Convert.ToInt32(reader["Following"]);
			return user;
        }

        public void CreateUser(User user)
        {
            _db.Open();
            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
            DbParameter usernameParam = command.CreateParameter();
            usernameParam.ParameterName = "Username";
            usernameParam.DbType = DbType.String;
            usernameParam.Value = user.FirstName;
            command.Parameters.Add(usernameParam);

            DbParameter emailParam = command.CreateParameter();
            emailParam.DbType = DbType.String;
            emailParam.ParameterName = "Email";
            emailParam.Value = user.Email;
            command.Parameters.Add(emailParam);

            DbParameter passwordParam = command.CreateParameter();
            passwordParam.DbType = DbType.String;
            passwordParam.ParameterName = "Password";
            passwordParam.Value = user.Password;
            command.Parameters.Add(passwordParam);

            command.ExecuteNonQuery();
            _db.Close();
        }

        public void DeleteUser(int id)
        {
            _db.Open();
            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText = "DELETE FROM Users WHERE ID = @ID";
            DbParameter parameter = command.CreateParameter();
            parameter.DbType = DbType.Int32;
            parameter.Value = id;
			parameter.ParameterName = "@ID";
			command.Parameters.Add(parameter);
            command.ExecuteNonQuery();          

            _db.Close();
        }

		public IEnumerable<UserDTO> GetAllUsers()
		{
			List<UserDTO> users = new();

			_db.Open();

			try
			{
				DbCommand command = _db.Connection.CreateCommand();

				command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u";

				using DbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					users.Add(CreateDTO(reader));
				}

				return users;
			}
			finally
			{
				_db.Close();
			}
		}

		//     public IEnumerable<User> GetAllUsers()
		//     {
		//         _db.Open();
		//         DbCommand command = _db.Connection.CreateCommand();
		//command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u";
		//List<User> users = new List<User>();
		//         DbDataReader reader = command.ExecuteReader();

		//         while (reader.Read())
		//         {
		//             users.Add(MapUser(reader));
		//         }

		//         reader.Close();
		//         _db.Close();
		//         return users;
		//     }


		public UserDTO? GetUserByID(int id)
		{
			_db.Open();

			try
			{
				DbCommand command = _db.Connection.CreateCommand();

				command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u WHERE u.ID = @ID";

				DbParameter parameter = command.CreateParameter();
				parameter.DbType = DbType.Int32;
				parameter.Value = id;
				parameter.ParameterName = "@ID";
				command.Parameters.Add(parameter);

				using DbDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					return CreateDTO(reader);
				}
				return null;
			}
			finally
			{
				_db.Close();
			}
		}

		//public User? GetUserByID(int id)
		//{
		//	_db.Open();

		//	try
		//	{
		//		DbCommand command = _db.Connection.CreateCommand();

		//		command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f  WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f  WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u WHERE u.ID = @ID";

		//		DbParameter parameter = command.CreateParameter();
		//		parameter.DbType = DbType.Int32;
		//		parameter.ParameterName = "@ID";
		//		parameter.Value = id;
		//		command.Parameters.Add(parameter);

		//		DbDataReader reader = command.ExecuteReader();

		//		if (reader.Read())
		//		{
		//			User user = MapUser(reader);
		//			reader.Close();
		//			return user;
		//		}

		//		reader.Close();
		//		return null;
		//	}
		//	finally
		//	{
		//		_db.Close();
		//	}
		//}

		public static int GetUserFollowerCount(int id) 
        {
            try 
            {
                _db.Open();

                DbCommand command = _db.Connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Followers WHERE following_id = @UserId";
                DbParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.Int32;
				parameter.ParameterName = "UserId";
				parameter.Value = id;
				command.Parameters.Add(parameter);
				int count = Convert.ToInt32(command.ExecuteScalar());
                return count;
			}
			finally 
            { 
                _db.Close(); 
            }
		}
		
		public static int GetUserFollowingCount(int userId)
		{
			try
			{
				_db.Open();

			    DbCommand command = _db.Connection.CreateCommand();
			    command.CommandText = "SELECT COUNT(*) FROM Followers WHERE follower_id = @UserId";

			    DbParameter parameter = command.CreateParameter();
			    parameter.DbType = DbType.Int32;
			    parameter.ParameterName = "@UserId";
			    parameter.Value = userId;
			    command.Parameters.Add(parameter);

			    int count = Convert.ToInt32(command.ExecuteScalar());

			    return count;   
			}
			finally
			{
				_db.Close();
			}

		}

		public void AddFollower(User follower, User following)
		{
			_db.Open();

			try
			{
				DbCommand command = _db.Connection.CreateCommand();

				command.CommandText = @"INSERT INTO Followers (follower_id, following_id) VALUES (@FollowerId, @FollowingId)";

				DbParameter followerParam = command.CreateParameter();
				followerParam.ParameterName = "@FollowerId";
				followerParam.DbType = DbType.Int32;
				followerParam.Value = follower.Id;
				command.Parameters.Add(followerParam);

				DbParameter followingParam = command.CreateParameter();
				followingParam.ParameterName = "@FollowingId";
				followingParam.DbType = DbType.Int32;
				followingParam.Value = following.Id;
				command.Parameters.Add(followingParam);

				command.ExecuteNonQuery();
			}
			finally
			{
				_db.Close();
			}
		}

		public void RemoveFollower(User follower, User following)
		{
			_db.Open();

			try
			{
				DbCommand command = _db.Connection.CreateCommand();

				command.CommandText = "DELETE FROM Followers WHERE follower_id = @FollowerId AND following_id = @FollowingId";

				DbParameter followerParam = command.CreateParameter();
				followerParam.ParameterName = "@FollowerId";
				followerParam.DbType = DbType.Int32;
				followerParam.Value = follower.Id;
				command.Parameters.Add(followerParam);

				DbParameter followingParam = command.CreateParameter();
				followingParam.ParameterName = "@FollowingId";
				followingParam.DbType = DbType.Int32;
				followingParam.Value = following.Id;
				command.Parameters.Add(followingParam);

				command.ExecuteNonQuery();
			}
			finally
			{
				_db.Close();
			}
		}

		public bool IsFollowing(int followerId, int followingId)
		{
			try
			{
			    _db.Open();
				DbCommand command = _db.Connection.CreateCommand();

				command.CommandText = "SELECT COUNT(*) FROM Followers WHERE follower_id = @FollowerId AND following_id = @FollowingId";

				DbParameter followerParam = command.CreateParameter();
				followerParam.ParameterName = "@FollowerId";
				followerParam.Value = followerId;
				command.Parameters.Add(followerParam);

				DbParameter followingParam = command.CreateParameter();
				followingParam.ParameterName = "@FollowingId";
				followingParam.Value = followingId;
				command.Parameters.Add(followingParam);

				int count = Convert.ToInt32(command.ExecuteScalar());
				return count > 0;
			}
			finally
			{
				_db.Close();
			}
		}

		public void UpdateUser(User user)
        {
            _db.Open();
            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText =
                "UPDATE Users SET Username = @Username, FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password, " +
                "City = @City, ProfilePicture = @ProfilePicture, PhoneNumber = @PhoneNumber, Description = @Description, " +
                "IsVerified = @IsVerified, RatingScore = @RatingScore WHERE ID = @ID";

            DbParameter idParam = command.CreateParameter();
            idParam.DbType = DbType.Int32;
            idParam.Value = user.Id;
			idParam.ParameterName = "@ID";
			command.Parameters.Add(idParam);

            DbParameter usernameParam = command.CreateParameter();
            usernameParam.DbType = DbType.String;
            usernameParam.Value = user.Username;
			usernameParam.ParameterName = "@Username";
			command.Parameters.Add(usernameParam);

            DbParameter firstNameParam = command.CreateParameter();
            firstNameParam.DbType = DbType.String;
            firstNameParam.Value = user.FirstName;
            firstNameParam.ParameterName = "@FirstName";
            command.Parameters.Add(firstNameParam);

            DbParameter lastNameParam = command.CreateParameter();
            lastNameParam.DbType = DbType.String;
            lastNameParam.Value = user.LastName;
            lastNameParam.ParameterName = "@LastName";
            command.Parameters.Add(lastNameParam);

            DbParameter emailParam = command.CreateParameter();
            emailParam.DbType = DbType.String;
            emailParam.Value = user.Email;
			emailParam.ParameterName = "@Email";
			command.Parameters.Add(emailParam);

            DbParameter passwordParam = command.CreateParameter();
            passwordParam.DbType = DbType.String;
            passwordParam.Value = user.Password;
			passwordParam.ParameterName = "@Password";
			command.Parameters.Add(passwordParam);

            DbParameter cityParam = command.CreateParameter();
            cityParam.DbType = DbType.String;
            cityParam.Value = user.City;
            cityParam.ParameterName = "@City";
            command.Parameters.Add(cityParam);

            DbParameter profilePictureParam = command.CreateParameter();
            profilePictureParam.DbType = DbType.String;
            profilePictureParam.Value = user.ProfilePicture;
            profilePictureParam.ParameterName = "@ProfilePicture";
            command.Parameters.Add(profilePictureParam);

            DbParameter phoneNumberParam = command.CreateParameter();
            phoneNumberParam.DbType = DbType.String;
            phoneNumberParam.Value = user.PhoneNumber;
            phoneNumberParam.ParameterName = "@PhoneNumber";
            command.Parameters.Add(phoneNumberParam);

            DbParameter descriptionParam = command.CreateParameter();
            descriptionParam.DbType = DbType.String;
            descriptionParam.Value = user.Description;
            descriptionParam.ParameterName = "@Description";
            command.Parameters.Add(descriptionParam);

            DbParameter isVerifiedParam = command.CreateParameter();
            isVerifiedParam.DbType = DbType.Int32;
            isVerifiedParam.Value = user.IsVerified ? 1 : 0;
            isVerifiedParam.ParameterName = "@IsVerified";
            command.Parameters.Add(isVerifiedParam);

            DbParameter ratingScoreParam = command.CreateParameter();
            ratingScoreParam.DbType = DbType.Single;
            ratingScoreParam.Value = user.RatingScore;
            ratingScoreParam.ParameterName = "@RatingScore";
            command.Parameters.Add(ratingScoreParam);

            command.ExecuteNonQuery();
            _db.Close();
        }

		UserDTO CreateDTO(DbDataReader reader)
		{
			int id = reader.GetOrdinal("ID");
			int username = reader.GetOrdinal("Username");
			int firstName = reader.GetOrdinal("FirstName");
			int lastName = reader.GetOrdinal("LastName");
			int email = reader.GetOrdinal("Email");
			int password = reader.GetOrdinal("Password");
			int city = reader.GetOrdinal("City");
			int profile = reader.GetOrdinal("ProfilePicture");
			int phone = reader.GetOrdinal("PhoneNumber");
			int description = reader.GetOrdinal("Description");
			int verified = reader.GetOrdinal("IsVerified");
			int rating = reader.GetOrdinal("RatingScore");
			int createdAt = reader.GetOrdinal("created_at");
			int updatedAt = reader.GetOrdinal("updated_at");
			int followers = reader.GetOrdinal("FollowersCount");
			int following = reader.GetOrdinal("FollowingCount");

			return new UserDTO
			{
				Id = reader.GetInt32(id),

				Username = reader.GetString(username),
				FirstName = reader.GetString(firstName),
				LastName = reader.GetString(lastName),

				Email = reader.GetString(email),
				Password = reader.GetString(password),

				City = reader.GetString(city),
				ProfilePicture = reader.GetString(profile),
				PhoneNumber = reader.GetString(phone),
				Description = reader.GetString(description),

				IsVerified = !reader.IsDBNull(verified) && Convert.ToInt32(reader.GetValue(verified)) == 1,
				RatingScore = reader.IsDBNull(rating) ? 0 : Convert.ToSingle(reader.GetValue(rating)),

				CreatedAt = reader.IsDBNull(createdAt)
					? DateTime.MinValue
					: Convert.ToDateTime(reader.GetValue(createdAt)),

				UpdatedAt = reader.IsDBNull(updatedAt)
					? DateTime.MinValue
					: Convert.ToDateTime(reader.GetValue(updatedAt)),

				FollowersCount = reader.IsDBNull(followers) ? 0 : Convert.ToInt32(reader.GetValue(followers)),
				FollowingCount = reader.IsDBNull(following) ? 0 : Convert.ToInt32(reader.GetValue(following))
			};
		}
	}
}