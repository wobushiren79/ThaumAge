using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class GameConfigBean 
{
    //屏幕模式 0窗口  1全屏
    public int window = 0;
    //语言
    public string language = "cn" ;
    //音效大小
    public float soundVolume = 0.5f;
    //音乐大小
    public float musicVolume = 0.5f;

    //环境音乐大小
    public float environmentVolume = 0.5f;
    //自动保存时间
    public float autoSaveTime = 30;
    //UI大小
    public float uiSize = 1f;

    //帧数限制开启 1开启 0关闭
    public int stateForFrames = 1;
    public int frames = 120;
    //阴影距离
    public float shadowDis = 200;
    /// <summary>
    /// 获取当前语言
    /// </summary>
    /// <returns></returns>
    public LanguageEnum GetLanguage()
    {
       return  EnumUtil.GetEnum<LanguageEnum>(language);
    }

    /// <summary>
    /// 设置语言
    /// </summary>
    /// <param name="language"></param>
    public void SetLanguage(LanguageEnum language)
    {
        this.language = EnumUtil.GetEnumName(language);
    }
}