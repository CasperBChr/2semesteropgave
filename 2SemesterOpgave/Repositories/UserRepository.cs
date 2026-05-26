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
                users.Add(new User(username: reader.GetString(reader.GetOrdinal("Username")), email: reader.GetString(reader.GetOrdinal("Email")), password: reader.GetString(reader.GetOrdinal("Password")), id: reader.GetInt32(reader.GetOrdinal("ID"))));
                //users.Add(new User(username: reader["Username"].ToString(), email: reader["Email"].ToString(), password: reader["Password"].ToString(), id: Convert.ToInt32(reader["ID"])));
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
                User user = new User(reader["Username"].ToString(), reader["Email"].ToString(), reader["Password"].ToString(), Convert.ToInt32(reader["ID"]));
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
            command.CommandText = "UPDATE Users SET Username = @Username, Email = @Email, Password = @Password WHERE ID = @ID";
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

            command.ExecuteNonQuery();
            _db.Close();
        }
    }
}