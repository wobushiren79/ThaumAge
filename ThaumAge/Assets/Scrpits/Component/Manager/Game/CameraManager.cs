﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : BaseManager
{
    protected Camera _mainCamera;

    protected Transform _cameraFreeLookContainer;
    protected CinemachineVirtualCameraBase[] _listCameraFreeLook;

    public float speedForCameraMove = 300;

    public CinemachineVirtualCameraBase[] listCameraFreeLook
    {
        get
        {
            if (_cameraFreeLookContainer == null)
            {
                _cameraFreeLookContainer = FindWithTag<Transform>(TagInfo.Tag_CameraFreeLook);
                if (_cameraFreeLookContainer != null)
                {
                    _listCameraFreeLook = _cameraFreeLookContainer.GetComponentsInChildren<CinemachineVirtualCameraBase>();
                }
            }
            return _listCameraFreeLook;
        }
    }

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
