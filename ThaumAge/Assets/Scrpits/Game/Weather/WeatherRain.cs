using UnityEditor;
using UnityEngine;

public class WeatherRain : WeatherBase
{

    public WeatherRain(WeatherBean weatherData) : base(weatherData)
    {
        InitRain();
    }


    /// <summary>
    /// 初始化雨
    /// </summary>
    public void InitRain()
    {
        SceneElementHandler.Instance.manager.SetRainActive(true);
        SceneElementHandler.Instance.manager.rain.SetData(Random.Range(100, 2000));
    }



}