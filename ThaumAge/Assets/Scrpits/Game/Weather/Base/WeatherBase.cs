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
        Vector3 startPosition = GameHandler.Instance.manager.player.transform.position + new Vector3(100, 0, Random.Range(-100, 100));
        Vector3 size = new Vector3(Random.Range(minSize, maxSize), 1, Random.Range(minSize, maxSize));
        WeatherHandler.Instance.manager.clouds.CreateCloud(startPosition, size, colorCloud);
        timeForCloud = timeMaxForCloud;
    }
}