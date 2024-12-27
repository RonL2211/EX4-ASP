using Matala2_ASP.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Matala2_ASP.DAL
{
    public class UserDal
    {

        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }
        // Add a new user
        public bool AddUser(UserOfMovies user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserName", user.UserName);
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@Password", user.Password);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertUser", con, paramDic); // create the command

            try
            {

                int numEff = cmd.ExecuteNonQuery();
                return numEff > 0;


            }
            catch (Exception ex)
            {
                return false;
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        // Update an existing user
        //public bool UpdateUser(User user)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SP_UpdateUser", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = user.Id });
        //            cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar) { Value = user.UserName });
        //            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email });
        //            cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = user.Password });

        //            try
        //            {
        //                con.Open();
        //                int rowsAffected = cmd.ExecuteNonQuery();
        //                return rowsAffected > 0;
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log exception
        //                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
        //                throw (ex);
        //                return false;
        //            }

        //            finally
        //            {
        //                con.Close();
        //            }
        //        }
        //    }
        //}

        // Get all users
        public List<UserOfMovies> GetAllUsers()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetAllUsers", con, null); // create the command

            try
            {
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    List<UserOfMovies> users = new List<UserOfMovies>();

                while (reader.Read())
                {
                    users.Add(new UserOfMovies
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Password = reader.GetString(reader.GetOrdinal("Password"))
                    });

                }
                return users;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        // Get a user by email and password
        public UserOfMovies GetUserByEmailAndPassword(string email, string password)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Email", email);
            paramDic.Add("@Password", password);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_UserLogin", con, paramDic); // create the command

            try
            {
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                {
                    UserOfMovies newUser = new UserOfMovies();
                     
                    {
                        newUser.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        newUser.UserName = reader.GetString(reader.GetOrdinal("UserName"));
                        newUser.Email = reader.GetString(reader.GetOrdinal("Email"));
                        newUser.Password = reader.GetString(reader.GetOrdinal("Password"));
                    };
                    return newUser;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //public bool DeleteUserMovie(int userId, int movieId)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("SP_DeleteUserMovie", con)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        cmd.Parameters.AddWithValue("@UserId", userId);
        //        cmd.Parameters.AddWithValue("@MovieId", movieId);

        //        try
        //        {
        //            int rowsAffected = cmd.ExecuteNonQuery();
        //            return rowsAffected > 0;
        //        }
        //        catch (Exception e)
        //        {
        //            throw (e);
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

    }
}
