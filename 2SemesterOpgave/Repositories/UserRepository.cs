using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.DTO;
using Microsoft.Data.Sqlite;

namespace _2SemesterOpgave.Repositories
{
    public class UserRepository
    {
		IDatabaseFactory _db;

        public UserRepository(IDatabaseFactory db)
        { 
            _db = db;
        }

		public int CreateUser(User user)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = @"
				INSERT INTO Users
				(
					Username,
					Email,
					Password
				)
				VALUES
				(
					@Username,
					@Email,
					@Password
				);

				SELECT last_insert_rowid();
			";

			IDbDataParameter usernameParam = command.CreateParameter();
			usernameParam.ParameterName = "@Username";
			usernameParam.Value = user.Username;
			command.Parameters.Add(usernameParam);

			IDbDataParameter emailParam = command.CreateParameter();
			emailParam.ParameterName = "@Email";
			emailParam.Value = user.Email;
			command.Parameters.Add(emailParam);

			IDbDataParameter passwordParam = command.CreateParameter();
			passwordParam.ParameterName = "@Password";
			passwordParam.Value = user.Password;
			command.Parameters.Add(passwordParam);

			return Convert.ToInt32(command.ExecuteScalar());
		}


		public void DeleteUser(int id)
        {
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = "DELETE FROM Users WHERE ID = @ID";
            IDbDataParameter parameter = command.CreateParameter();
            parameter.DbType = DbType.Int32;
            parameter.Value = id;
			parameter.ParameterName = "@ID";
			command.Parameters.Add(parameter);
            command.ExecuteNonQuery();          
        }

		public IEnumerable<UserDTO> GetAllUsers()
		{
			List<UserDTO> users = new();

			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = "SELECT * FROM Users";

				using IDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					users.Add(CreateDTO(reader));
				}

				return users;
		}


		public UserDTO? GetUserByID(int id)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u WHERE u.ID = @ID";

				IDbDataParameter parameter = command.CreateParameter();
				parameter.DbType = DbType.Int32;
				parameter.Value = id;
				parameter.ParameterName = "@ID";
				command.Parameters.Add(parameter);

				using IDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					return CreateDTO(reader);
				}
				return null;
		}

		public int GetUserFollowerCount(int id) 
        {
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT COUNT(*) FROM Followers WHERE following_id = @UserId";
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.Int32;
				parameter.ParameterName = "UserId";
				parameter.Value = id;
				command.Parameters.Add(parameter);
				int count = Convert.ToInt32(command.ExecuteScalar());
                return count;
		}
		
		public int GetUserFollowingCount(int userId)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT COUNT(*) FROM Followers WHERE follower_id = @UserId";

			    IDbDataParameter parameter = command.CreateParameter();
			    parameter.DbType = DbType.Int32;
			    parameter.ParameterName = "@UserId";
			    parameter.Value = userId;
			    command.Parameters.Add(parameter);

			    int count = Convert.ToInt32(command.ExecuteScalar());

			    return count;

		}

		public void AddFollower(User follower, User following)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText = @"INSERT INTO Followers (follower_id, following_id) VALUES (@FollowerId, @FollowingId)";

				IDbDataParameter followerParam = command.CreateParameter();
				followerParam.ParameterName = "@FollowerId";
				followerParam.DbType = DbType.Int32;
				followerParam.Value = follower.Id;
				command.Parameters.Add(followerParam);

				IDbDataParameter followingParam = command.CreateParameter();
				followingParam.ParameterName = "@FollowingId";
				followingParam.DbType = DbType.Int32;
				followingParam.Value = following.Id;
				command.Parameters.Add(followingParam);

				command.ExecuteNonQuery();
		}

		public void RemoveFollower(User follower, User following)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = "DELETE FROM Followers WHERE follower_id = @FollowerId AND following_id = @FollowingId";

				IDbDataParameter followerParam = command.CreateParameter();
				followerParam.ParameterName = "@FollowerId";
				followerParam.DbType = DbType.Int32;
				followerParam.Value = follower.Id;
				command.Parameters.Add(followerParam);

				IDbDataParameter followingParam = command.CreateParameter();
				followingParam.ParameterName = "@FollowingId";
				followingParam.DbType = DbType.Int32;
				followingParam.Value = following.Id;
				command.Parameters.Add(followingParam);

				command.ExecuteNonQuery();
		}

		public bool IsFollowing(int followerId, int followingId)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = "SELECT COUNT(*) FROM Followers WHERE follower_id = @FollowerId AND following_id = @FollowingId";

				IDbDataParameter followerParam = command.CreateParameter();
				followerParam.ParameterName = "@FollowerId";
				followerParam.Value = followerId;
				command.Parameters.Add(followerParam);

				IDbDataParameter followingParam = command.CreateParameter();
				followingParam.ParameterName = "@FollowingId";
				followingParam.Value = followingId;
				command.Parameters.Add(followingParam);

				int count = Convert.ToInt32(command.ExecuteScalar());
				return count > 0;
		}

		public void UpdateUser(User user)
        {
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();
			command.CommandText =
                "UPDATE Users SET Username = @Username, FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password, " +
                "City = @City, ProfilePicture = @ProfilePicture, PhoneNumber = @PhoneNumber, Description = @Description, " +
                "IsVerified = @IsVerified WHERE ID = @ID";

            IDbDataParameter idParam = command.CreateParameter();
            idParam.DbType = DbType.Int32;
            idParam.Value = user.Id;
			idParam.ParameterName = "@ID";
			command.Parameters.Add(idParam);

            IDbDataParameter usernameParam = command.CreateParameter();
            usernameParam.DbType = DbType.String;
            usernameParam.Value = user.Username;
			usernameParam.ParameterName = "@Username";
			command.Parameters.Add(usernameParam);

            IDbDataParameter firstNameParam = command.CreateParameter();
            firstNameParam.DbType = DbType.String;
            firstNameParam.Value = user.FirstName;
            firstNameParam.ParameterName = "@FirstName";
            command.Parameters.Add(firstNameParam);

            IDbDataParameter lastNameParam = command.CreateParameter();
            lastNameParam.DbType = DbType.String;
            lastNameParam.Value = user.LastName;
            lastNameParam.ParameterName = "@LastName";
            command.Parameters.Add(lastNameParam);

            IDbDataParameter emailParam = command.CreateParameter();
            emailParam.DbType = DbType.String;
            emailParam.Value = user.Email;
			emailParam.ParameterName = "@Email";
			command.Parameters.Add(emailParam);

            IDbDataParameter passwordParam = command.CreateParameter();
            passwordParam.DbType = DbType.String;
            passwordParam.Value = user.Password;
			passwordParam.ParameterName = "@Password";
			command.Parameters.Add(passwordParam);

            IDbDataParameter cityParam = command.CreateParameter();
            cityParam.DbType = DbType.String;
            cityParam.Value = user.City;
            cityParam.ParameterName = "@City";
            command.Parameters.Add(cityParam);

            IDbDataParameter profilePictureParam = command.CreateParameter();
            profilePictureParam.DbType = DbType.String;
            profilePictureParam.Value = user.ProfilePicture;
            profilePictureParam.ParameterName = "@ProfilePicture";
            command.Parameters.Add(profilePictureParam);

            IDbDataParameter phoneNumberParam = command.CreateParameter();
            phoneNumberParam.DbType = DbType.String;
            phoneNumberParam.Value = user.PhoneNumber;
            phoneNumberParam.ParameterName = "@PhoneNumber";
            command.Parameters.Add(phoneNumberParam);

            IDbDataParameter descriptionParam = command.CreateParameter();
            descriptionParam.DbType = DbType.String;
            descriptionParam.Value = user.Description;
            descriptionParam.ParameterName = "@Description";
            command.Parameters.Add(descriptionParam);

            IDbDataParameter isVerifiedParam = command.CreateParameter();
            isVerifiedParam.DbType = DbType.Int32;
            isVerifiedParam.Value = user.IsVerified ? 1 : 0;
            isVerifiedParam.ParameterName = "@IsVerified";
            command.Parameters.Add(isVerifiedParam);

            command.ExecuteNonQuery();
        }

		public UserDTO? GetUserByUsername(string username)
		{
			using IDbConnection connection = _db.CreateConnection();
			using IDbCommand command = connection.CreateCommand();

			command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u WHERE u.Username = @Username";

				IDbDataParameter usernameParam = command.CreateParameter();
				usernameParam.ParameterName = "@Username";
				usernameParam.DbType = DbType.String;
				usernameParam.Value = username;

				command.Parameters.Add(usernameParam);

				using IDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{
					return CreateDTO(reader);
				}

				return null;
		}


		UserDTO CreateDTO(IDataReader reader)
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
			int createdAt = reader.GetOrdinal("created_at");
			int updatedAt = reader.GetOrdinal("updated_at");

			return new UserDTO
			{
				Id = reader.GetInt32(id),

				Username = reader.IsDBNull(username)
					? string.Empty
					: reader.GetString(username),

				FirstName = reader.IsDBNull(firstName)
					? string.Empty
					: reader.GetString(firstName),

				LastName = reader.IsDBNull(lastName)
					? string.Empty
					: reader.GetString(lastName),

				Email = reader.IsDBNull(email)
					? string.Empty
					: reader.GetString(email),

				Password = reader.IsDBNull(password)
					? string.Empty
					: reader.GetString(password),

				City = reader.IsDBNull(city)
					? string.Empty
					: reader.GetString(city),

				ProfilePicture = reader.IsDBNull(profile)
					? string.Empty
					: reader.GetString(profile),

				PhoneNumber = reader.IsDBNull(phone)
					? string.Empty
					: reader.GetString(phone),

				Description = reader.IsDBNull(description)
					? string.Empty
					: reader.GetString(description),

				IsVerified = !reader.IsDBNull(verified)
					&& Convert.ToInt32(reader.GetValue(verified)) == 1,

				CreatedAt = reader.IsDBNull(createdAt)
					? DateTime.MinValue
					: Convert.ToDateTime(reader.GetValue(createdAt)),

				UpdatedAt = reader.IsDBNull(updatedAt)
					? DateTime.MinValue
					: Convert.ToDateTime(reader.GetValue(updatedAt)),
			};
		}
	}
}