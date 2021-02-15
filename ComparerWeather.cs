using System;

namespace WeatherAPI
{
    public class ComparerWeather
    {
        public string Temperature { get; set; }
        public string WindSpeed { get; set; }
        public string Cloudiness { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public ComparerWeather(CurrentWeather firstCity, CurrentWeather secondCity)
        {
            Temperature = firstCity.Temperature + "C vs " + secondCity.Temperature + "C";
            WindSpeed = firstCity.WindSpeed + " m/s vs " + secondCity.WindSpeed + " m/s";
            Cloudiness = firstCity.Cloudiness + "% vs " + secondCity.Cloudiness + "%";
            Description = firstCity.Description + " vs " + secondCity.Description;
            Date = firstCity.Date;
        }
    }
}