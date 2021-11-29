using UnityEditor;
using UnityEngine;

public class SceneElementManager : BaseManager
{
    protected SceneElementStar _star;
    protected SceneElementSky _sky;
    protected SceneElementClouds _clouds;
    protected SceneElementRain _rain;

    public SceneElementStar star
    {
        get
        {
            if (_star == null)
            {
                _star = CreateElement<SceneElementStar>("Star", "Assets/Prefabs/Game/Element/Star.prefab");

            }
            return _star;
        }
    }


    public SceneElementSky sky
    {
        get
        {
            if (_sky == null)
            {
                _sky = CreateElement<SceneElementSky>("Sky", "Assets/Prefabs/Game/Element/Sky.prefab");

            }
            return _sky;
        }
    }

    public SceneElementClouds clouds
    {
        get
        {
            if (_clouds == null)
            {
                _clouds = CreateElement<SceneElementClouds>("Clouds", "Assets/Prefabs/Game/Weather/Clouds.prefab");
            }
            return _clouds;
        }
    }


    public SceneElementRain rain
    {
        get
        {
            if (_rain == null)
            {
                _rain = CreateElement<SceneElementRain>("Rain", "Assets/Prefabs/Game/Weather/Rain.prefab");
            }
            return _rain;
        }
    }

    public void SetRainActive(bool active)
    {
        rain.gameObject.SetActive(active);
    }
    public void SetCloudAction(bool active)
    {
        clouds.gameObject.SetActive(active);
    }


    private T CreateElement<T>(string name,string key)
    {
        GameObject objModel = LoadAddressablesUtil.LoadAssetSync<GameObject>(key);
        GameObject objRain = Instantiate(gameObject, objModel);
        objRain.name = name;
        return objRain.GetComponent<T>();
    }
}