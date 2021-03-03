using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class AudioHandler : BaseHandler<AudioHandler, AudioManager>
{
    protected AudioListener _audioListener;
    protected AudioSource _audioSourceForMusic;
    protected AudioSource _audioSourceForSound;
    protected AudioSource _audioSourceForEnvironment;

    protected int sourceNumber = 0;
    protected int sourceMaxNumber = 5;

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

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitAudio()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        audioSourceForMusic.volume = gameConfig.musicVolume;
        audioSourceForSound.volume = gameConfig.soundVolume;
        audioSourceForEnvironment.volume = gameConfig.environmentVolume;
    }

    /// <summary>
    ///  循环播放音乐
    /// </summary>
    /// <param name="audioMusic"></param>
    public void PlayMusicForLoop(AudioMusicEnum audioMusic)
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        PlayMusicForLoop(audioMusic, gameConfig.musicVolume);
    }

    /// <summary>
    /// 循环播放音乐
    /// </summary>
    /// <param name="audioMusic"></param>
    /// <param name="volumeScale"></param>
    public void PlayMusicForLoop(AudioMusicEnum audioMusic, float volumeScale)
    {
        AudioClip audioClip = null;
        //switch (audioMusic)
        //{
            //case AudioMusicEnum.LangTaoSha:
            //    audioClip = audioManager.GetMusicClip("music_langtaosha_1");
            //    break;
            //case AudioMusicEnum.YangChunBaiXue:
            //    audioClip = audioManager.GetMusicClip("music_yangchunbaixue_1");
            //    break;
            //case AudioMusicEnum.Main:
            //    audioClip = audioManager.GetMusicClip("music_1");
            //    break;
            //case AudioMusicEnum.Game:
            //    List<AudioClip> listGameClip = new List<AudioClip>()
            //    {
            //        audioManager.GetMusicClip("music_1"),
            //        audioManager.GetMusicClip("music_2"),
            //        audioManager.GetMusicClip("music_3"),
            //        audioManager.GetMusicClip("music_6"),
            //        audioManager.GetMusicClip("music_7")
            //    };
            //    audioClip = RandomUtil.GetRandomDataByList(listGameClip);
            //    break;
            //case AudioMusicEnum.Battle:
            //    List<AudioClip> listBattleClip = new List<AudioClip>()
            //    {
            //        audioManager.GetMusicClip("music_4"),
            //        audioManager.GetMusicClip("music_8")
            //    };
            //    audioClip = RandomUtil.GetRandomDataByList(listBattleClip);
            //    break;
         // }
        if (audioClip != null)
        {
            audioSourceForMusic.clip = audioClip;
            audioSourceForMusic.volume = volumeScale;
            audioSourceForMusic.loop = true;
            audioSourceForMusic.Play();
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sound">音效</param>
    /// <param name="volumeScale">音量大小</param>
    public void PlaySound(AudioSoundEnum sound, Vector3 soundPosition, float volumeScale)
    {
        if (sourceNumber > sourceMaxNumber)
            return;
        AudioClip audioClip = null;
        AudioSource audioSource = audioSourceForSound;
        switch (sound)
        {
            case AudioSoundEnum.ButtonForNormal:
                audioClip = manager.GetSoundClip("sound_btn_3");
                break;
            case AudioSoundEnum.ButtonForBack:
                audioClip = manager.GetSoundClip("sound_btn_2");
                break;
            case AudioSoundEnum.ButtonForHighLight:
                audioClip = manager.GetSoundClip("sound_btn_1");
                break;
            case AudioSoundEnum.ButtonForShow:
                audioClip = manager.GetSoundClip("sound_btn_6");
                break;
        }
        if (audioClip != null)
        {

            StartCoroutine(CoroutineForPlayOneShot(audioSource, audioClip, volumeScale));
            //audioSource.PlayOneShot(audioClip, volumeScale);
        }
        // AudioSource.PlayClipAtPoint(soundClip, soundPosition,volumeScale);
    }

    /// <summary>
    /// 协程播放音效
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="audioClip"></param>
    /// <param name="volumeScale"></param>
    /// <returns></returns>
    IEnumerator CoroutineForPlayOneShot(AudioSource audioSource, AudioClip audioClip, float volumeScale)
    {
        sourceNumber++;
        audioSource.PlayOneShot(audioClip, volumeScale);
        yield return new WaitForSeconds(audioClip.length);
        sourceNumber--;
    }

    public void PlaySound(AudioSoundEnum sound)
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        PlaySound(sound, Camera.main.transform.position, gameConfig.soundVolume);
    }
    public void PlaySound(AudioSoundEnum sound, float volumeScale)
    {
        PlaySound(sound, Camera.main.transform.position, volumeScale);
    }
    public void PlaySound(AudioSoundEnum sound, Vector3 soundPosition)
    {
        PlaySound(sound, soundPosition, 1);
    }

    /// <summary>
    /// 播放环境音乐
    /// </summary>
    /// <param name="audioEnvironment"></param>
    public void PlayEnvironment(AudioEnvironmentEnum audioEnvironment, float volumeScale)
    {
        AudioClip audioClip = null;
        //switch (audioEnvironment)
        //{

        //}
        audioSourceForEnvironment.volume = volumeScale;
        audioSourceForEnvironment.clip = audioClip;
        audioSourceForEnvironment.Play();
    }
    /// <summary>
    /// 停止播放
    /// </summary>
    public void StopEnvironment()
    {
        audioSourceForEnvironment.clip = null;
        audioSourceForEnvironment.Stop();
    }

    public void StopMusic()
    {
        audioSourceForMusic.clip = null;
        audioSourceForMusic.Stop();
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void PauseEnvironment()
    {
        audioSourceForEnvironment.Pause();
    }


    public void PauseMusic()
    {
        audioSourceForMusic.Pause();
    }


    /// <summary>
    /// 恢复
    /// </summary>
    public void RestoreEnvironment()
    {
        audioSourceForEnvironment.Play();
    }

    public void RestoreMusic()
    {
        audioSourceForMusic.Play();
    }

}