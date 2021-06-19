using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AppBlaBlaCarWebApi.Models
{
    public class UserModel
    {
        string ConnectionString = "Server=tcp:driverpalf.database.windows.net,1433;Initial Catalog=BlaBlaCarDB;Persist Security Info=False;User ID=driverpalf;Password=Hola123.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // * Content table model
        public int IDUser { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public ResponseModel Login(string Email, string Password)
        {
            UserModel user = new UserModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Person WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserModel
                                {
                                    IDUser = (int)reader["IDUser"],
                                    Name = (string)reader["Name"],
                                    Picture = (string)reader["Picture"],
                                    Email = (string)reader["Email"],
                                    Password = (string)reader["Password"],
                                    Role = (string)reader["Role"],
                                };
                            }
                        }
                    }
                }

                if ( user.Name == null )
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = $"Contraseña o Correo invalidos :?",
                        Result = null
                    };
                }

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "El usuario fue obtenida con exito",
                    Result = user
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al Logear al usuario ({ex.Message})",
                    Result = null
                };
            }
        }
        public ResponseModel GetAll()
        {
            List<UserModel> list = new List<UserModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Person";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new UserModel
                                {
                                    IDUser = (int)reader["IDUser"],
                                    Name = (string)reader["Name"],
                                    Picture = (string)reader["Picture"],
                                    Email = (string)reader["Email"],
                                    Password = (string)reader["Password"],
                                    Role = (string)reader["Role"],

                                });
                            }
                        }
                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "Los usuarios fueron obtenidas con exito",
                            Result = list
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al consultar los usuarios ({ex.Message})",
                    Result = null
                };
            }
        }

        public ResponseModel Get(int id)
        {
            UserModel obj = new UserModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Person WHERE IDUser = @IDUser;";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDUser", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                obj = new UserModel
                                {
                                    IDUser = (int)reader["IDUser"],
                                    Name = (string)reader["Name"],
                                    Picture = (string)reader["Picture"],
                                    Email = (string)reader["Email"],
                                    Password = (string)reader["Password"],
                                    Role = (string)reader["Role"],
                                };
                            }
                        }
                    }
                }
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "El usuario fue obtenida con exito",
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al consultar el usuario ({ex.Message})",
                    Result = null
                };
            }
        }

        public ResponseModel Insert()
        {
            try
            {
                object newID;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "INSERT INTO Person (Name, Picture, Email, Password, Role) VALUES(@Name, @Picture, @Email, @Password, @Role); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Picture", Picture);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@Role", Role);

                        newID = cmd.ExecuteScalar();

                        if (newID != null && newID.ToString().Length > 0)
                        {
                            return new ResponseModel
                            {
                                IsSuccess = true,
                                Message = "El usuario fue agregado con éxito",
                                Result = newID
                            };
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                IsSuccess = false,
                                Message = $"Se generó un error al insertar el usuario",
                                Result = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al insertar el usuario ({ex.Message})",
                    Result = null
                };
            }
        }

        public ResponseModel Update()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "UPDATE Person SET Name = @Name, Picture = @Picture, Email = @Email, Password = @Password, Role = @Role WHERE IDUser = @IDUser;";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Picture", Picture);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@Role", Role);
                        cmd.Parameters.AddWithValue("@IDUser", IDUser);
                        cmd.ExecuteNonQuery();

                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "El usuario fue actualizado con éxito",
                            Result = IDUser
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al actualizar el usuario ({ex.Message})",
                    Result = null
                };
            }
        }

        public ResponseModel Delete(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "DELETE FROM Person WHERE IDUser = @IDUser;";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@IDUser", id);
                        cmd.ExecuteNonQuery();

                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = $"El usuario fue eliminado",
                            Result = IDUser
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al eliminar el usuario ({ex.Message})",
                    Result = null
                };
            }
        }
    }
}
