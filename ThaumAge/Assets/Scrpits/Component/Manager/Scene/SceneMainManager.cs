using Cinemachine;
using UnityEditor;
using UnityEngine;

public class SceneMainManager : SceneBaseManager
{
    protected CinemachineVirtualCamera _cameraPositionStart;
    public CinemachineVirtualCamera cameraPositionStart
    {
        get
        {
            if (_cameraPositionStart == null)
            {
                _cameraPositionStart = GameObject.Find("CameraStartPosition")?.GetComponent<CinemachineVirtualCamera>();
            }
            return _cameraPositionStart;
        }
    }

    protected CinemachineVirtualCamera _cameraPosition_1;
    public CinemachineVirtualCamera cameraPosition_1
    {
        get
        {
            if (_cameraPosition_1 == null)
            {
                _cameraPosition_1 = GameObject.Find("CameraPosition_1")?.GetComponent<CinemachineVirtualCamera>();
            }
            return _cameraPosition_1;
        }
    }

    protected CinemachineVirtualCamera _cameraPosition_2;
    public CinemachineVirtualCamera cameraPosition_2
    {
        get
        {
            if (_cameraPosition_2 == null)
            {
                _cameraPosition_2 = GameObject.Find("CameraPosition_2")?.GetComponent<CinemachineVirtualCamera>();
            }
            return _cameraPosition_2;
        }
    }

    protected CinemachineVirtualCamera _cameraPosition_3;
    public CinemachineVirtualCamera cameraPosition_3
    {
        get
        {
            if (_cameraPosition_3 == null)
            {
                _cameraPosition_3 = GameObject.Find("CameraPosition_3")?.GetComponent<CinemachineVirtualCamera>();
            }
            return _cameraPosition_3;
        }
    }


    protected GameObject _characterObj_1;
    public GameObject characterObj_1
    {
        get
        {
            if (_characterObj_1 == null)
            {
                _characterObj_1 = GameObject.Find("CharacterBase_1");
            }
            return _characterObj_1;
        }
    }

    protected GameObject _characterObj_2;
    public GameObject characterObj_2
    {
        get
        {
            if (_characterObj_2 == null)
            {
                _characterObj_2 = GameObject.Find("CharacterBase_2");
            }
            return _characterObj_2;
        }
    }

    protected GameObject _characterObj_3;
    public GameObject characterObj_3
    {
        get
        {
            if (_characterObj_3 == null)
            {
                _characterObj_3 = GameObject.Find("CharacterBase_3");
            }
            return _characterObj_3;
        }
    }


    /// <summary>
    /// 通过索引 获取角色
    /// </summary>
    /// <param name="index"></param>
    public GameObject GetCharacterObjByIndex(int index)
    {
        switch (index)
        {
            case 1:
                return characterObj_1;
            case 2:
                return characterObj_2;
            case 3:
                return characterObj_3;
        }
        return null;
    }

    /// <summary>
    /// 通过索引 展示角色
    /// </summary>
    public GameObject ShowCharacterObjByIndex(int index, bool isShow)
    {
        GameObject objCharacter = GetCharacterObjByIndex(index);
        if (objCharacter == null)
            return null;
        objCharacter.SetActive(isShow);
        return objCharacter;
    }
}