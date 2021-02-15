using System;

namespace WeatherAPI
{
    public class HttpResponseException : Exception
    {
        public int Code { get; set; }

        public HttpResponseException(int code, string message) : base(message)
        {          
            Code = code;
        }
    }
}