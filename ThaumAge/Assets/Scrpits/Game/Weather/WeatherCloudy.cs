using UnityEditor;
using UnityEngine;

public class WeatherCloudy : WeatherBase
{

    public WeatherCloudy(WeatherBean weatherData) : base(weatherData)
    {
        InitClouds();
    }

    public override void Update()
    {
        base.Update();
    }


    /// <summary>
    /// 初始化云
    /// </summary>
    public void InitClouds()
    {
        SceneElementHandler.Instance.manager.SetCloudAction(true);
        colorClouds = new Color(0, 0, 0, 0.5f);
        ChangeAllCloudsColor(colorClouds, 0.5f);
    }


}