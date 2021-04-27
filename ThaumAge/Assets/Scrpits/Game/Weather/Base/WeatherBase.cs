using UnityEditor;
using UnityEngine;

public class WeatherBase
{
    public float timeForCloud = 0;
    public float timeMaxForCloud = 10;
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
        Vector3 size = new Vector3(Random.Range(minSize, maxSize), 1, Random.Range(minSize, maxSize));
        WeatherHandler.Instance.manager.clouds.CreateCloud(size, colorCloud);
        timeForCloud = timeMaxForCloud;
    }
}