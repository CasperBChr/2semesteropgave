using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;
using _2SemesterOpgave.Data; // Giver adgang til database-factory
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User
using _2SemesterOpgave.Repositories.DTO; // Giver adgang til UserDTO
using Microsoft.Data.Sqlite; // Giver adgang til SQLite

namespace _2SemesterOpgave.Repositories
{
	// Repositoryklasse der håndterer databasekald for brugere
	/// <summary>
	/// Kodet på af os alle
	/// </summary>
	public class UserRepository
    {
        // Database-factory der bruges til at oprette databaseforbindelser
        IDatabaseFactory _db;

        // Constructor der modtager database-factory
        public UserRepository(IDatabaseFactory db)
        {
            // Gemmer database-factory, så den kan bruges i repository-metoderne
            _db = db;
        }

        // Opretter en ny bruger i databasen og returnerer brugerens nye id
        public int CreateUser(User user)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter en ny bruger og henter id'et på den nye række
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

            // Opretter parameter til brugernavn
            IDbDataParameter usernameParam = command.CreateParameter();
            usernameParam.ParameterName = "@Username";
            usernameParam.Value = user.Username;
            command.Parameters.Add(usernameParam);

            // Opretter parameter til email
            IDbDataParameter emailParam = command.CreateParameter();
            emailParam.ParameterName = "@Email";
            emailParam.Value = user.Email;
            command.Parameters.Add(emailParam);

            // Opretter parameter til adgangskode
            IDbDataParameter passwordParam = command.CreateParameter();
            passwordParam.ParameterName = "@Password";
            passwordParam.Value = user.Password;
            command.Parameters.Add(passwordParam);

            // Kører SQL-kommandoen og returnerer id'et på den nye bruger
            return Convert.ToInt32(command.ExecuteScalar());
        }


        // Sletter en bruger fra databasen ud fra id
        public void DeleteUser(int id)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der sletter brugeren ud fra id
            command.CommandText = "DELETE FROM Users WHERE ID = @ID";

            // Opretter parameter til brugerens id
            IDbDataParameter parameter = command.CreateParameter();

            // Sætter parameterens datatype
            parameter.DbType = DbType.Int32;

            // Sætter parameterens værdi til id'et
            parameter.Value = id;

            // Sætter parameterens navn
            parameter.ParameterName = "@ID";

            // Tilføjer parameteren til kommandoen
            command.Parameters.Add(parameter);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Henter alle brugere fra databasen
        public IEnumerable<UserDTO> GetAllUsers()
        {
            // Opretter en liste til UserDTO'er
            List<UserDTO> users = new List<UserDTO>();

            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter alle brugere
            command.CommandText = "SELECT * FROM Users";

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Looper igennem alle rækker i resultatet
            while (reader.Read())
            {
                // Omdanner databaserækken til en UserDTO og tilføjer den til listen
                users.Add(CreateDTO(reader));
            }

            // Returnerer listen med brugere
            return users;
        }


        // Henter en bruger ud fra id
        public UserDTO? GetUserByID(int id)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter brugeren og tæller antal followers og following
            command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u WHERE u.ID = @ID";

            // Opretter parameter til brugerens id
            IDbDataParameter parameter = command.CreateParameter();
            parameter.DbType = DbType.Int32;
            parameter.Value = id;
            parameter.ParameterName = "@ID";
            command.Parameters.Add(parameter);

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Tjekker om der findes en bruger
            if (reader.Read())
            {
                // Omdanner databaserækken til en UserDTO og returnerer den
                return CreateDTO(reader);
            }

            // Returnerer null hvis brugeren ikke findes
            return null;
        }

        // Henter antal followers for en bruger
        public int GetUserFollowerCount(int id)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der tæller hvor mange der følger brugeren
            command.CommandText = "SELECT COUNT(*) FROM Followers WHERE following_id = @UserId";

            // Opretter parameter til brugerens id
            IDbDataParameter parameter = command.CreateParameter();
            parameter.DbType = DbType.Int32;
            parameter.ParameterName = "UserId";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            // Kører SQL'en og konverterer resultatet til et heltal
            int count = Convert.ToInt32(command.ExecuteScalar());

            // Returnerer antal followers
            return count;
        }

        // Henter antal brugere som en bruger følger
        public int GetUserFollowingCount(int userId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der tæller hvor mange brugeren følger
            command.CommandText = "SELECT COUNT(*) FROM Followers WHERE follower_id = @UserId";

            // Opretter parameter til brugerens id
            IDbDataParameter parameter = command.CreateParameter();
            parameter.DbType = DbType.Int32;
            parameter.ParameterName = "@UserId";
            parameter.Value = userId;
            command.Parameters.Add(parameter);

            // Kører SQL'en og konverterer resultatet til et heltal
            int count = Convert.ToInt32(command.ExecuteScalar());

            // Returnerer antal brugere som brugeren følger
            return count;
        }

        // Tilføjer en follower-relation mellem to brugere
        public void AddFollower(User follower, User following)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der indsætter relationen mellem follower og following
            command.CommandText = @"INSERT INTO Followers (follower_id, following_id) VALUES (@FollowerId, @FollowingId)";

            // Opretter parameter til brugeren der følger
            IDbDataParameter followerParam = command.CreateParameter();
            followerParam.ParameterName = "@FollowerId";
            followerParam.DbType = DbType.Int32;
            followerParam.Value = follower.Id;
            command.Parameters.Add(followerParam);

            // Opretter parameter til brugeren der bliver fulgt
            IDbDataParameter followingParam = command.CreateParameter();
            followingParam.ParameterName = "@FollowingId";
            followingParam.DbType = DbType.Int32;
            followingParam.Value = following.Id;
            command.Parameters.Add(followingParam);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Fjerner en follower-relation mellem to brugere
        public void RemoveFollower(User follower, User following)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der sletter relationen mellem follower og following
            command.CommandText = "DELETE FROM Followers WHERE follower_id = @FollowerId AND following_id = @FollowingId";

            // Opretter parameter til brugeren der følger
            IDbDataParameter followerParam = command.CreateParameter();
            followerParam.ParameterName = "@FollowerId";
            followerParam.DbType = DbType.Int32;
            followerParam.Value = follower.Id;
            command.Parameters.Add(followerParam);

            // Opretter parameter til brugeren der bliver fulgt
            IDbDataParameter followingParam = command.CreateParameter();
            followingParam.ParameterName = "@FollowingId";
            followingParam.DbType = DbType.Int32;
            followingParam.Value = following.Id;
            command.Parameters.Add(followingParam);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Tjekker om én bruger følger en anden bruger
        public bool IsFollowing(int followerId, int followingId)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der tæller om follower-relationen findes
            command.CommandText = "SELECT COUNT(*) FROM Followers WHERE follower_id = @FollowerId AND following_id = @FollowingId";

            // Opretter parameter til follower id
            IDbDataParameter followerParam = command.CreateParameter();
            followerParam.ParameterName = "@FollowerId";
            followerParam.Value = followerId;
            command.Parameters.Add(followerParam);

            // Opretter parameter til following id
            IDbDataParameter followingParam = command.CreateParameter();
            followingParam.ParameterName = "@FollowingId";
            followingParam.Value = followingId;
            command.Parameters.Add(followingParam);

            // Kører SQL'en og konverterer resultatet til et heltal
            int count = Convert.ToInt32(command.ExecuteScalar());

            // Returnerer true hvis relationen findes
            return count > 0;
        }

        // Opdaterer en bruger i databasen
        public void UpdateUser(User user)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der opdaterer brugerens oplysninger ud fra id
            command.CommandText =
                "UPDATE Users SET Username = @Username, FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password, " +
                "City = @City, ProfilePicture = @ProfilePicture, PhoneNumber = @PhoneNumber, Description = @Description, " +
                "IsVerified = @IsVerified WHERE ID = @ID";

            // Opretter parameter til brugerens id
            IDbDataParameter idParam = command.CreateParameter();
            idParam.DbType = DbType.Int32;
            idParam.Value = user.Id;
            idParam.ParameterName = "@ID";
            command.Parameters.Add(idParam);

            // Opretter parameter til brugernavn
            IDbDataParameter usernameParam = command.CreateParameter();
            usernameParam.DbType = DbType.String;
            usernameParam.Value = user.Username;
            usernameParam.ParameterName = "@Username";
            command.Parameters.Add(usernameParam);

            // Opretter parameter til fornavn
            IDbDataParameter firstNameParam = command.CreateParameter();
            firstNameParam.DbType = DbType.String;
            firstNameParam.Value = user.FirstName;
            firstNameParam.ParameterName = "@FirstName";
            command.Parameters.Add(firstNameParam);

            // Opretter parameter til efternavn
            IDbDataParameter lastNameParam = command.CreateParameter();
            lastNameParam.DbType = DbType.String;
            lastNameParam.Value = user.LastName;
            lastNameParam.ParameterName = "@LastName";
            command.Parameters.Add(lastNameParam);

            // Opretter parameter til email
            IDbDataParameter emailParam = command.CreateParameter();
            emailParam.DbType = DbType.String;
            emailParam.Value = user.Email;
            emailParam.ParameterName = "@Email";
            command.Parameters.Add(emailParam);

            // Opretter parameter til adgangskode
            IDbDataParameter passwordParam = command.CreateParameter();
            passwordParam.DbType = DbType.String;
            passwordParam.Value = user.Password;
            passwordParam.ParameterName = "@Password";
            command.Parameters.Add(passwordParam);

            // Opretter parameter til by
            IDbDataParameter cityParam = command.CreateParameter();
            cityParam.DbType = DbType.String;
            cityParam.Value = user.City;
            cityParam.ParameterName = "@City";
            command.Parameters.Add(cityParam);

            // Opretter parameter til profilbillede
            IDbDataParameter profilePictureParam = command.CreateParameter();
            profilePictureParam.DbType = DbType.String;
            profilePictureParam.Value = user.ProfilePicture;
            profilePictureParam.ParameterName = "@ProfilePicture";
            command.Parameters.Add(profilePictureParam);

            // Opretter parameter til telefonnummer
            IDbDataParameter phoneNumberParam = command.CreateParameter();
            phoneNumberParam.DbType = DbType.String;
            phoneNumberParam.Value = user.PhoneNumber;
            phoneNumberParam.ParameterName = "@PhoneNumber";
            command.Parameters.Add(phoneNumberParam);

            // Opretter parameter til beskrivelse
            IDbDataParameter descriptionParam = command.CreateParameter();
            descriptionParam.DbType = DbType.String;
            descriptionParam.Value = user.Description;
            descriptionParam.ParameterName = "@Description";
            command.Parameters.Add(descriptionParam);

            // Opretter parameter til om brugeren er verificeret
            IDbDataParameter isVerifiedParam = command.CreateParameter();
            isVerifiedParam.DbType = DbType.Int32;
            isVerifiedParam.Value = user.IsVerified ? 1 : 0;
            isVerifiedParam.ParameterName = "@IsVerified";
            command.Parameters.Add(isVerifiedParam);

            // Kører SQL-kommandoen
            command.ExecuteNonQuery();
        }

        // Henter en bruger ud fra brugernavn
        public UserDTO? GetUserByUsername(string username)
        {
            // Opretter forbindelse til databasen
            using IDbConnection connection = _db.CreateConnection();

            // Opretter en SQL-kommando på forbindelsen
            using IDbCommand command = connection.CreateCommand();

            // SQL der henter brugeren og tæller antal followers og following
            command.CommandText = "SELECT u.*, (SELECT COUNT(*) FROM Followers f WHERE f.following_id = u.ID) AS FollowersCount, (SELECT COUNT(*) FROM Followers f WHERE f.follower_id = u.ID) AS FollowingCount FROM Users u WHERE u.Username = @Username";

            // Opretter parameter til brugernavn
            IDbDataParameter usernameParam = command.CreateParameter();
            usernameParam.ParameterName = "@Username";
            usernameParam.DbType = DbType.String;
            usernameParam.Value = username;

            // Tilføjer parameteren til kommandoen
            command.Parameters.Add(usernameParam);

            // Kører SQL-kommandoen og læser resultatet
            using IDataReader reader = command.ExecuteReader();

            // Tjekker om der findes en bruger
            if (reader.Read())
            {
                // Omdanner databaserækken til en UserDTO og returnerer den
                return CreateDTO(reader);
            }

            // Returnerer null hvis brugeren ikke findes
            return null;
        }


        // Omdanner en databaserække til en UserDTO
        UserDTO CreateDTO(IDataReader reader)
        {
            // Finder placeringen af ID-kolonnen
            int id = reader.GetOrdinal("ID");

            // Finder placeringen af Username-kolonnen
            int username = reader.GetOrdinal("Username");

            // Finder placeringen af FirstName-kolonnen
            int firstName = reader.GetOrdinal("FirstName");

            // Finder placeringen af LastName-kolonnen
            int lastName = reader.GetOrdinal("LastName");

            // Finder placeringen af Email-kolonnen
            int email = reader.GetOrdinal("Email");

            // Finder placeringen af Password-kolonnen
            int password = reader.GetOrdinal("Password");

            // Finder placeringen af City-kolonnen
            int city = reader.GetOrdinal("City");

            // Finder placeringen af ProfilePicture-kolonnen
            int profile = reader.GetOrdinal("ProfilePicture");

            // Finder placeringen af PhoneNumber-kolonnen
            int phone = reader.GetOrdinal("PhoneNumber");

            // Finder placeringen af Description-kolonnen
            int description = reader.GetOrdinal("Description");

            // Finder placeringen af IsVerified-kolonnen
            int verified = reader.GetOrdinal("IsVerified");

            // Finder placeringen af created_at-kolonnen
            int createdAt = reader.GetOrdinal("created_at");

            // Finder placeringen af updated_at-kolonnen
            int updatedAt = reader.GetOrdinal("updated_at");

            // Opretter og returnerer en UserDTO med data fra databasen
            return new UserDTO
            {
                // Sætter brugerens id
                Id = reader.GetInt32(id),

                // Sætter brugernavn eller tom tekst hvis feltet er null
                Username = reader.IsDBNull(username)
                    ? string.Empty
                    : reader.GetString(username),

                // Sætter fornavn eller tom tekst hvis feltet er null
                FirstName = reader.IsDBNull(firstName)
                    ? string.Empty
                    : reader.GetString(firstName),

                // Sætter efternavn eller tom tekst hvis feltet er null
                LastName = reader.IsDBNull(lastName)
                    ? string.Empty
                    : reader.GetString(lastName),

                // Sætter email eller tom tekst hvis feltet er null
                Email = reader.IsDBNull(email)
                    ? string.Empty
                    : reader.GetString(email),

                // Sætter adgangskode eller tom tekst hvis feltet er null
                Password = reader.IsDBNull(password)
                    ? string.Empty
                    : reader.GetString(password),

                // Sætter by eller tom tekst hvis feltet er null
                City = reader.IsDBNull(city)
                    ? string.Empty
                    : reader.GetString(city),

                // Sætter profilbillede eller tom tekst hvis feltet er null
                ProfilePicture = reader.IsDBNull(profile)
                    ? string.Empty
                    : reader.GetString(profile),

                // Sætter telefonnummer eller tom tekst hvis feltet er null
                PhoneNumber = reader.IsDBNull(phone)
                    ? string.Empty
                    : reader.GetString(phone),

                // Sætter beskrivelse eller tom tekst hvis feltet er null
                Description = reader.IsDBNull(description)
                    ? string.Empty
                    : reader.GetString(description),

                // Sætter om brugeren er verificeret
                IsVerified = !reader.IsDBNull(verified)
                    && Convert.ToInt32(reader.GetValue(verified)) == 1,

                // Sætter oprettelsesdato eller DateTime.MinValue hvis feltet er null
                CreatedAt = reader.IsDBNull(createdAt)
                    ? DateTime.MinValue
                    : Convert.ToDateTime(reader.GetValue(createdAt)),

                // Sætter opdateringsdato eller DateTime.MinValue hvis feltet er null
                UpdatedAt = reader.IsDBNull(updatedAt)
                    ? DateTime.MinValue
                    : Convert.ToDateTime(reader.GetValue(updatedAt)),
            };
        }
    }
}