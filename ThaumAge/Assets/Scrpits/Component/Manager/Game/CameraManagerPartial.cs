using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public partial class CameraManager
{

    //第一人称摄像头
    private CinemachineVirtualCamera _cameraForFirst;
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

    //第三人称摄像头
    private CinemachineFreeLook _cameraForThree;

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

    //第三人称摄像头初始数据
    public CinemachineFreeLook.Orbit[] threeFreeLookOriginalOrbits;

    //编辑建筑的摄像头
    private CinemachineFreeLook _cameraForBuildingEditor;

    public CinemachineFreeLook cameraForBuildingEditor
    {
        get
        {
            if (_cameraForBuildingEditor == null)
            {
                _cameraForBuildingEditor = FindWithTag<CinemachineFreeLook>(TagInfo.Tag_CameraCinemachine);
            }
            return _cameraForBuildingEditor;
        }
    }


    public GameObject _objEffectLiquid;
    public GameObject objEffectLiquid
    {
        get
        {
            if (_objEffectLiquid == null)
            {
                _objEffectLiquid = mainCamera.transform.Find("EffectUI_Liquid").gameObject;
            }
            return _objEffectLiquid;
        }
    }
}
