using Matala2_ASP.BL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Matala2_ASP.DAL
{
    public class CastDal
    {
        private readonly string connectionString = "Server=194.90.158.75;Database=bgroup1_test2;User Id=bgroup1;Password=bgroup1_14409;";

        // Get all casts
        public List<Cast> GetAllCasts()
        {
            List<Cast> casts = new List<Cast>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_GetAllCasts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
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
                }
                con.Close();
            }

            return casts;
        }

        // Insert a new cast
        public bool InsertCast(Cast cast)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_InsertCast", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", cast.Id);
                cmd.Parameters.AddWithValue("@Name", cast.Name);
                cmd.Parameters.AddWithValue("@Role", cast.Role);
                cmd.Parameters.AddWithValue("@DateOfBirth", cast.Date);
                cmd.Parameters.AddWithValue("@Country", cast.Country);
                cmd.Parameters.AddWithValue("@PhotoUrl", cast.PhotoUrl);

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

        // Update an existing cast
        public bool UpdateCast(Cast cast)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_UpdateCast", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", cast.Id);
                cmd.Parameters.AddWithValue("@Name", cast.Name);
                cmd.Parameters.AddWithValue("@Role", cast.Role);
                cmd.Parameters.AddWithValue("@DateOfBirth", cast.Date);
                cmd.Parameters.AddWithValue("@Country", cast.Country);
                cmd.Parameters.AddWithValue("@PhotoUrl", cast.PhotoUrl);

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

        // Delete a cast by ID
        public bool DeleteCast(string id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_DeleteCast", con)
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

        // Link a cast to a movie
        public bool InsertMovieCast(int movieId, string castId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_InsertMovieCast", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@MovieId", movieId);
                cmd.Parameters.AddWithValue("@CastId", castId);

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

        // Fetch all casts for a specific movie



    }
}
