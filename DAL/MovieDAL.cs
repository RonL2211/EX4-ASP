using Matala2_ASP.BL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Matala2_ASP.DAL
{
    public class MovieDal
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

        // Get all movies
        public List<object> GetAllMovies()
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

            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetAllMovies", con, null); // create the command
            try
            {
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<object> movieCast = new List<object>();

                while (reader.Read())
                {
                    Movie movie = new Movie();
                    movie.Id = (int)reader["Id"];
                    movie.Title = reader["Title"]?.ToString() ?? "Unknown Title";
                    movie.Rating = reader["Rating"] is DBNull ? 0.0 : (double)reader["Rating"];
                    movie.ReleaseYear = reader["ReleaseYear"] is DBNull ? 0 : (int)reader["ReleaseYear"];
                    movie.Income = reader["Income"] is DBNull ? 0 : (int)reader["Income"];
                    movie.Duration = reader["Duration"] is DBNull ? 0 : (int)reader["Duration"];
                    movie.Language = reader["Language"]?.ToString() ?? "Unknown Language";
                    movie.Description = reader["Description"]?.ToString() ?? "No Description";
                    movie.Genre = reader["Genre"]?.ToString() ?? "Unknown Genre";
                    movie.PhotoUrl = reader["PhotoUrl"]?.ToString() ?? string.Empty;

                    List<Cast> casts = GetAllMovieCasts(movie.Id);
                    movieCast.Add(new { Movie = movie, Casts = casts });

                }
                return movieCast;
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

        public List<Cast> GetAllMovieCasts(int movieId)
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


            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetAllMovieCasts", con, paramDic); // create the command

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




        public bool InsertMovie(Movie movie)
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
            paramDic.Add("@Title", movie.Title);
            paramDic.Add("@Rating", movie.Rating);
            paramDic.Add("@Income", movie.Income);
            paramDic.Add("@ReleaseYear", movie.ReleaseYear);
            paramDic.Add("@Duration", movie.Duration);
            paramDic.Add("@Language", movie.Language);
            paramDic.Add("@Description", movie.Description);
            paramDic.Add("@Genre", movie.Genre);
            paramDic.Add("@PhotoUrl", movie.PhotoUrl);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertMovie", con, paramDic); // create the command

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

        //public bool UpdateMovie(Movie movie)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("SP_UpdateMovie", con)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        cmd.Parameters.AddWithValue("@Id", movie.Id);
        //        cmd.Parameters.AddWithValue("@Title", movie.Title);
        //        cmd.Parameters.AddWithValue("@Rating", movie.Rating);
        //        cmd.Parameters.AddWithValue("@Income", movie.Income);
        //        cmd.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
        //        cmd.Parameters.AddWithValue("@Duration", movie.Duration);
        //        cmd.Parameters.AddWithValue("@Language", movie.Language);
        //        cmd.Parameters.AddWithValue("@Description", movie.Description);
        //        cmd.Parameters.AddWithValue("@Genre", movie.Genre);
        //        cmd.Parameters.AddWithValue("@PhotoUrl", movie.PhotoUrl);

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

        //// Delete a movie
        //public bool DeleteMovie(int id)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("SP_DeleteMovie", con)
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


        public bool InsertWishlist(int userId, int movieId)
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
            paramDic.Add("@UserId", userId);
            paramDic.Add("@MovieId", movieId);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_InsertUserMovie", con, paramDic); // create the command

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


        public List<Movie> ShowWishlist(int userId)
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
            paramDic.Add("@UserId", userId);
            cmd = CreateCommandWithStoredProcedureGeneral("SP_GetAllUserMovies", con, paramDic); // create the command

            try
            {
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                List<Movie> wishlist = new List<Movie>();

                while (reader.Read())
                {
                    wishlist.Add(new Movie
                    {
                        Id = (int)reader["Id"],
                        Title = reader["Title"]?.ToString() ?? "Unknown Title",
                        Rating = reader["Rating"] is DBNull ? 0.0 : (double)reader["Rating"],
                        Income = reader["Income"] is DBNull ? 0 : (int)reader["Income"],
                        ReleaseYear = reader["ReleaseYear"] is DBNull ? 0 : (int)reader["ReleaseYear"],
                        Duration = reader["Duration"] is DBNull ? 0 : (int)reader["Duration"],
                        Language = reader["Language"]?.ToString() ?? "Unknown Language",
                        Description = reader["Description"]?.ToString() ?? "No Description",
                        Genre = reader["Genre"]?.ToString() ?? "Unknown Genre",
                        PhotoUrl = reader["PhotoUrl"]?.ToString() ?? string.Empty
                    });
                }
                return wishlist;
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

        // Link a user to a movie (add to favorites)
        //public bool InsertUserMovie(int userId, int movieId)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("SP_InsertUserMovie", con)
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
        //            con.Close(); // Always close connection
        //        }
        //    }
        //}

        public bool DeleteUserMovie(int userId, int movieId)
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
            paramDic.Add("@UserId", userId);
            paramDic.Add("@MovieId", movieId);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_DeleteUserMovie", con, paramDic); // create the command

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

