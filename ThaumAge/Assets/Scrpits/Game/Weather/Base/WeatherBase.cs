using UnityEditor;
using UnityEngine;

public class WeatherBase
{
    public float timeForCloud = 0;
    public float timeMaxForCloud = 30;

    public Color colorClouds;

    public WeatherBean weatherData;

    public WeatherBase(WeatherBean weatherData)
    {
        this.weatherData = weatherData;
    }

    public virtual void Update()
    {

    }
}