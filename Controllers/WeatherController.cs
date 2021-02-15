using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string key = "c4945cce5817f5dfa549956a86dfaf68";
        private static readonly string units = "metric";

        private async static Task<JObject> GetAnswer(string type, string city)
        {
            if(String.IsNullOrEmpty(city))
                throw new HttpResponseException(400, "city field is empty");
            string url = "http://api.openweathermap.org/data/2.5/" + type + "?q=" + city + "&APPID=" + key + "&units=" + units;
            HttpResponseMessage response = await client.GetAsync(url);
            JObject answer = JObject.Parse(await response.Content.ReadAsStringAsync());
            if((int)answer["cod"] != 200)
                throw new HttpResponseException((int)answer["cod"], (string)answer["message"]);
            return answer;
        }

        [HttpGet("GetCurrentWeather/{city}")]
        public async Task<CurrentWeather> GetCurrentWeather(string city)
        {            
            JObject answer = await GetAnswer("weather", city);
            return new CurrentWeather
            {
                Date = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((int)answer["dt"] * TimeSpan.TicksPerSecond)).ToLocalTime(),
                Temperature = (float)answer["main"]["temp"],
                WindSpeed = (float)answer["wind"]["speed"],
                Cloudiness = (int)answer["clouds"]["all"],
                Description = (string)answer["weather"][0]["description"],
            };
        }

        [HttpGet("GetWeatherForecast/{city}")]
        public async Task<List<WeatherForecast> > GetWeatherForecast(string city)
        {
            JObject answer = await GetAnswer("forecast", city);
            List<WeatherForecast> result = new List<WeatherForecast>();
            foreach(var i in answer["list"])
                result.Add(new WeatherForecast
                {
                    Date = DateTime.Parse((string)i["dt_txt"]),
                    MaxTemperature = (float)i["main"]["temp_max"],
                    MinTemperature = (float)i["main"]["temp_min"],
                    WindSpeed = (float)i["wind"]["speed"],
                    Cloudiness = (int)i["clouds"]["all"],
                    Description = (string)i["weather"][0]["description"],
                });
            return result;
        }

        [HttpGet("GetCompareWeather/{firstCity},{secondCity}")]
        public async Task<ComparerWeather> GetCompareWeather(string firstCity, string secondCity)
        {            
            JObject answerFirstCity = await GetAnswer("weather", firstCity);
            JObject answerSecondCity = await GetAnswer("weather", secondCity);
            return new ComparerWeather(
                new CurrentWeather
                    {
                        Date = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((int)answerFirstCity["dt"] * TimeSpan.TicksPerSecond)).ToLocalTime(),
                        Temperature = (float)answerFirstCity["main"]["temp"],
                        WindSpeed = (float)answerFirstCity["wind"]["speed"],
                        Cloudiness = (int)answerFirstCity["clouds"]["all"],
                        Description = (string)answerFirstCity["weather"][0]["description"],
                    },
                new CurrentWeather
                    {
                        Date = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((int)answerSecondCity["dt"] * TimeSpan.TicksPerSecond)).ToLocalTime(),
                        Temperature = (float)answerSecondCity["main"]["temp"],
                        WindSpeed = (float)answerSecondCity["wind"]["speed"],
                        Cloudiness = (int)answerSecondCity["clouds"]["all"],
                        Description = (string)answerSecondCity["weather"][0]["description"],
                    }
                );
        }
    }
}
