using UnityEditor;
using UnityEngine;

public class WeatherRain : WeatherBase
{
    public Color colorClouds;

    public WeatherRain(WeatherBean weatherData) : base(weatherData)
    {
        InitRain();
        InitClouds();
    }

    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 初始化雨
    /// </summary>
    public void InitRain()
    {
        SceneElementHandler.Instance.manager.SetRainActive(true);
        SceneElementHandler.Instance.manager.rain.SetData(Random.Range(100, 2000));
    }

    /// <summary>
    /// 初始化云
    /// </summary>
    public void InitClouds()
    {
        colorClouds = new Color(0, 0, 0, 0.5f);
        ChangeAllCloudsColor(colorClouds, 0.5f);
    }

}