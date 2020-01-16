using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.EF.WeatherForecast.Repository;
using DAL.WeatherForecast.Interface;
using DAL.WeatherForecast.Repository;
using DAL.WeatherForecast.Entity;
using CustomWeather = WeatherForecast;

namespace BLL.WeatherForecast.Controller
{
    public class WeatherService
    {
        private readonly IImagePathRepository _imagePathRepository;
        private readonly IDayWeatherRepository _dayWeatherRepository;
        private readonly IMapper _mapper;

        public WeatherService(IImagePathRepository imagePathRepository, IDayWeatherRepository dayWeatherRepository)
        {
            try
            {
                //_imagePathRepository = new ImagePathRepository("weatherConnStr");
                //_dayWeatherRepository = new DayWeatherRepository("weatherConnStr");
                //_imagePathRepository = new ImagePathEfRepository();
                //_dayWeatherRepository = new DayWeatherEfRepository();
                _imagePathRepository = imagePathRepository;
                _dayWeatherRepository = dayWeatherRepository;
            }
            catch (Exception ex)
            {
                throw new Exception(MyStrings.ConnError + ex.Message, ex);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ImagePath, CustomWeather.ImagePath>();
                cfg.CreateMap<DayWeather, CustomWeather.DayWeather>()
                .ForMember(dest => dest.Weather,
                          opts => opts.MapFrom(src => GetWeatherType(_imagePathRepository.Get(src.IdImagePath))));
            });
            _mapper = config.CreateMapper();
        }

        public List<CustomWeather.ImagePath> GetImagePathList()
        {
            var listFromDb = _imagePathRepository.GetList().ToList();
            return listFromDb.Select(_mapper.Map<CustomWeather.ImagePath>).ToList();
        }

        private CustomWeather.Weather  GetWeatherType(ImagePath item)
        {
            CustomWeather.Weather weather;
            switch (item.Weather)
            {
                case "Sunny":
                    weather = CustomWeather.Weather.Sunny;
                    break;
                case "Rainy":
                    weather = CustomWeather.Weather.Rainy;
                    break;
                default:
                    weather = CustomWeather.Weather.Cloudy;
                    break;
            }
            return weather;
        }

        public List<CustomWeather.DayWeather> GetDayWeatherList()
        {
            var listFromDb = _dayWeatherRepository.GetList().ToList();
            return listFromDb.Select(_mapper.Map<CustomWeather.DayWeather>).ToList();
        }

        public void AddDayWeather(CustomWeather.DayWeather dayWeather)
        {
            var list = _imagePathRepository.GetList().ToList();
            int id = list.First(x => x.Weather == dayWeather.Weather.ToString()).Id;
            var item = new DayWeather()
            {
                Id = _dayWeatherRepository.GetList().ToList().Count + 1,
                Day = dayWeather.Day,
                DayTemperature = dayWeather.DayTemperature,
                NightTemperature = dayWeather.NightTemperature,
                IdImagePath = id
            };
            _dayWeatherRepository.Create(item);
            _dayWeatherRepository.Save();
        }
    }
}
