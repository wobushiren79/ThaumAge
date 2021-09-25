using UnityEditor;
using UnityEngine;

public class UIChildGameSettingAudioContent : UIChildGameSettingBaseContent
{
    //音乐
    protected UIListItemGameSettingRange settingMusic;
    //音效
    protected UIListItemGameSettingRange settingSound;

    public UIChildGameSettingAudioContent(GameObject objListContainer) : base(objListContainer)
    {

    }

    public override void Open()
    {
        base.Open();
        //音乐
        settingMusic = CreateItemForRange(TextHandler.Instance.GetTextById(106), HandleForMusicChange);
        settingMusic.SetPro(gameConfig.musicVolume);
        //音效
        settingSound = CreateItemForRange(TextHandler.Instance.GetTextById(107), HandleForSoundChange);
        settingSound.SetPro(gameConfig.soundVolume);
    }

    /// <summary>
    /// 音乐控制
    /// </summary>
    /// <param name="data"></param>
    public void HandleForMusicChange(float data)
    {
        gameConfig.musicVolume = data;
    }

    /// <summary>
    /// 音效控制
    /// </summary>
    /// <param name="data"></param>
    public void HandleForSoundChange(float data)
    {
        gameConfig.soundVolume = data;
    }
}