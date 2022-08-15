using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioManager : BaseManager
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
            }
            return _audioSourceForEnvironment;
        }
    }


    public Dictionary<string, AudioClip> dicMusicData = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> dicSoundData = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> dicEnvironmentData = new Dictionary<string, AudioClip>();

    /// <summary>
    /// 根据名字获取音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetMusicClip(string name, System.Action<AudioClip> completeAction)
    {
        LoadClipDataByAddressbles(AuidoTypeEnum.Music, name, completeAction);
    }

    /// <summary>
    /// 根据名字获取音效
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetSoundClip(string name, System.Action<AudioClip> completeAction)
    {
        LoadClipDataByAddressbles(AuidoTypeEnum.Sound, name, completeAction);
    }

    /// <summary>
    /// 根据名字获取环境音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void GetEnvironmentClip(string name, System.Action<AudioClip> completeAction)
    {
        LoadClipDataByAddressbles(AuidoTypeEnum.Environment, name, completeAction);
    }

    /// <summary>
    /// 加载音频资源
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <param name="completeAction"></param>
    public void LoadClipDataByAddressbles(AuidoTypeEnum audioType, string name, System.Action<AudioClip> completeAction)
    {
        Dictionary<string, AudioClip> dicAudioData;
        switch (audioType)
        {
            case AuidoTypeEnum.Music:
                dicAudioData = dicMusicData;
                break;
            case AuidoTypeEnum.Sound:
                dicAudioData = dicSoundData;
                break;
            case AuidoTypeEnum.Environment:
                dicAudioData = dicEnvironmentData;
                break;
            default:
                return;
        }
        if (dicAudioData.TryGetValue(name, out AudioClip audioClip))
        {
            completeAction?.Invoke(audioClip);
            return;
        }
        LoadAddressablesUtil.LoadAssetAsync<AudioClip>(name, (data) =>
        {
            if (data.Result != null)
            {
                dicAudioData.Add(name, data.Result);
                completeAction?.Invoke(data.Result);
                return;
            }
            completeAction?.Invoke(null);
        });
    }
}