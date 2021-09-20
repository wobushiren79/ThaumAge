using UnityEditor;
using UnityEngine;

public class UIChildGameSettingAudioContent : UIChildGameSettingBaseContent
{
    public UIChildGameSettingAudioContent(GameObject objListContainer) : base(objListContainer)
    {

    }

    public override void Open()
    {
        base.Open();
        //音乐
        CreateItemForRange("音乐", HandleForMusicChange);
        //音效
        CreateItemForRange("音效", HandleForSoundChange);
    }

    /// <summary>
    /// 音乐控制
    /// </summary>
    /// <param name="data"></param>
    public void HandleForMusicChange(float data)
    {

    }

    /// <summary>
    /// 音效控制
    /// </summary>
    /// <param name="data"></param>
    public void HandleForSoundChange(float data)
    {

    }
}