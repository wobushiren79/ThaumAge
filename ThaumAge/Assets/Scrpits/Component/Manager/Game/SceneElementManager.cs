using UnityEditor;
using UnityEngine;

public class SceneElementManager : BaseManager
{
    protected SceneElementStar _star;
    protected SceneElementSky _sky;
    protected SceneElementClouds _clouds;

    public SceneElementStar star
    {
        get
        {
            if (_star == null)
            {
                _star = FindWithTag<SceneElementStar>(TagInfo.Tag_SceneElementStar);

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
                _sky = FindWithTag<SceneElementSky>(TagInfo.Tag_SceneElementSky);

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
                _clouds = FindWithTag<SceneElementClouds>(TagInfo.Tag_SceneElementClouds);
            }
            return _clouds;
        }
    }
}