using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.WeatherForecast.Controller;
using Microsoft.Practices.Unity.Configuration;
using Unity;
using DAL.WeatherForecast.Interface;

namespace FillDb
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //try
            //{
                IUnityContainer container = BuildContainer();
                IDayWeatherRepository dayWeatherRepository = (IDayWeatherRepository)container.Resolve<IDayWeatherRepository>();
                IImagePathRepository imagePathRepository = (IImagePathRepository)container.Resolve<IImagePathRepository>();
                var weatherService = new WeatherService(imagePathRepository, dayWeatherRepository);
                var dayWeatherList = weatherService.GetDayWeatherList();
                WeatherForecast.DayWeather dayWeather;
                for (int i = 0; i < 500000; i++)
                {
                    dayWeather = new WeatherForecast.DayWeather()
                    {
                        Day = new DateTime(i + 1, i % 12 + 1, i % 25 + 1),
                        Weather = WeatherForecast.Weather.Cloudy,
                        DayTemperature =  i % 60 - 10,
                        NightTemperature = i % 60 - 29
                    };
                    //Console.WriteLine($"{dayWeather.Day.ToString()}, {dayWeather.Weather}, {dayWeather.DayTemperature}");
                    weatherService.AddDayWeather(dayWeather);
                }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            Console.WriteLine("End");
            Console.ReadKey();
        }

        public static IUnityContainer BuildContainer()
        {
            IUnityContainer container = new UnityContainer();
            //container.RegisterType<IDayWeatherRepository, DayWeatherEfRepository>();
            //container.RegisterType<IImagePathRepository, ImagePathEfRepository>();
            //container.RegisterType<IDayWeatherRepository, DayWeatherRepository>(new InjectionConstructor("weatherConnStr"));
            //container.RegisterType<IImagePathRepository, ImagePathRepository>(new InjectionConstructor("weatherConnStr"));
            container.LoadConfiguration();
            return container;
        }
    }
}
