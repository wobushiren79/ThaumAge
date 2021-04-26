using UnityEditor;
using UnityEngine;

public class WeatherManager : BaseManager
{
    protected Clouds _clouds;

    public WeatherBase currentWeather;

    public Clouds clouds
    { 
        get
        {
            if (_clouds == null)
            {
                _clouds = FindWithTag<Clouds>(TagInfo.Tag_Clouds);
            }
            return _clouds;
        }
    }

    /// <summary>
    /// 设置天气数据
    /// </summary>
    /// <param name="weatherData"></param>
    public void SetWeatherData(WeatherBean weatherData)
    {
        WeatherTypeEnum weatherType=  weatherData.GetWeatherType();
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
    public WeatherBean GetWeatherData()
    {
        return currentWeather.weatherData;
    }
}