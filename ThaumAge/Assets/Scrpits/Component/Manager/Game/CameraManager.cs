using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : BaseManager
{
    //主摄像头
    protected Camera _mainCamera;
    //ui摄像头
    protected Camera _uiCamera;

    //第一人称摄像头
    private CinemachineVirtualCamera _cameraForFirst;
    //第三人称摄像头
    private CinemachineFreeLook _cameraForThree;

    //第三人称摄像头初始数据
    public CinemachineFreeLook.Orbit[] threeFreeLookOriginalOrbits;

    public CinemachineVirtualCamera cameraForFirst
    {
        get
        {
            if (_cameraForFirst == null)
            {
                _cameraForFirst = FindWithTag<CinemachineVirtualCamera>(TagInfo.Tag_CameraCinemachine);
            }
            return _cameraForFirst;
        }
    }

    public CinemachineFreeLook cameraForThree
    {
        get
        {
            if (_cameraForThree == null)
            {
                _cameraForThree = FindWithTag<CinemachineFreeLook>(TagInfo.Tag_CameraCinemachine);
                //获取第三人称相机得间距初始数据
                if (_cameraForThree != null)
                {
                    threeFreeLookOriginalOrbits = new CinemachineFreeLook.Orbit[_cameraForThree.m_Orbits.Length];
                    for (int i = 0; i < _cameraForThree.m_Orbits.Length; i++)
                    {
                        threeFreeLookOriginalOrbits[i].m_Height = _cameraForThree.m_Orbits[i].m_Height;
                        threeFreeLookOriginalOrbits[i].m_Radius = _cameraForThree.m_Orbits[i].m_Radius;
                    }
                }        
            }
            return _cameraForThree;
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

    public Camera uiCamera
    {
        get
        {
            if (_uiCamera == null)
            {
                _uiCamera = FindWithTag<Camera>(TagInfo.Tag_UICamera);
            }
            return _uiCamera;
        }
    }
}
