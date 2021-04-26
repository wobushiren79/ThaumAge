using UnityEditor;
using UnityEngine;

public class WeatherBase 
{
    public WeatherBean weatherData;

    public WeatherBase(WeatherBean weatherData)
    {
        this.weatherData = weatherData;
    }
}