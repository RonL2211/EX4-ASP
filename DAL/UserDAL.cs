    using Matala2_ASP.BL;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    namespace Matala2_ASP.DAL
    {
        public class UserDal
        {
            private readonly string connectionString = "Server=194.90.158.75;Database=bgroup1_test2;User Id=bgroup1;Password=bgroup1_14409;";

            // Add a new user
            public bool AddUser(User user)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_InsertUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar) { Value = user.UserName });
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email });
                        cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = user.Password });

                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log exception
                        Console.WriteLine($"Error in AddUser: {ex.Message}");
                        throw (ex);
                        return false;
                    }

                    finally
                    {
                        con.Close();
                    }
                }
                }
            }

            // Update an existing user
            public bool UpdateUser(User user)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = user.Id });
                        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar) { Value = user.UserName });
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email });
                        cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = user.Password });

                        try
                        {
                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            // Log exception
                            Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                        throw (ex);
                        return false;
                    }

                    finally
                    {
                        con.Close();
                    }
                }
                }
            }

            // Get all users
            public List<User> GetAllUsers()
            {
                List<User> users = new List<User>();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetAllUsers", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    users.Add(new User
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        Password = reader.GetString(reader.GetOrdinal("Password"))
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log exception
                            Console.WriteLine($"Error in GetAllUsers: {ex.Message}");
                        }
                        finally
                        { 
                                 con.Close();
                        }
                    }
                }
                return users;
            }

            // Get a user by email and password
            public User? GetUserByEmailAndPassword(string email, string password)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UserLogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email });
                        cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = password });

                        try
                        {
                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return new User
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        Password = reader.GetString(reader.GetOrdinal("Password"))
                                    };
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log exception
                            Console.WriteLine($"Error in GetUserByEmailAndPassword: {ex.Message}");
                        }
                        finally { con.Close(); }
                    }
                }
                return null;
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
                    con.Close();
                }
            }
        }

    }
}
