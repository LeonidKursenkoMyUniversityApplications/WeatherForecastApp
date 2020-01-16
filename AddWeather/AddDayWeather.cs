using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using AddWeather.Event;
using BLL.WeatherForecast.Controller;
using DAL.WeatherForecast.Interface;
using WeatherForecast;
using WeatherForecast.Event;

namespace AddWeather
{
    [Export(typeof(UserControl))]
    [DisplayName("Add")]
    public partial class AddDayWeather: UserControl
    {
        private WeatherService _weatherService;
        private List<WeatherForecast.DayWeather> dayWeatherList;
        private List<WeatherForecast.ImagePath> imagePathList;

        [ImportingConstructor]
        public AddDayWeather()
        {
            InitializeComponent();
        }

        public AddDayWeather(IImagePathRepository imagePathRepository, IDayWeatherRepository dayWeatherRepository)
        {
            InitializeComponent();
            InitAll(imagePathRepository, dayWeatherRepository);
        }

        public void InitAll(IImagePathRepository imagePathRepository, IDayWeatherRepository dayWeatherRepository)
        {
            try
            {
                _weatherService = new WeatherService(imagePathRepository, dayWeatherRepository);
                imagePathList = _weatherService.GetImagePathList();
                dayWeatherList = _weatherService.GetDayWeatherList();
                InitListBox();
                LoadUserConfig();
            }
            catch (Exception ex)
            {
                throw new Exception(MyStrings.OpenException + ex.Message, ex);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            SaveUserConfig();
            base.OnHandleDestroyed(e);

        }

        private void InitListBox()
        {
            var list = imagePathList.Select(x => x.Weather);
            foreach (var li in list)
            {
                weatherListBox.Items.Add(li);
            }
            weatherListBox.SelectedIndex = 0;
        }

        private string userConfigFile = "userConfig.xml";
        private void SaveUserConfig()
        {
            if(weatherListBox.Items.Count == 0) return;

            var saveState = new SaveState
            {
                WeatherId = weatherListBox.SelectedIndex,
                DayTemperature = dayTempTextBox.Text,
                NightTemperature = nightTempTextBox.Text
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(SaveState));
            using (var stream = new FileStream(userConfigFile, FileMode.Create))
            {
                serializer.Serialize(stream, saveState);
            }
        }

        private void LoadUserConfig()
        {

            SaveState saveState = null;

            XmlSerializer serialiser = new XmlSerializer(typeof(SaveState));
            if(File.Exists(userConfigFile) != true) return;
            using (var stream = new FileStream(userConfigFile, FileMode.Open))
            {
                saveState = (SaveState)serialiser.Deserialize(stream);
                
                weatherListBox.SelectedIndex = (saveState.WeatherId < weatherListBox.Items.Count || saveState.WeatherId > -1) ? saveState.WeatherId : 0;
                dayTempTextBox.Text = saveState.DayTemperature;
                nightTempTextBox.Text = saveState.NightTemperature;
            }


        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            WeatherForecast.DayWeather dayWeather;
            try
            {
                dayWeather = new WeatherForecast.DayWeather()
                {
                    Day = dateTimePicker1.Value,
                    Weather = (WeatherForecast.Weather)weatherListBox.SelectedItem,
                    DayTemperature = Convert.ToDouble(dayTempTextBox.Text),
                    NightTemperature = Convert.ToDouble(nightTempTextBox.Text)
                };

                _weatherService.AddDayWeather(dayWeather);                
            }
            catch (Exception ex)
            {

                MessageBox.Show(MyStrings.AddException + ex.Message);
                return;
            }
            var args = new DayWeatherAddedEventArgs()
            {
                DayWeather = dayWeather
            };
            OnDayWeatherAdded(args);
            MessageBox.Show(MyStrings.AddSuccess);
        }

        #region Events
        public event EventHandler<DayWeatherAddedEventArgs> DayWeatherAdded;
        #endregion

        #region Handlers
        protected virtual void OnDayWeatherAdded(DayWeatherAddedEventArgs e)
        {
            DayWeatherAdded?.Invoke(this, e);
        }
        #endregion
    }
}
