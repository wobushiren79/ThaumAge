using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioManager : BaseManager
{
    public AudioBeanDictionary listMusicData = new AudioBeanDictionary();
    public AudioBeanDictionary listSoundData = new AudioBeanDictionary();
    public AudioBeanDictionary listEnvironmentData = new AudioBeanDictionary();

    /// <summary>
    /// 根据名字获取音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetMusicClip(string name, System.Action<AudioClip> completeAction)
    {
        LoadClipData(1, name, completeAction);
    }

    /// <summary>
    /// 根据名字获取音效
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetSoundClip(string name, System.Action<AudioClip> completeAction)
    {
        LoadClipData(2, name, completeAction);
    }

    /// <summary>
    /// 根据名字获取环境音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetEnvironmentClip(string name, System.Action<AudioClip> completeAction)
    {
        LoadClipData(3, name, completeAction);
    }

    protected void LoadClipData(int type, string name, System.Action<AudioClip> completeAction)
    {
        string dataPath = "audio/";
        AudioBeanDictionary dicData = new AudioBeanDictionary();
        AudioClip audioClip = null;
        switch (type)
        {
            case 1:
                dicData = listMusicData;
                dataPath += "music";
                break;
            case 2:
                dicData = listSoundData;
                dataPath += "sound";
                break;
            case 3:
                dicData = listEnvironmentData;
                dataPath += "environment";
                break;
        }
        if (dicData.TryGetValue(name, out audioClip))
        {
            completeAction?.Invoke(audioClip);
            return;
        }
        audioClip = LoadAssetUtil.SyncLoadAsset<AudioClip>(dataPath, name);
        if (audioClip != null)
        {
            dicData.Add(name, audioClip);
        }
        completeAction?.Invoke(audioClip);
    }

    public void LoadClipDataByAddressbles(int type, string name, System.Action<AudioClip> completeAction)
    {
        AudioBeanDictionary dicData = new AudioBeanDictionary();
        switch (type)
        {
            case 1:
                dicData = listMusicData;
                break;
            case 2:
                dicData = listSoundData;
                break;
            case 3:
                dicData = listEnvironmentData;
                break;
        }
        if (dicData.TryGetValue(name, out AudioClip audioClip))
        {
            completeAction?.Invoke(audioClip);
            return;
        }
        LoadAddressablesUtil.LoadAssetAsync<AudioClip>(name, (data) =>
        {
            if (data.Result != null)
            {
                dicData.Add(name, data.Result);
            }
            completeAction?.Invoke(data.Result);
        });

    }
}