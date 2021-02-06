namespace WeatherAPI
{
    public class WeatherForecast : Weather
    {
        public int MaxTemperature { get; set; }
        public int MinTemperature { get; set; }

        public override string ToString()
        {
            return  base.ToString() + "\nТемпература воздуха(мин.): " + MinTemperature + "C"
                + "\nТемпература воздуха(макс.): " + MaxTemperature + "C";
        }
    }
}
