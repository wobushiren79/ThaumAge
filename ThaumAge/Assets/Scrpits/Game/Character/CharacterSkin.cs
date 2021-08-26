using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CharacterSkin
{
    //头部渲染
    public MeshRenderer headRenderer;
    public Material headMat;

    //身体渲染
    public SkinnedMeshRenderer bodyRenderer;
    public Material bodyMat;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="headRenderer"></param>
    /// <param name="bodyRenderer"></param>
    public CharacterSkin(MeshRenderer headRenderer, SkinnedMeshRenderer bodyRenderer)
    {
        this.headRenderer = headRenderer;
        this.bodyRenderer = bodyRenderer;

        if (this.headRenderer != null)
        {
            headMat = headRenderer.sharedMaterial;
        }
        if (this.bodyRenderer != null)
        {
            bodyMat = bodyRenderer.sharedMaterial;
        }
    }

    /// <summary>
    /// 修改眼睛
    /// </summary>
    /// <param name="eyeId"></param>
    public void ChangeEye(long eyeId)
    {
        CharacterInfoBean characterInfo =  CreatureHandler.Instance.manager.GetCharacterInfoEye(eyeId);
        if (characterInfo == null)
        {
            LogUtil.LogError($"查询眼睛数据失败，没有ID为 {eyeId} 的眼睛数据");
        }
        else
        {
            CreatureHandler.Instance.manager.GetCharacterEyeTex(characterInfo.model_name, 
                (data) => 
                {
                    if (data==null)
                    {
                        LogUtil.LogError($"查询眼睛贴图失败，没有名字为 {characterInfo.model_name} 的眼睛贴图");
                    }
                    else
                    {
                        headMat.SetTexture("Tex_Eye", data);
                    }
                });
        }
    }

    /// <summary>
    /// 修改嘴巴
    /// </summary>
    /// <param name="mouthId"></param>
    public void ChangeMouth(long mouthId)
    {
        CharacterInfoBean characterInfo = CreatureHandler.Instance.manager.GetCharacterInfoMouth(mouthId);
        if (characterInfo == null)
        {
            LogUtil.LogError($"查询嘴巴数据失败，没有ID为 {mouthId} 的嘴巴数据");
        }
        else
        {
            CreatureHandler.Instance.manager.GetCharacterMouthTex(characterInfo.model_name,
                (data) =>
                {
                    if (data == null)
                    {
                        LogUtil.LogError($"查询嘴巴贴图失败，没有名字为 {characterInfo.model_name} 的嘴巴贴图");
                    }
                    else
                    {
                        headMat.SetTexture("Tex_Mouth", data);
                    }
                });
        }
    }
}