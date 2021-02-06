using System;

namespace WeatherAPI
{
    public class Weather
    {
        public float WindSpeed { get; set; }
        public int Cloudiness { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return  "\nДата: " + Date + "\nСкорость ветра: " + WindSpeed + " м/с\nОблачность: " + Cloudiness + "%";
        }
    }
}
