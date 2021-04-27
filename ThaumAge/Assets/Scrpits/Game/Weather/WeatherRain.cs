using UnityEditor;
using UnityEngine;

public class WeatherRain : WeatherBase
{
    public WeatherRain(WeatherBean weatherData) : base(weatherData)
    {
    }
    public override void Update()
    {
        base.Update();
        HandleForClouds();
    }
    public void HandleForClouds()
    {
        timeForCloud -= Time.deltaTime;
        if (timeForCloud <= 0)
        {
            CreateClouds(new Color(0, 0, 0, 0.5f), 10, 30);
            timeForCloud = timeMaxForCloud;
        }
    }


}