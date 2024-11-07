using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastRB.Models;
using WeatherForecastRB.ViewModels;

namespace WeatherForecastRB.Controllers
{
    public class HomeController : Controller
    {
        List<Day> days_result;
        Month month_av = new Month();

        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            return doc;
        }

        static List<Day> GetDays(string url_month, string city, string month, string year, out Month month_av_local)
        {
            HtmlDocument doc = GetDocument(url_month);

            month_av_local = new Month();
            var days = new List<Day>();

            int day_count;
            if ((year == "2024" && month == "november") || (year == "2024" && month == "december")) { day_count = -1; }
            else if (month == "february" && Convert.ToInt32(year) % 4 == 0) { day_count = 29; }
            else if (month == "february") { day_count = 28; }
            else if (month == "april" || month == "june" || month == "september" || month == "november") { day_count = 30; }
            else { day_count = 31; }


            for (int day_i = 1; day_i <= day_count; day_i++)
            {
                var dateXPath = $"//div[@class='day day_calendar']/a[@href =\"/prognoz/{city}/{Convert.ToString(day_i)}-{month}/\"]/div/div[contains(concat(@class,\"\"),\"day__date\")]";
                var temperatureXPath = $"//div[@class='day day_calendar']/a[@href =\"/prognoz/{city}/{Convert.ToString(day_i)}-{month}/\"]/div/div[@class=\"day__temperature\"]";
                var preasureXPath = $"//div[@class='day day_calendar']/a[@href =\"/prognoz/{city}/{Convert.ToString(day_i)}-{month}/\"]/div/div/div/span[contains(concat(\" \",@title,\" \"),\"Давление: \")]";
                var humidityXPath = $"//div[@class='day day_calendar']/a[@href =\"/prognoz/{city}/{Convert.ToString(day_i)}-{month}/\"]/div/div/div/span[contains(concat(\" \",@title,\" \"),\"Влажность: \")]";

                Day day = new Day();

                day.Date = doc.DocumentNode.SelectSingleNode(dateXPath).InnerText;

                string temp_t = doc.DocumentNode.SelectSingleNode(temperatureXPath).InnerText;
                string[] temp_d_n = Regex.Replace(temp_t, @"([\t\n&deg])", "").TrimEnd(';').Split(new char[] { ';' });
                string temp_temperature = doc.DocumentNode.SelectSingleNode(temperatureXPath).InnerText;

                day.Temperature_Day = Convert.ToInt32(temp_d_n[0]);
                day.Temperature_Night = Convert.ToInt32(temp_d_n[1]);

                string temp_preasure = doc.DocumentNode.SelectSingleNode(preasureXPath).InnerText;
                day.Preasure = Convert.ToInt32(temp_preasure.Trim(new Char[] { '\t', '\n' }).TrimEnd(' ', 'м'));

                string temp_humidity = doc.DocumentNode.SelectSingleNode(humidityXPath).InnerText;
                day.Humidity = Convert.ToInt32(temp_humidity.Trim(new Char[] { '\t', '\n' }).TrimEnd('%'));

                month_av_local.Average_Temperature_Day += day.Temperature_Day;
                month_av_local.Average_Temperature_Night += day.Temperature_Night;
                month_av_local.Average_Preasure += day.Preasure;
                month_av_local.Average_Humidity += day.Humidity;

                days.Add(day);

            }

            month_av_local.Average_Temperature_Day = Math.Round(month_av_local.Average_Temperature_Day/ day_count, 2);
            month_av_local.Average_Temperature_Night = Math.Round(month_av_local.Average_Temperature_Night / day_count, 2);
            month_av_local.Average_Preasure = Math.Round(month_av_local.Average_Preasure / day_count, 2);
            month_av_local.Average_Humidity = Math.Round(month_av_local.Average_Humidity / day_count, 2);

            return days;

        }

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(int? companyId, string city = "gomel", string month = "october", string year = "2024")
        {
            days_result = GetDays($"https://pogoda.mail.ru/prognoz/{city}/{month}-{year}/", city, month, year, out month_av);
            
            ViewData["SelectedCity"] = city;
            ViewData["SelectedMonth"] = month;
            ViewData["SelectedYear"] = year;

            IndexViewModel viewModel = new() { Days = days_result, month_av_data = month_av };

            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
