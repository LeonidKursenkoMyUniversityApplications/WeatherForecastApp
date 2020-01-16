using System;
using WeatherForecast;

namespace AddWeather.Event
{
    public class DayWeatherAddedEventArgs : EventArgs
    {
        public DayWeather DayWeather { set; get; }
    }
}
