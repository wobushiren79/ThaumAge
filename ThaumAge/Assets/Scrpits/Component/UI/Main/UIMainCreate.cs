using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class UIMainCreate : BaseUIComponent,
    SelectView.ICallBack,
    SelectColorView.ICallBack
{
    //数据序号
    public int userDataIndex;

    /// <summary>
    /// 头发数据
    /// </summary>
    protected List<CharacterInfoBean> listHairInfoData;

    /// <summary>
    /// 眼睛数据
    /// </summary>
    protected List<CharacterInfoBean> listEyeInfoData;

    /// <summary>
    /// 嘴巴数据
    /// </summary>
    protected List<CharacterInfoBean> listMouthInfoData;

    /// <summary>
    /// 衣服数据
    /// </summary>
    protected List<ItemsInfoBean> listClotehsInfoData;

    public override void Awake()
    {
        base.Awake();
        ui_RRotate.AddLongClickListener(OnLongClickForRoateR);
        ui_LRotate.AddLongClickListener(OnLongClickForRoateL);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ViewClose)
        {
            HandleForBack();
        }
        else if (viewButton == ui_Man)
        {
            ChangeSex(SexTypeEnum.Man);
        }
        else if (viewButton == ui_Woman)
        {
            ChangeSex(SexTypeEnum.Woman);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        //设置回调
        ui_ViewSelectChange_Hair.SetCallBack(this);
        ui_ViewSelectChange_Eye.SetCallBack(this);
        ui_ViewSelectChange_Mouth.SetCallBack(this);
        ui_ViewSelectChange_Clothes.SetCallBack(this);

        ui_ViewSelectColorChange_Hair.SetCallBack(this);
        ui_ViewSelectColorChange_Skin.SetCallBack(this);

        //设置头发
        List<long> listHairId = GameDataHandler.Instance.GetBaseInfoListLong(1);
        listHairInfoData = CreatureHandler.Instance.manager.GetCharacterInfoHair(listHairId);
        List<string> listHairData = CharacterInfoBean.GetNameList(listHairInfoData);
        ui_ViewSelectChange_Hair.SetListData(listHairData);

        //设置眼睛
        List<long> listEyeId = GameDataHandler.Instance.GetBaseInfoListLong(2);
        listEyeInfoData = CreatureHandler.Instance.manager.GetCharacterInfoEye(listEyeId);
        List<string> listEyeData = CharacterInfoBean.GetNameList(listEyeInfoData);
        ui_ViewSelectChange_Eye.SetListData(listEyeData);

        //设置嘴巴
        List<long> listMouthId = GameDataHandler.Instance.GetBaseInfoListLong(3);
        listMouthInfoData = CreatureHandler.Instance.manager.GetCharacterInfoMouth(listMouthId);
        List<string> listMouthData = CharacterInfoBean.GetNameList(listMouthInfoData);
        ui_ViewSelectChange_Mouth.SetListData(listMouthData);

        //设置衣服
        List<long> listClothesId = GameDataHandler.Instance.GetBaseInfoListLong(4);
        listClotehsInfoData = ItemsHandler.Instance.manager.GetItemsInfoById(listClothesId);
        List<string> listClothesData = ItemsInfoBean.GetNameList(listClotehsInfoData);
        ui_ViewSelectChange_Clothes.SetListData(listClothesData);

    }

    /// <summary>
    /// 设置用户数据序号
    /// </summary>
    /// <param name="userDataIndex"></param>
    public void SetUserDataIndex(int userDataIndex)
    {
        this.userDataIndex = userDataIndex;
        ui_ViewSelectChange_Hair.SetPosition(0);
        ui_ViewSelectChange_Eye.SetPosition(0);
        ui_ViewSelectChange_Mouth.SetPosition(0);
        ui_ViewSelectChange_Clothes.SetPosition(0);
        ui_ViewSelectColorChange_Skin.SetData(1, 1, 1);
        ui_ViewSelectColorChange_Hair.SetData(0, 0, 0);
        ChangeSex(SexTypeEnum.Man);
    }

    /// <summary>
    /// 处理-返回
    /// </summary>
    public void HandleForBack()
    {
        //打开用户存档界面
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainUserData>(UIEnum.MainUserData);
        //还原摄像头
        SceneMainHandler.Instance.ChangeCameraByIndex(0);
        //还原角色角度
        SceneMainHandler.Instance.CharacterResetRotate();
    }

    /// <summary>
    /// 处理-角色旋转
    /// </summary>
    /// <param name="direction"></param>
    public void HandleForCharacterRotate(DirectionEnum direction)
    {
        SceneMainHandler.Instance.RotateCharacter(userDataIndex, direction);
    }

    #region 左右长按旋转
    public void OnLongClickForRoateR()
    {
        HandleForCharacterRotate(DirectionEnum.Right);
    }

    public void OnLongClickForRoateL()
    {
        HandleForCharacterRotate(DirectionEnum.Left);
    }
    #endregion

    /// <summary>
    /// 选择回调
    /// </summary>
    /// <param name="selectView"></param>
    /// <param name="position"></param>
    public void ChangeSelectPosition(SelectView selectView, int position)
    {
        Character character = GetChanracter();
        if (character == null)
        {
            LogUtil.LogError("没有找到Character组件");
            return;
        }
        if (selectView == ui_ViewSelectChange_Hair)
        {
            CharacterInfoBean characterInfo = listHairInfoData[position];
            character.characterSkin.ChangeHair(characterInfo.id);
        }
        else if (selectView == ui_ViewSelectChange_Eye)
        {
            CharacterInfoBean characterInfo = listEyeInfoData[position];
            character.characterSkin.ChangeEye(characterInfo.id);
        }
        else if (selectView == ui_ViewSelectChange_Mouth)
        {
            CharacterInfoBean characterInfo = listMouthInfoData[position];
            character.characterSkin.ChangeMouth(characterInfo.id);
        }
        else if (selectView == ui_ViewSelectChange_Clothes)
        {
            ItemsInfoBean itemsInfo = listClotehsInfoData[position];
            character.characterEquip.ChangeClothes(itemsInfo.id);
        }
    }

    /// <summary>
    /// 颜色选择回调
    /// </summary>
    /// <param name="colorView"></param>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public void SelectColorChange(SelectColorView colorView, float r, float g, float b)
    {
        Character character = GetChanracter();
        if (character == null)
        {
            LogUtil.LogError("没有找到Character组件");
            return;
        }
        if (colorView == ui_ViewSelectColorChange_Hair)
        {
            character.characterSkin.ChangeHairColor(new Color(r, g, b, 1));
        }
        else if (colorView == ui_ViewSelectColorChange_Skin)
        {
            character.characterSkin.ChangeSkinColor(new Color(r, g, b, 1));
        }
    }

    /// <summary>
    /// 改变性别
    /// </summary>
    /// <param name="sexType"></param>
    public void ChangeSex(SexTypeEnum sexType)
    {
        Character character = GetChanracter();
        character.characterSkin.ChangeSex(sexType);

        ui_Man.interactable = true;
        ui_Woman.interactable = true;
        switch (sexType)
        {
            case SexTypeEnum.Man:
                ui_Man.interactable = false;
                break;
            case SexTypeEnum.Woman:
                ui_Woman.interactable = false;
                break;
        }
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <returns></returns>
    protected Character GetChanracter()
    {
        GameObject objCharacter = SceneMainHandler.Instance.manager.GetCharacterObjByIndex(userDataIndex);
        Character character = objCharacter.GetComponent<Character>();
        return character;
    }
}