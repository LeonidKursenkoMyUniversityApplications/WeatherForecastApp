using DAL.WeatherForecast.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.WeatherForecast.Interface
{
    public interface IDayWeatherRepository : IRepository<DayWeather>
    {
    }
}
