using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using DAL.WeatherForecast.Entity;
using DAL.WeatherForecast.Interface;

namespace DAL.EF.WeatherForecast.Repository
{
    public class ImagePathEfRepository : IImagePathRepository
    {
        private WeatherEntities _context;
        private IMapper _mapper;

        public ImagePathEfRepository()
        {
            _context = new WeatherEntities();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ImagePath, DAL.WeatherForecast.Entity.ImagePath>().ReverseMap();
            });
            _mapper = config.CreateMapper();
        }

        public void Create(DAL.WeatherForecast.Entity.ImagePath item)
        {
            _context.ImagePath.Add(_mapper.Map<ImagePath>(item));
        }

        public void Delete(DAL.WeatherForecast.Entity.ImagePath item)
        {
            _context.ImagePath.Remove(_mapper.Map<ImagePath>(item));
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public DAL.WeatherForecast.Entity.ImagePath Get(int id)
        {
            var imagePath = _context.ImagePath.First(imPath => imPath.Id == id);
            return _mapper.Map<DAL.WeatherForecast.Entity.ImagePath>(imagePath);
        }

        public IEnumerable<DAL.WeatherForecast.Entity.ImagePath> GetList()
        {
            var list = _context.ImagePath.ToList<ImagePath>();
            return _mapper.Map<List<ImagePath>, List<DAL.WeatherForecast.Entity.ImagePath>>(list);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(DAL.WeatherForecast.Entity.ImagePath item)
        {
            _context.Entry(_mapper.Map<ImagePath>(item)).State = EntityState.Modified;
        }
    }
}
