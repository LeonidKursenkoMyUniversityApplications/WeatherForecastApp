using authorAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AddWeather.Event;
using BLL.WeatherForecast.Controller;
using DAL.WeatherForecast.Interface;
using WeatherForecast;
using WeatherForecast.Event;
using WeatherForecast.Exception;
using WeatherForecast.Store;

namespace PLWeatherApp
{
    public partial class Form1 : Form
    {
        private AddWeather.AddDayWeather addDayWeather1;
        private DayWeatherStore _dayWeatherStore;
        private readonly IImagePathRepository _imagePathRepository;
        private readonly IDayWeatherRepository _dayWeatherRepository;

        public Form1(IImagePathRepository imagePathRepository, IDayWeatherRepository dayWeatherRepository)
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Language))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            }
            InitializeComponent();
            _imagePathRepository = imagePathRepository;
            _dayWeatherRepository = dayWeatherRepository;
            AddDayWeather();
            LoadData();
            
        }

        private void AddDayWeather()
        {
            this.addDayWeather1 = new AddWeather.AddDayWeather(_imagePathRepository, _dayWeatherRepository);
            this.addDayWeather1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addDayWeather1.Location = new System.Drawing.Point(3, 3);
            this.addDayWeather1.Name = "addDayWeather1";
            this.addDayWeather1.Size = new System.Drawing.Size(950, 455);
            this.addDayWeather1.TabIndex = 0;
            this.tabPage2.Controls.Add(this.addDayWeather1);
            addDayWeather1.DayWeatherAdded += OnDayWeatherAdded;
        }
        
        private async void LoadData()
        {
            comboBox1.DataSource = new System.Globalization.CultureInfo[]
            {
                System.Globalization.CultureInfo.GetCultureInfo("en"),
                System.Globalization.CultureInfo.GetCultureInfo("uk"),
                System.Globalization.CultureInfo.GetCultureInfo("ru")
            };
            comboBox1.DisplayMember = "NativeName";
            comboBox1.ValueMember = "Name";
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Language))
                comboBox1.SelectedValue = Properties.Settings.Default.Language;
            try
            {
                var weatherService = new WeatherService(_imagePathRepository, _dayWeatherRepository);
                weatherView1.ResourcesPath = weatherService.GetImagePathList();
                weatherView1.Items = await LoadDataAsync(weatherService);
                //weatherView1.Items = weatherService.GetDayWeatherList();

                _dayWeatherStore = new DayWeatherStore
                {
                    Days = weatherView1.Items
                };
                InitFilterListBox();
            }
            catch(Exception ex)
            {
                MessageBox.Show(MyStrings.IncorrectDataLoading + " " + ex.Message);
            }
        }

        private Task<List<DayWeather>> LoadDataAsync(WeatherService weatherService)
        {
            return Task.Run(() =>
            {
                var list = weatherService.GetDayWeatherList();
                return list;
            });
        }

        private void FilterShow()
        {
            int index = filterListBox.SelectedIndex;
            if (index == 0)
            {
                var list = _dayWeatherStore.Days;
                weatherView1.Items = list;
            }
            else
            {
                Weather weather = (Weather)filterListBox.Items[index];
                try
                {
                    var list = _dayWeatherStore.Filter(day => day.Weather == weather);
                    weatherView1.Items = list;
                }
                catch (WeatherViewException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void AboutComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutComponent().Show();
        }

        delegate List<DayWeather> FilterShowHandler(int selectedIndex);
        private List<DayWeather> FilterShow(int index)
        {
            List<DayWeather> list = null;
            if (index == 0)
            {
                list = _dayWeatherStore.Days;
            }
            else
            {
                Weather weather = (Weather)(index - 1);
                try
                {
                    list = _dayWeatherStore.Filter(day => day.Weather == weather);
                }
                catch (WeatherViewException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return list;
        }
        
        private FilterShowHandler handler;
        private List<DayWeather> dayWeathers;
        private void FilterListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterShow();
            //handler = new FilterShowHandler(FilterShow);
            //weatherView1.Enabled = false;
            //filterListBox.Enabled = false;
            //handler.BeginInvoke(filterListBox.SelectedIndex, new AsyncCallback(EndInvokeAsync), null);
            ////weatherView1.Items = handler.EndInvoke(resultOb);
            //weatherView1.Items = dayWeathers;
            //filterListBox.Enabled = true;
            //weatherView1.Enabled = true;
        }

        private void EndInvokeAsync(IAsyncResult resultOb)
        {
            dayWeathers = handler.EndInvoke(resultOb);
        }

        private void InitFilterListBox()
        {
            var list = weatherView1.ResourcesPath.Select(x => x.Weather);
            filterListBox.Items.Add(MyStrings.All);
            foreach (var li in list)
            {
                filterListBox.Items.Add(li);
            }
            filterListBox.SelectedIndex = 0;
        }

        private void SortButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                weatherView1.Items = _dayWeatherStore.Sort<DateTime>(d => d.Day);
            }
            catch (WeatherViewException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            weatherView1.DataReceived += OnDataReceive;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            weatherView1.DataReceived -= OnDataReceive;
        }

        private static void OnDataReceive(object sender, DataReceivedEventArgs args)
        {
            MessageBox.Show(args.Message);
        }

        private void OnDayWeatherAdded(object sender, DayWeatherAddedEventArgs e)
        {
            LoadData();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Language = comboBox1.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void MEFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2(_imagePathRepository, _dayWeatherRepository).Show();
        }
    }

}

