using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherForecastRB.Models
{
    public class Day
    {
        public string Date { get; set; }
        public int Temperature_Day { get; set; }
        public int Temperature_Night { get; set; }
        public int Preasure { get; set; }
        public int Humidity { get; set; }
        public Day()
        {
            Date = "26 июня 2001";
            Temperature_Day = 26;
            Temperature_Night = 10;
            Preasure = 760;
            Humidity = 34;
        }
        public Day(string date, int temperature_day, int temperature_night, int preasure, int humidity)
        {
            Date = date;
            Temperature_Day = temperature_day;
            Temperature_Night = temperature_night;
            Preasure = preasure;
            Humidity = humidity;
        }
    }
}
