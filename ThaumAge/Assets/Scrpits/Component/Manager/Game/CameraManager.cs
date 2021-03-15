using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BaseManager
{
    public Camera _mainCamera;

    public Camera mainCamera
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
            return _mainCamera;
        }
    }

}
