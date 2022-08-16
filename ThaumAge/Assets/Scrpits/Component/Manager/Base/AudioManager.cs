﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AudioManager : BaseManager, IAudioInfoView
{
    protected AudioListener _audioListener;
    protected AudioSource _audioSourceForMusic;
    protected AudioSource _audioSourceForSound;
    protected AudioSource _audioSourceForEnvironment;
    public AudioListener audioSourceForListener
    {
        get
        {
            if (_audioListener == null)
            {
                _audioListener = FindWithTag<AudioListener>(TagInfo.Tag_AudioListener);
                if (_audioListener == null)
                {
                    Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
                    GameObject obj = new GameObject("AudioListener");

                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.parent = mainCamera.transform;
                    obj.tag = TagInfo.Tag_AudioListener;

                    _audioListener = obj.AddComponentEX<AudioListener>();
                }
            }
            return _audioListener;
        }
    }

    public AudioSource audioSourceForMusic
    {
        get
        {
            if (_audioSourceForMusic == null)
            {
                _audioSourceForMusic = FindWithTag<AudioSource>(TagInfo.Tag_AudioMusic);
                if (_audioListener == null)
                {
                    Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
                    GameObject obj = new GameObject("AudioMusic");

                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.parent = mainCamera.transform;
                    obj.tag = TagInfo.Tag_AudioMusic;

                    _audioSourceForMusic = obj.AddComponentEX<AudioSource>();
                }
            }
            return _audioSourceForMusic;
        }
    }

    public AudioSource audioSourceForSound
    {
        get
        {
            if (_audioSourceForSound == null)
            {
                _audioSourceForSound = FindWithTag<AudioSource>(TagInfo.Tag_AudioSound);
                if (_audioListener == null)
                {
                    Camera mainCamera = CameraHandler.Instance.manager.mainCamera;

                    GameObject obj = new GameObject("AudioSound");
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.parent = mainCamera.transform;
                    obj.tag = TagInfo.Tag_AudioSound;

                    _audioSourceForSound = obj.AddComponentEX<AudioSource>();
                }
            }
            return _audioSourceForSound;
        }
    }

    public AudioSource audioSourceForEnvironment
    {
        get
        {
            if (_audioSourceForEnvironment == null)
            {
                _audioSourceForEnvironment = FindWithTag<AudioSource>(TagInfo.Tag_AudioEnvironment);
                if (_audioListener == null)
                {
                    Camera mainCamera = CameraHandler.Instance.manager.mainCamera;

                    GameObject obj = new GameObject("AudioEnvironment");
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.parent = mainCamera.transform;
                    obj.tag = TagInfo.Tag_AudioEnvironment;

                    _audioSourceForEnvironment = obj.AddComponentEX<AudioSource>();
                }
            }
            return _audioSourceForEnvironment;
        }
    }

    protected Dictionary<string, AudioClip> dicMusicData = new Dictionary<string, AudioClip>();
    protected Dictionary<string, AudioClip> dicSoundData = new Dictionary<string, AudioClip>();
    protected Dictionary<string, AudioClip> dicEnvironmentData = new Dictionary<string, AudioClip>();

    protected static string PathMusic = "Assets/Prefabs/Audio/Music";
    protected static string PathSound = "Assets/Prefabs/Audio/Sound";
    protected static string PathEnvironment = "Assets/Prefabs/Audio/Environment";

    protected Dictionary<int, AudioInfoBean> dicAudioInfoData = new Dictionary<int, AudioInfoBean>();
    protected AudioInfoController controllerForAudioInfo;

    public virtual void Awake()
    {
        controllerForAudioInfo = new AudioInfoController(this, this);
        controllerForAudioInfo.GetAllAudioInfoData(InitAudioInfoData);
    }

    /// <summary>
    /// 初始化音频数据
    /// </summary>
    /// <param name="listData"></param>
    public void InitAudioInfoData(List<AudioInfoBean> listData)
    {
        dicAudioInfoData.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            var itemData = listData[i];
            dicAudioInfoData.Add(itemData.id, itemData);
        }
    }

    /// <summary>
    /// 获取音频数据
    /// </summary>
    /// <param name="id"></param>
    public AudioInfoBean GetAudioInfo(int id)
    {
        if (dicAudioInfoData.TryGetValue(id,out AudioInfoBean data))
        {
            return data;
        }
        return null;
    }

    /// <summary>
    /// 根据名字获取音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetMusicClip(string name, Action<AudioClip> completeAction)
    {
        LoadClipDataByAddressbles(AuidoTypeEnum.Music, name, completeAction);
    }

    /// <summary>
    /// 根据名字获取音效
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetSoundClip(string name, Action<AudioClip> completeAction)
    {
        LoadClipDataByAddressbles(AuidoTypeEnum.Sound, name, completeAction);
    }

    /// <summary>
    /// 根据名字获取环境音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetEnvironmentClip(string name, Action<AudioClip> completeAction)
    {
        LoadClipDataByAddressbles(AuidoTypeEnum.Environment, name, completeAction);
    }

    /// <summary>
    /// 加载音频资源
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void LoadClipDataByAddressbles(AuidoTypeEnum audioType, string name, Action<AudioClip> completeAction)
    {
        Dictionary<string, AudioClip> dicAudioData;
        string pathData;
        switch (audioType)
        {
            case AuidoTypeEnum.Music:
                pathData = PathMusic;
                dicAudioData = dicMusicData;
                break;
            case AuidoTypeEnum.Sound:
                pathData = PathSound;
                dicAudioData = dicSoundData;
                break;
            case AuidoTypeEnum.Environment:
                pathData = PathEnvironment;
                dicAudioData = dicEnvironmentData;
                break;
            default:
                return;
        }
        string allPathData = $"{pathData}/{name}";
        if (dicAudioData.TryGetValue(allPathData, out AudioClip audioClip))
        {
            completeAction?.Invoke(audioClip);
            return;
        }
        LoadAddressablesUtil.LoadAssetAsync<AudioClip>(allPathData, (data) =>
        {
            if (data.Result != null)
            {
                dicAudioData.Add(allPathData, data.Result);
                completeAction?.Invoke(data.Result);
                return;
            }
            completeAction?.Invoke(null);
        });
    }

    #region 获取数据回调
    public void GetAudioInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetAudioInfoFail(string failMsg, Action action)
    {
    }
    #endregion
}