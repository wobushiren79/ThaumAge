using UnityEditor;
using UnityEngine;

public class WeatherBase
{
    public float timeForCloud = 0;
    public float timeMaxForCloud = 30;

    public WeatherBean weatherData;

    public WeatherBase(WeatherBean weatherData)
    {
        this.weatherData = weatherData;
    }

    public virtual void Update()
    {

    }

    public virtual void ChangeAllCloudsColor(Color colorCloud,float changeTime)
    {
        SceneElementHandler.Instance.manager.clouds.ChangeCloudsColor(colorCloud, changeTime);
    }
}