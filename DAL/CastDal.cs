using Matala2_ASP.BL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Matala2_ASP.DAL
{
    public class CastDal
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

        // Get all casts
        public List<Cast> GetAllCasts()
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


            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetAllCasts", con, null); // create the command

            try
            {
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Cast> casts = new List<Cast>();

                while (reader.Read())
                {
                    casts.Add(new Cast
                    {
                        Id = reader["Id"]?.ToString() ?? "Unknown Id",
                        Name = reader["Name"]?.ToString() ?? "Unknown Name",
                        Role = reader["Role"]?.ToString() ?? "Unknown Role",
                        Date = reader["DateOfBirth"] is DBNull ? DateTime.MinValue : (DateTime)reader["DateOfBirth"],
                        Country = reader["Country"]?.ToString() ?? "Unknown Country",
                        PhotoUrl = reader["PhotoUrl"]?.ToString() ?? string.Empty

                    });
                }
                return casts;
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


        // Insert a new cast
        public bool InsertCast(Cast cast)
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
            paramDic.Add("@Id", cast.Id);
            paramDic.Add("@Name", cast.Name);
            paramDic.Add("@Role", cast.Role);
            paramDic.Add("@DateOfBirth", cast.Date);
            paramDic.Add("@Country", cast.Country);
            paramDic.Add("@PhotoUrl", cast.PhotoUrl);
            cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertCast", con, paramDic); // create the command
            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                return false;
                throw (e);
            }
            finally
            {
                con.Close();
            }
        }
       

        // Update an existing cast
        //public bool UpdateCast(Cast cast)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("SP_UpdateCast", con)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        cmd.Parameters.AddWithValue("@Id", cast.Id);
        //        cmd.Parameters.AddWithValue("@Name", cast.Name);
        //        cmd.Parameters.AddWithValue("@Role", cast.Role);
        //        cmd.Parameters.AddWithValue("@DateOfBirth", cast.Date);
        //        cmd.Parameters.AddWithValue("@Country", cast.Country);
        //        cmd.Parameters.AddWithValue("@PhotoUrl", cast.PhotoUrl);

        //        try
        //        {
        //            int rowsAffected = cmd.ExecuteNonQuery();

        //            return rowsAffected > 0;
        //        }
        //        catch (Exception e)
        //        {

        //            throw (e);
        //            return false;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        //// Delete a cast by ID
        //public bool DeleteCast(string id)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("SP_DeleteCast", con)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        cmd.Parameters.AddWithValue("@Id", id);

        //        try
        //        {
        //            int rowsAffected = cmd.ExecuteNonQuery();

        //            return rowsAffected > 0;
        //        }
        //        catch (Exception e)
        //        {

        //            throw (e);
        //            return false;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        //// Link a cast to a movie
        /// <summary>
        /// 
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="castId"></param>
        /// <returns></returns>
      
        
        public bool InsertMovieCast(int movieId, string castId)
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
            paramDic.Add("@MovieId", movieId);
            paramDic.Add("@CastId", castId);
            cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertMovieCast", con, paramDic); // create the command


                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception e)
                {
                    return false;
                    throw (e);
                }
                finally
                {
                    con.Close();
                }
            }
        }

    }
