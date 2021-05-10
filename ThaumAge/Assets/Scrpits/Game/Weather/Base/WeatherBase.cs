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

    public virtual void CreateClouds(Color colorCloud, int minSize, int maxSize)
    {
        Vector3 size = new Vector3(Random.Range(minSize, maxSize), Random.Range(5, 10), Random.Range(minSize, maxSize));
        SceneElementHandler.Instance.manager.clouds.CreateCloud(size, colorCloud);
        timeForCloud = timeMaxForCloud;
    }
}