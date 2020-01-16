using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast;

namespace AddWeather
{
    [Serializable]
    public class SaveState
    {
        public int WeatherId { get; set; }
        public string DayTemperature { get; set; }
        public string NightTemperature { get; set; }
        public int _some;

        public SaveState()
        {
            
        }

        public SaveState(int weatherId, string dayTemperature, string nightTemperature, int some)
        {
            WeatherId = weatherId;
            DayTemperature = dayTemperature;
            NightTemperature = nightTemperature;
            _some = some;
        }
    }
}
