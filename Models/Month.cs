using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherForecastRB.Models
{
    public class Month
    {
        public string Text { get; set; }
        public double Average_Temperature_Day { get; set; }
        public double Average_Temperature_Night { get; set; }
        public double Average_Preasure { get; set; }
        public double Average_Humidity { get; set; }
        public Month()
        {
            Text = "Среднее значение";
            Average_Temperature_Day = 1;
            Average_Temperature_Night = 1;
            Average_Preasure = 1;
            Average_Humidity = 1;
        }
        public Month(double average_temperature_day, double average_temperature_night, double average_preasure, double average_humidity)
        {
            Text = "Среднее значение";
            Average_Temperature_Day = average_temperature_day;
            Average_Temperature_Night = average_temperature_night;
            Average_Preasure = average_preasure;
            Average_Humidity = average_humidity;
        }
    }
}
