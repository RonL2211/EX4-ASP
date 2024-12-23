using Matala2_ASP.BL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Matala2_ASP.DAL
{
    public class MovieDal
    {
        private readonly string connectionString = "Server=194.90.158.75;Database=bgroup1_test2;User Id=bgroup1;Password=bgroup1_14409;";

        // Get all movies
        public List<object> GetAllMovies()
        {
            SqlConnection con;
            List<object> movies = new List<object>();

            using (con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_GetAllMovies", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader reader = cmd.ExecuteReader();

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
                    movieCast.Add(new {Movie = movie , Casts=casts});

                }

                con.Close();
                return movieCast;

            }

        }



        public List<Cast> GetAllMovieCasts(int movieId)
        {
            List<Cast> casts = new List<Cast>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_GetAllMovieCasts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@MovieId", movieId);

                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            casts.Add(new Cast
                            {
                                Id = reader["CastId"]?.ToString() ?? "Unknown Id",
                                Name = reader["Name"]?.ToString() ?? "Unknown Name",
                                Role = reader["Role"]?.ToString() ?? "Unknown Role",
                                Date = reader["DateOfBirth"] is DBNull ? DateTime.MinValue : (DateTime)reader["DateOfBirth"],
                                Country = reader["Country"]?.ToString() ?? "Unknown Country",
                                PhotoUrl = reader["PhotoUrl"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
                finally
                {
                    con.Close(); // Close connection to avoid errors
                }
            }

            return casts;
        }



        // Insert a new movie
        public bool InsertMovie(Movie movie)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_InsertMovie", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Title", movie.Title);
                cmd.Parameters.AddWithValue("@Rating", movie.Rating);
                cmd.Parameters.AddWithValue("@Income", movie.Income);
                cmd.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                cmd.Parameters.AddWithValue("@Duration", movie.Duration);
                cmd.Parameters.AddWithValue("@Language", movie.Language);
                cmd.Parameters.AddWithValue("@Description", movie.Description);
                cmd.Parameters.AddWithValue("@Genre", movie.Genre);
                cmd.Parameters.AddWithValue("@PhotoUrl", movie.PhotoUrl);


                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
                catch (Exception e)
                {

                    throw (e);
                    return false;
                }
                finally
                {
                    con.Close();
                }

            }
        }

        public bool UpdateMovie(Movie movie)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_UpdateMovie", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", movie.Id);
                cmd.Parameters.AddWithValue("@Title", movie.Title);
                cmd.Parameters.AddWithValue("@Rating", movie.Rating);
                cmd.Parameters.AddWithValue("@Income", movie.Income);
                cmd.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                cmd.Parameters.AddWithValue("@Duration", movie.Duration);
                cmd.Parameters.AddWithValue("@Language", movie.Language);
                cmd.Parameters.AddWithValue("@Description", movie.Description);
                cmd.Parameters.AddWithValue("@Genre", movie.Genre);
                cmd.Parameters.AddWithValue("@PhotoUrl", movie.PhotoUrl);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
                catch (Exception e)
                {

                    throw (e);
                    return false;
                }
                finally
                {
                    con.Close();
                }

            }
        }

        // Delete a movie
        public bool DeleteMovie(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_DeleteMovie", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
                catch (Exception e)
                {

                    throw (e);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }


        public bool InsertWishlist(int userId, int movieId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_InsertUserMovie", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@MovieId", movieId);



                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
                catch (Exception e)
                {

                    throw (e);
                    return false;
                }
                finally
                {
                    con.Close();
                }

            }

        }


        public List<Movie> ShowWishlist(int userId)
        {

            SqlConnection con;
            List<Movie> wishlist = new List<Movie>();

            using (con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_GetAllUserMovies", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
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
                }
                con.Close();
            }

            return wishlist;
        }

        // Link a user to a movie (add to favorites)
        public bool InsertUserMovie(int userId, int movieId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_InsertUserMovie", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@MovieId", movieId);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception e)
                {
                    throw (e);
                }
                finally
                {
                    con.Close(); // Always close connection
                }
            }
        }

        public bool DeleteUserMovie(int userId, int movieId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_DeleteUserMovie", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@MovieId", movieId);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception e)
                {
                    throw (e);
                }
                finally
                {
                    con.Close(); // Always close connection
                }
            }
        }


    }
}

