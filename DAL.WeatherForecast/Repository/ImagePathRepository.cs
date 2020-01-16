using DAL.WeatherForecast.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.WeatherForecast.Entity;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL.WeatherForecast.Repository
{
    public class ImagePathRepository : IImagePathRepository
    {
        private SqlConnection _connection = new SqlConnection();

        public ImagePathRepository(string connectionString)
        {
            _connection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }
        public void Create(ImagePath item)
        {            
            string sql = string.Format("Insert Into ImagePath" +
                "(Id, Weather, Path)" +
                "Values(@Id, @Weather, @Path");

            using (var cmd = new SqlCommand(sql, _connection))
            {
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Weather", item.Weather);
                cmd.Parameters.AddWithValue("@Path", item.Path);
                _connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Insert operation wasn`t successful." + ex.Message, ex);
                }
                finally
                {
                    _connection.Close();
                }
            }           
        }

        public void Delete(ImagePath item)
        {
            string sql = string.Format($"Delete from ImagePath where Id ='{item.Id}'");
            using (var cmd = new SqlCommand(sql, _connection))
            {
                _connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Delete operation wasn`t successful." + ex.Message, ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public ImagePath Get(int id)
        {
            ImagePath record = null;

            string sql = "Select * FROM ImagePath where Id = @Id";

            using (var cmd = new SqlCommand(sql, _connection))
            {
                _connection.Open();
                try
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    var reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                            record = GetRecord(reader);
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Find operation wasn`t successful." + ex.Message, ex);
                }
                finally
                {
                    _connection.Close();
                }
            }

            return record;
        }

        public IEnumerable<ImagePath> GetList()
        {
            var list = new List<ImagePath>();
            string sql = "Select * FROM ImagePath";
            using (var cmd = new SqlCommand(sql, _connection))
            {
                _connection.Open();
                try
                {
                    var reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                            list.Add(GetRecord(reader));
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Select operation wasn`t successful." + ex.Message, ex);
                }
                finally
                {
                    _connection.Close();
                }
            }

            return list.ToList();
        }

        public void Save()
        {
        }

        public void Update(ImagePath item)
        {
            string sql = string.Format("Update ImagePath Set " +
                "Weather = '@Weather', Path = '@Path'");

            using (var cmd = new SqlCommand(sql, _connection))
            {
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Weather", item.Weather);
                cmd.Parameters.AddWithValue("@Path", item.Path);
                _connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Update operation wasn`t successful." + ex.Message, ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        private ImagePath GetRecord(SqlDataReader reader)
        {
            return new ImagePath
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Weather = reader.GetString(reader.GetOrdinal("Weather")),
                Path = reader.GetString(reader.GetOrdinal("Path"))                
            };
        }
    }
}
