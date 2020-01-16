using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AddWeather;
using BLL.WeatherForecast.Controller;
using DAL.WeatherForecast.Interface;
using WeatherForecast;
using WeatherForecast.Store;

namespace PLWeatherApp
{
    public partial class Form2 : Form
    {
        private DayWeatherStore _dayWeatherStore;
        private readonly IImagePathRepository _imagePathRepository;
        private readonly IDayWeatherRepository _dayWeatherRepository;

        [ImportMany(typeof(UserControl))]
        private IEnumerable<UserControl> _controls;

        public Form2(IImagePathRepository imagePathRepository, IDayWeatherRepository dayWeatherRepository)
        {
            InitializeComponent();
            _imagePathRepository = imagePathRepository;
            _dayWeatherRepository = dayWeatherRepository;
            Import();

            tabControl1.TabPages.Clear();
            foreach (UserControl uc in _controls)
            {
                TabPage tp = new TabPage();
                var displayNameAttribute = uc.GetType().GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                if (displayNameAttribute != null)
                    tp.Text = displayNameAttribute.DisplayName;

                if (uc is AddDayWeather addDayWeather)
                    addDayWeather.InitAll(_imagePathRepository, _dayWeatherRepository);

                if (uc is WeatherView weatherView)
                {
                    var weatherService = new WeatherService(_imagePathRepository, _dayWeatherRepository);
                    weatherView.ResourcesPath = weatherService.GetImagePathList();
                    weatherView.Items = weatherService.GetDayWeatherList();
                }

                uc.Dock = DockStyle.Fill;
                tp.Controls.Add(uc);
                tabControl1.TabPages.Add(tp);
            }
        }

        public void Import()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in all assemblies in
            //the same directory as the executing program
            catalog.Catalogs.Add(
                new DirectoryCatalog(
                    Path.GetDirectoryName(
                        Assembly.GetExecutingAssembly().Location)));
            //Create the CompositionContainer with the parts in the catalog
            CompositionContainer container = new CompositionContainer(catalog);
            //Fill the imports of this object
            container.ComposeParts(this);
        }
    }
}
