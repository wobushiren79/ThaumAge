using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CharacterSkin : CharacterBase
{
    //头部渲染
    public MeshRenderer headRenderer;
    public Material headMat;

    //身体渲染
    public SkinnedMeshRenderer bodyRenderer;
    public Material bodyMat;

    //头发容器
    public GameObject objHairContainer;
    public Material hairMat;


    public CharacterSkin(CreatureCptCharacter character) : base(character)
    {
        if (character.characterHead == null)
        {
            LogUtil.LogError($"初始化角色失败，{character.gameObject.name}的角色 缺少 Head 部件");
            return;
        }
        if (character.characterBody == null)
        {
            LogUtil.LogError($"初始化角色失败，{character.gameObject.name}的角色 缺少 Body 部件");
            return;
        }
        if (character.characterHair == null)
        {
            LogUtil.LogError($"初始化角色失败，{character.gameObject.name}的角色 缺少 Hair 部件");
            return;
        }
        characterData = character.GetCharacterData();

        headRenderer = character.characterHead.GetComponent<MeshRenderer>();
        bodyRenderer = character.characterBody.GetComponentInChildren<SkinnedMeshRenderer>();
        objHairContainer = character.characterHair;

        if (this.headRenderer != null)
        {
            headMat = headRenderer.material;
        }
        if (this.bodyRenderer != null)
        {
            bodyMat = bodyRenderer.material;
        }
    }

    /// <summary>
    /// 设置角色数据
    /// </summary>
    public override void SetCharacterData(CharacterBean characterData)
    {
        base.SetCharacterData(characterData);

        ChangeSex(this.characterData.GetSex());

        ChangeSkinColor(this.characterData.GetColorSkin());
        ChangeHairColor(this.characterData.GetColorHair());

        ChangeHair(this.characterData.hairId);
        ChangeEye(this.characterData.eyeId);
        ChangeMouth(this.characterData.mouthId);
    }


    /// <summary>
    /// 修改头发颜色
    /// </summary>
    /// <param name="color"></param>
    public void ChangeHairColor(Color color)
    {
        this.characterData.SetColorHair(color);
        if (hairMat == null)
            return;
        hairMat.color = color;
    }

    /// <summary>
    /// 改变发型
    /// </summary>
    /// <param name="hairId"></param>
    public void ChangeHair(long hairId)
    {
        this.characterData.hairId = hairId;
        CptUtil.RemoveChild(objHairContainer.transform);
        if (hairId == 0)
        {
            //没有头发
            return;
        }
        CharacterInfoBean characterInfo = CreatureHandler.Instance.manager.GetCharacterInfoHair(hairId);
        if (characterInfo == null)
        {
            LogUtil.LogError($"查询发型数据失败，没有ID为 {hairId} 的发型数据");
        }
        else
        {
            CreatureHandler.Instance.manager.GetCharacterHairModel(characterInfo.model_name,
                (data) =>
                {
                    if (data == null)
                    {
                        LogUtil.LogError($"查询发型失败，没有名字为 {characterInfo.model_name} 的发型模型");
                    }
                    else
                    {
                        GameObject objHair = CreatureHandler.Instance.Instantiate(objHairContainer, data);
                        objHair.transform.localPosition = Vector3.zero;
                        //objHair.transform.localEulerAngles = Vector3.zero;
                        MeshRenderer hairMeshRebderer = objHair.GetComponentInChildren<MeshRenderer>();
                        hairMat = hairMeshRebderer.sharedMaterial;
                    }
                });
        }
    }

    /// <summary>
    /// 改变性别
    /// </summary>
    /// <param name="sexType"></param>
    public void ChangeSex(SexTypeEnum sexType)
    {
        this.characterData.SetSex(sexType);
        long skinId = 0;
        switch (sexType)
        {
            case SexTypeEnum.Man:
                skinId = 2;
                break;
            case SexTypeEnum.Woman:
                skinId = 1;
                break;
        }

        CharacterInfoBean characterInfo = CreatureHandler.Instance.manager.GetCharacterInfoSkin(skinId);
        if (characterInfo == null)
        {
            LogUtil.LogError($"查询皮肤数据失败，没有ID为 {skinId} 的皮肤数据");
        }
        else
        {
            CreatureHandler.Instance.manager.GetCharacterSkinTex(characterInfo.model_name,
                (data) =>
                {
                    if (data == null)
                    {
                        LogUtil.LogError($"查询皮肤贴图失败，没有名字为 {characterInfo.model_name} 的皮肤贴图");
                    }
                    else
                    {
                        bodyMat.mainTexture = data;
                    }
                });
        }
    }

    /// <summary>
    /// 改变皮肤颜色
    /// </summary>
    /// <param name="color"></param>
    public void ChangeSkinColor(Color color)
    {
        this.characterData.SetColorSkin(color);
        headMat.SetColor("Head_Color", color);
        bodyMat.color = color;
    }

    /// <summary>
    /// 修改眼睛
    /// </summary>
    /// <param name="eyeId"></param>
    public void ChangeEye(long eyeId)
    {
        this.characterData.eyeId = eyeId;
        CharacterInfoBean characterInfo = CreatureHandler.Instance.manager.GetCharacterInfoEye(eyeId);
        if (characterInfo == null)
        {
            LogUtil.LogError($"查询眼睛数据失败，没有ID为 {eyeId} 的眼睛数据");
        }
        else
        {
            CreatureHandler.Instance.manager.GetCharacterEyeTex(characterInfo.model_name,
                (data) =>
                {
                    if (data == null)
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
        this.characterData.mouthId = mouthId;
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