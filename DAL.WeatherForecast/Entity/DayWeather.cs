using System;

namespace DAL.WeatherForecast.Entity
{
    public class DayWeather
    {
        public int Id { set; get; }
        public DateTime Day { set; get; }
        public double DayTemperature { set; get; }
        public double NightTemperature { set; get; }
        public int IdImagePath { set; get; }
    }
}

