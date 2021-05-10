using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeatherManager : BaseManager
{

    //当前天气
    public WeatherBase currentWeather;
    //主世界天气
    public List<WeatherTypeEnum> listWeatherForMain = new List<WeatherTypeEnum>();


    /// <summary>
    /// 设置天气数据
    /// </summary>
    /// <param name="weatherData"></param>
    public void SetWeatherData(WeatherBean weatherData)
    {
        WeatherTypeEnum weatherType = weatherData.GetWeatherType();
        switch (weatherType)
        {
            case WeatherTypeEnum.Sunny:
                currentWeather = new WeatherSunny(weatherData);
                break;
            case WeatherTypeEnum.Cloudy:
                currentWeather = new WeatherCloudy(weatherData);
                break;
            case WeatherTypeEnum.Rain:
                currentWeather = new WeatherSunny(weatherData);
                break;
        }
    }

    /// <summary>
    /// 获取天气数据
    /// </summary>
    /// <returns></returns>
    public WeatherBase GetWeather()
    {
        return currentWeather;
    }

    /// <summary>
    /// 根据世界类型获取天气类型
    /// </summary>
    /// <param name="worldType"></param>
    public List<WeatherTypeEnum> GetWeatherTypeListByWorldType(WorldTypeEnum worldType)
    {
        List<WeatherTypeEnum> listWeatherType = new List<WeatherTypeEnum>();
        switch (worldType)
        {
            case WorldTypeEnum.Main:
                if (CheckUtil.ListIsNull(listWeatherForMain))
                {
                    listWeatherForMain.Add(WeatherTypeEnum.Sunny);
                    listWeatherForMain.Add(WeatherTypeEnum.Cloudy);
                    listWeatherForMain.Add(WeatherTypeEnum.Rain);
                }
                listWeatherType = listWeatherForMain;
                break;
        }
        return listWeatherType;
    }
}