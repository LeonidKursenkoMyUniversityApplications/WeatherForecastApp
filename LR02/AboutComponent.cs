using authorAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherForecast;

namespace PLWeatherApp
{
    public partial class AboutComponent : Form
    {
        public AboutComponent()
        {
            InitializeComponent();
            label1.Text = "";
            PrintAuthorInfo(typeof(DayWeather));
            PrintAuthorInfo(typeof(WeatherView));
        }

        private void PrintAuthorInfo(System.Type t)
        {
            label1.Text += "Author information for " + t;

            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);

            foreach (System.Attribute attr in attrs)
            {
                if (attr is Author)
                {
                    Author a = (Author)attr;
                    label1.Text += $" {a.GetName()}, version {a.version}\n";
                }
            }
        }
    }
}
