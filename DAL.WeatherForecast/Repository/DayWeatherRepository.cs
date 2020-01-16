using DAL.WeatherForecast.Entity;
using DAL.WeatherForecast.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace DAL.WeatherForecast.Repository
{
    public class DayWeatherRepository : IDayWeatherRepository
    {
        private SqlConnection _connection = new SqlConnection();

        public DayWeatherRepository(string connectionString)
        {
            _connection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }
        
        #region Implemented methods
        public void Create(DayWeather item)
        {
            string sql = string.Format("Insert Into DayWeather" +
                "(Id, Day, DayTemperature, NightTemperature, IdImagePath)" +
                "Values(@Id, @Day, @DayTemperature, @NightTemperature, @IdImagePath)");

            using (var cmd = new SqlCommand(sql, _connection))
            {
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Day", item.Day);
                cmd.Parameters.AddWithValue("@DayTemperature", item.DayTemperature);
                cmd.Parameters.AddWithValue("@NightTemperature", item.NightTemperature);
                cmd.Parameters.AddWithValue("@IdImagePath", item.IdImagePath);
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

        public void Delete(DayWeather item)
        {
            string sql = string.Format($"Delete from DayWeather where Id ='{item.Id}'");
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

        public void Save()
        {
        }

        public void Update(DayWeather item)
        {
            string sql = string.Format("Update DayWeather Set " +
                "Day = '@Day', DayTemperature = '@DayTemperature', NightTemperature = '@NightTemperature', " +
                "IdImagePath = '@IdImagePath'");

            using (var cmd = new SqlCommand(sql, _connection))
            {
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Day", item.Day);
                cmd.Parameters.AddWithValue("@DayTemperature", item.DayTemperature);
                cmd.Parameters.AddWithValue("@NightTemperature", item.NightTemperature);
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

        public void Dispose()
        {
            _connection.Dispose();
        }

        public DayWeather Get(int id)
        {
            DayWeather record = null;

            string sql = string.Format("Select * FROM DayWeather where Id = @Id");

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

        public IEnumerable<DayWeather> GetList()
        {
            var list = new List<DayWeather>();

            string sql = string.Format("Select * FROM DayWeather");

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
        #endregion
        
        private DayWeather GetRecord(SqlDataReader reader)
        {
            string str = reader.GetDateTime(reader.GetOrdinal("Day")).ToString();
            DateTime date = DateTime.Parse(str);
            return new DayWeather
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Day = date,
                DayTemperature = reader.GetDouble(reader.GetOrdinal("DayTemperature")),
                NightTemperature = reader.GetDouble(reader.GetOrdinal("NightTemperature")),
                IdImagePath = reader.GetInt32(reader.GetOrdinal("IdImagePath"))
            };
        }

        
    }
}
