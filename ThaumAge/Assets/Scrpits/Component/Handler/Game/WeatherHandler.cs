using UnityEditor;
using UnityEngine;

public class WeatherHandler : BaseHandler<WeatherHandler, WeatherManager>
{
    public float timeForWeather = 0;

    private void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForWeather();
        }
    }

    public void HandleForWeather()
    {
        WeatherBean weatherData = manager.GetWeatherData();
        timeForWeather -= Time.deltaTime;
        if (timeForWeather <= 0)
        {

            timeForWeather = weatherData.timeMaxForWeather;
        }
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
        weatherData.timeMaxForWeather = timeMaxForWeather;
        manager.SetWeatherData(weatherData);
    }

}