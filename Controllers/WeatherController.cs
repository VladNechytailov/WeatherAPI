using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string key = "c4945cce5817f5dfa549956a86dfaf68";
        private static readonly string units = "metric";

        private async static Task<JObject> GetAnswer(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            JObject answer = JObject.Parse(await response.Content.ReadAsStringAsync());
            if((int)answer["cod"] != 200)
                throw new Exception("Response code: " + (string)answer["cod"] + "\nMessage: " + (string)answer["message"] + ".");
            return answer;
        }

        [HttpGet("GetCurrentWeather")]
        public async Task<string> GetCurrentWeather(string city)
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&APPID=" + key + "&units=" + units;
            try
            {
                JObject answer = await GetAnswer(url);
                return "Погода в данный момент\n" + new CurrentWeather
                {
                    Date = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((int)answer["dt"] * TimeSpan.TicksPerSecond)).ToLocalTime(),
                    Temperature = (int)answer["main"]["temp"],
                    WindSpeed = (float)answer["wind"]["speed"],
                    Cloudiness = (int)answer["clouds"]["all"],
                }.ToString();
            }
            catch(Exception e) {
                return e.Message;
            }            
        }

        [HttpGet("GetWeatherForecast")]
        public async Task<string> GetWeatherForecast(string city)
        {
            string url = "http://api.openweathermap.org/data/2.5/forecast?q=" + city + "&APPID=" + key + "&units=" + units;
            try
            {
                JObject answer = await GetAnswer(url);
                string result = "Прогноз погоды на ближайшие 5 дней";
                foreach(var i in answer["list"])
                    result += "\n" + new WeatherForecast
                    {
                        Date = DateTime.Parse((string)i["dt_txt"]),
                        MaxTemperature = (int)i["main"]["temp_max"],
                        MinTemperature = (int)i["main"]["temp_min"],
                        WindSpeed = (float)i["wind"]["speed"],
                        Cloudiness = (int)i["clouds"]["all"],
                    }.ToString();
                return result;
            }
            catch(Exception e) {
                return e.Message;
            }
        }
    }
}
