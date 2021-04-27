using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class WeatherBean 
{
    public int weatherType;
    public float timeForWeather;
    public float timeMaxForWeather;

    public WeatherTypeEnum GetWeatherType()
    {
        return (WeatherTypeEnum)weatherType;
    }
    public void SetWeatherType(WeatherTypeEnum weatherType)
    {
        this.weatherType = (int)weatherType;
    }
}