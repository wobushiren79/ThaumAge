using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeatherHandler : BaseHandler<WeatherHandler, WeatherManager>
{
    private void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForWeather();
        }
    }

    public void HandleForWeather()
    {
        WeatherBase weather = manager.GetWeather();
        weather.weatherData.timeForWeather -= Time.deltaTime;
        if (weather.weatherData.timeForWeather <= 0)
        {
            //根据世界类型获取天气
            List<WeatherTypeEnum> weatherTypes = manager.GetWeatherTypeListByWorldType(WorldTypeEnum.Main);
            ChangeWeather(RandomUtil.GetRandomDataByList(weatherTypes), 2000);
            SystemUtil.GCCollect();
        }
        //天气更新
        weather.Update();
    }

    /// <summary>
    /// 改变天气
    /// </summary>
    /// <param name="weatherType">天气类型</param>
    /// <param name="timeMaxForWeather"></param>
    public void ChangeWeather(WeatherTypeEnum weatherType, float timeMaxForWeather)
    {
        WeatherBean weatherData = new WeatherBean();
        weatherData.SetWeatherType(weatherType);
        weatherData.timeForWeather = timeMaxForWeather;
        weatherData.timeMaxForWeather = timeMaxForWeather;
        manager.SetWeatherData(weatherData);
    }

}