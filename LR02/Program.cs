using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.WeatherForecast.Interface;
using DAL.EF.WeatherForecast.Repository;
using DAL.WeatherForecast.Repository;
using Microsoft.Practices.Unity.Configuration;
using Unity;
using Unity.Injection;

namespace PLWeatherApp
{
    static class Program
    {
        private static Form1 form1;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IUnityContainer container = BuildContainer();
            IDayWeatherRepository dayWeatherRepository = (IDayWeatherRepository) container.Resolve<IDayWeatherRepository>();
            IImagePathRepository imagePathRepository = (IImagePathRepository) container.Resolve<IImagePathRepository>();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form1 = new Form1(imagePathRepository, dayWeatherRepository);
            Application.Run(form1);
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
