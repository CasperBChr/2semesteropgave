using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;
using _2SemesterOpgave.Data;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories.Interfaces;

namespace _2SemesterOpgave.Repositories
{
    public class UserRepository : IUserRepository
    {
        private Database _db;

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
                user.SignupTime = Convert.ToDateTime(reader["SignupTime"]);
            }

            user.FollowersCount = reader.IsDBNull(reader.GetOrdinal("Followers")) ? 0 : Convert.ToInt32(reader["Followers"]);
            user.FollowingCount = reader.IsDBNull(reader.GetOrdinal("Following")) ? 0 : Convert.ToInt32(reader["Following"]);

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

        public IEnumerable<User> GetAllUsers()
        {
            _db.Open();
            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users";
            List<User> users = new List<User>();
            DbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(MapUser(reader));
            }

            reader.Close();
            _db.Close();
            return users;
        }

        public User? GetUserByID(int id)
        {
            _db.Open();
            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE ID = @ID";
            DbParameter parameter = command.CreateParameter();
            parameter.DbType = DbType.Int32;
            parameter.ParameterName = "@ID";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            DbDataReader reader = command.ExecuteReader();

           if (reader.Read())
           {
                User user = MapUser(reader);
                reader.Close();
                _db.Close();
                return user;
            }

            reader.Close();
            _db.Close();
            throw new ArgumentException("User not found");
        }

        public void UpdateUser(User user)
        {
            _db.Open();
            DbCommand command = _db.Connection.CreateCommand();
            command.CommandText =
                "UPDATE Users SET Username = @Username, FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password, " +
                "City = @City, ProfilePicture = @ProfilePicture, PhoneNumber = @PhoneNumber, Description = @Description, " +
                "IsVerified = @IsVerified, RatingScore = @RatingScore, Followers = @Followers, Following = @Following WHERE ID = @ID";

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

            DbParameter followersParam = command.CreateParameter();
            followersParam.DbType = DbType.Int32;
            followersParam.Value = user.FollowersCount;
            followersParam.ParameterName = "@Followers";
            command.Parameters.Add(followersParam);

            DbParameter followingParam = command.CreateParameter();
            followingParam.DbType = DbType.Int32;
            followingParam.Value = user.FollowingCount;
            followingParam.ParameterName = "@Following";
            command.Parameters.Add(followingParam);

            command.ExecuteNonQuery();
            _db.Close();
        }
    }
}