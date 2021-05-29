using UnityEditor;
using UnityEngine;

public class LightManager : BaseManager
{
    protected Light _sunLight;
    protected Light _moonLight;

    public Light sunLight
    {
        get
        {
            if (_sunLight == null)
            {
                _sunLight = FindWithTag<Light>(TagInfo.Tag_Sun);

            }
            return _sunLight;
        }
    }

    public Light moonLight
    {
        get
        {
            if (_moonLight == null)
            {
                _moonLight = FindWithTag<Light>(TagInfo.Tag_Moon);

            }
            return _moonLight;
        }
    }
}