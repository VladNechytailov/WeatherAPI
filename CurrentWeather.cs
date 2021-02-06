namespace WeatherAPI
{
    public class CurrentWeather : Weather
    {
        public int Temperature { get; set; }

        public override string ToString()
        {
            return  base.ToString() + "\nТемпература воздуха: " + Temperature + "C";
        }
    }
}