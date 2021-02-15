using System;

namespace WeatherAPI
{
    public class Weather
    {
        public float WindSpeed { get; set; }
        public int Cloudiness { get; set; }
        public string Description { get; set; } 
        public DateTime Date { get; set; }
    }
}
