using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AppBlaBlaCarWebApi.Models
{
    public class RideModel
    {
        string ConnectionString = "Server=tcp:driverpalf.database.windows.net,1433;Initial Catalog=BlaBlaCarDB;Persist Security Info=False;User ID=driverpalf;Password=Hola123.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int IDRide { get; set; }
        public int IDDriver { get; set; }
        public string OriginStr { get; set; }
        public string DestinationStr { get; set; }
        public double OriginLat { get; set; }
        public double OriginAlt { get; set; }
        public double DestinationLat { get; set; }
        public double DestinationAlt { get; set; }
        public int Passengers { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }

        public ResponseModel GetAll()
        {
            List<RideModel> list = new List<RideModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Ride";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new RideModel
                                {
                                    IDRide = (int)reader["IDRide"],
                                    IDDriver = (int)reader["IDDriver"],
                                    OriginStr = reader["OriginStr"].ToString(),
                                    DestinationStr = reader["DestinationStr"].ToString(),
                                    OriginLat = (double)reader["OriginLat"],
                                    OriginAlt = (double)reader["OriginAlt"],
                                    DestinationLat = (double)reader["DestinationLat"],
                                    DestinationAlt = (double)reader["DestinationAlt"],
                                    Passengers = (int)reader["Passengers"],
                                    Date = (DateTime)reader["Date"],
                                    Price = (double)reader["Price"]
                                });
                            }
                        }
                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "Los viajees fueron obtenidas con exito",
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
                    Message = $"Se generó un error al consultar los viajes ({ex.Message})",
                    Result = null
                };
            }
        }

        public ResponseModel Get(int id)
        {
            RideModel obj = new RideModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Ride WHERE IDRide = @IDRide;";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDRide", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                obj = new RideModel
                                {
                                    IDRide = (int)reader["IDRide"],
                                    IDDriver = (int)reader["IDDriver"],
                                    OriginStr = reader["OriginStr"].ToString(),
                                    DestinationStr = reader["DestinationStr"].ToString(),
                                    OriginLat = (double)reader["OriginLat"],
                                    OriginAlt = (double)reader["OriginAlt"],
                                    DestinationLat = (double)reader["DestinationLat"],
                                    DestinationAlt = (double)reader["DestinationAlt"],
                                    Passengers = (int)reader["Passengers"],
                                    Date = (DateTime)reader["Date"],
                                    Price = (double)reader["Price"]
                                };
                            }
                        }
                    }
                }
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "El viaje fue obtenida con exito",
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al consultar el viaje ({ex.Message})",
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
                    string tsql = "INSERT INTO Ride (IDDriver, OriginStr, DestinationStr, OriginLat, OriginAlt, DestinationLat, DestinationAlt, Passengers, Date, Price) VALUES(@IDDriver, @OriginStr, @DestinationStr, @OriginLat, @OriginAlt, @DestinationLat, @DestinationAlt, @Passengers, @Date, @Price); SELECT LAST_INSERT_ID();";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@IDDriver", IDDriver);
                        cmd.Parameters.AddWithValue("@OriginStr", OriginStr);
                        cmd.Parameters.AddWithValue("@DestinationStr", DestinationStr);
                        cmd.Parameters.AddWithValue("@OriginLat", OriginLat);
                        cmd.Parameters.AddWithValue("@OriginAlt", OriginAlt);
                        cmd.Parameters.AddWithValue("@DestinationLat", DestinationLat);
                        cmd.Parameters.AddWithValue("@DestinationAlt", DestinationAlt);
                        cmd.Parameters.AddWithValue("@Passengers", Passengers);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        newID = cmd.ExecuteScalar();

                        if (newID != null && newID.ToString().Length > 0)
                        {
                            return new ResponseModel
                            {
                                IsSuccess = true,
                                Message = "El viaje fue agregada con éxito",
                                Result = newID
                            };
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                IsSuccess = false,
                                Message = $"Se generó un error al insertar el viaje",
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
                    Message = $"Se generó un error al insertar el viaje ({ex.Message})",
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
                    string tsql = "UPDATE Ride SET OriginStr = @OriginStr, DestinationStr = @DestinationStr, OriginLat = @OriginLat, OriginAlt = @OriginAlt, DestinationLat = @DestinationLat, DestinationAlt = @DestinationAlt, Passengers = @Passengers, Date = @Date, Price = @Price WHERE IDRide = @IDRide;";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@OriginStr", OriginStr);
                        cmd.Parameters.AddWithValue("@DestinationStr", DestinationStr);
                        cmd.Parameters.AddWithValue("@OriginLat", OriginLat);
                        cmd.Parameters.AddWithValue("@OriginAlt", OriginAlt);
                        cmd.Parameters.AddWithValue("@DestinationLat", DestinationLat);
                        cmd.Parameters.AddWithValue("@DestinationAlt", DestinationAlt);
                        cmd.Parameters.AddWithValue("@Passengers", Passengers);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        cmd.Parameters.AddWithValue("@IDRide", IDRide);
                        cmd.ExecuteNonQuery();

                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "El viaje fue actualizado con éxito",
                            Result = IDRide
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al actualizar el viaje ({ex.Message})",
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
                    string tsql = "DELETE FROM Ride WHERE IDRide = @IDRide;";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@IDRide", id);
                        cmd.ExecuteNonQuery();

                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = $"El viaje fue eliminado",
                            Result = IDRide
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al eliminar el viaje ({ex.Message})",
                    Result = null
                };
            }
        }
    }
}
