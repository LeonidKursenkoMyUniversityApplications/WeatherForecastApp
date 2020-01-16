using DAL.WeatherForecast.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using DAL.WeatherForecast.Entity;
using AutoMapper;

namespace DAL.EF.WeatherForecast.Repository
{
    public class DayWeatherEfRepository : IDayWeatherRepository
    {
        WeatherEntities _context;
        private IMapper _mapper;

        public DayWeatherEfRepository()
        {
            _context = new WeatherEntities();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DayWeather, DAL.WeatherForecast.Entity.DayWeather>().ReverseMap();
            });
            _mapper = config.CreateMapper();
        }

        public void Create(DAL.WeatherForecast.Entity.DayWeather item)
        {
            _context.DayWeather.Add(_mapper.Map<DayWeather>(item));
        }

        public void Delete(DAL.WeatherForecast.Entity.DayWeather item)
        {
            _context.DayWeather.Remove(_mapper.Map<DayWeather>(item));
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public DAL.WeatherForecast.Entity.DayWeather Get(int id)
        {
            var dayWeather = _context.DayWeather.First(dWeather => dWeather.Id == id);
            return _mapper.Map<DAL.WeatherForecast.Entity.DayWeather>(dayWeather);
        }

        public IEnumerable<DAL.WeatherForecast.Entity.DayWeather> GetList()
        {
            var list =  _context.DayWeather.ToList<DayWeather>();
            return _mapper.Map<List<DayWeather>, List<DAL.WeatherForecast.Entity.DayWeather>>(list);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(DAL.WeatherForecast.Entity.DayWeather item)
        {
            _context.Entry(_mapper.Map<DayWeather>(item)).State = EntityState.Modified;
        }
    }
}
