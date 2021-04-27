using UnityEditor;
using UnityEngine;

public class WeatherCloudy : WeatherBase
{

    public WeatherCloudy(WeatherBean weatherData) : base(weatherData)
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
            CreateClouds(new Color(1, 1, 1, 0.5f), 10, 30);
            timeForCloud = timeMaxForCloud;
        }
    }
}