using WeatherForecastRB.Models;

namespace WeatherForecastRB.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Day> Days { get; set; } = new List<Day>();
        public Month month_av_data = new Month();
    }
}
