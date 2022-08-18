using System;
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

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        SetUIText();
    }

    /// <summary>
    /// 设置文本信息
    /// </summary>
    public void SetUIText()
    {
        ui_StartText.text = TextHandler.Instance.GetTextById(31);
        ui_NameTitle.text = TextHandler.Instance.GetTextById(32);
        ui_TitleHair.text = TextHandler.Instance.GetTextById(35);
        ui_TitleHairColor.text = TextHandler.Instance.GetTextById(36);
        ui_TitleEye.text = TextHandler.Instance.GetTextById(37);
        ui_TitleSkin.text = TextHandler.Instance.GetTextById(38);
        ui_TitleMouth.text = TextHandler.Instance.GetTextById(39);
        ui_TitleClothes.text = TextHandler.Instance.GetTextById(40);
        ui_NamePlaceholder.text = TextHandler.Instance.GetTextById(41);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ViewClose)
        {
            HandleForBack();
            //播放音效
            AudioHandler.Instance.PlaySound(2);
        }
        else if (viewButton == ui_Man)
        {
            ChangeSex(SexTypeEnum.Man);
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
        else if (viewButton == ui_Woman)
        {
            ChangeSex(SexTypeEnum.Woman);
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
        else if (viewButton == ui_RandomCharacter)
        {
            HandleForRandomCharacter();
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
        else if (viewButton == ui_Start)
        {
            HandleForStartGame();
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
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
    /// 处理-随机角色
    /// </summary>
    public void HandleForRandomCharacter()
    {
        UnityEngine.Random.InitState(TimeUtil.GetTimeStampForS32());
        ui_ViewSelectChange_Hair.RandomSelect();
        ui_ViewSelectChange_Eye.RandomSelect();
        ui_ViewSelectChange_Mouth.RandomSelect();
        ui_ViewSelectChange_Clothes.RandomSelect();
        ui_ViewSelectColorChange_Skin.SetRandomColor();
        ui_ViewSelectColorChange_Hair.SetRandomColor();
        //随机性别
        int sexRandom = WorldRandTools.Range(0, 2);
        ChangeSex(sexRandom == 0 ? SexTypeEnum.Man : SexTypeEnum.Woman);
    }

    /// <summary>
    /// 处理-开始游戏
    /// </summary>
    public void HandleForStartGame()
    {
        CreatureCptCharacter character = GetCharacter();
        string characterName = ui_NameInput.text;
        string userId = $"UserId_{SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N)}";
        if (characterName.IsNull())
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30001));
            return;
        }
        Action<DialogView, DialogBean> actionSubmit = (view, data) =>
        {
            UserDataBean userData = new UserDataBean();
            userData.dataIndex = userDataIndex;
            userData.userId = userId;
            userData.characterData = character.GetCharacterData();
            userData.characterData.characterName = characterName;
            userData.seed = UnityEngine.Random.Range(0, int.MaxValue);
            //设置位置
            userData.userPosition.SetPosition(Vector3.zero);
            userData.userPosition.SetWorldType(WorldTypeEnum.Main);
            //设置时间
            userData.timeForGame.hour = 6;

            //保存数据
            GameDataHandler.Instance.manager.SaveUserData(userData);
            //使用数据
            GameDataHandler.Instance.manager.UseUserData(userData);
            //改变场景
            SceneMainHandler.Instance.ChangeScene(ScenesEnum.GameScene);
        };
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.GetTextById(20002);
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.actionSubmit = actionSubmit;

        UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
    }

    /// <summary>
    /// 处理-返回
    /// </summary>
    public void HandleForBack()
    {
        //打开用户存档界面
        UIHandler.Instance.OpenUIAndCloseOther<UIMainUserData>(UIEnum.MainUserData);
        //还原摄像头
        SceneMainHandler.Instance.ChangeCameraByIndex(0);
        //还原角色角度
        SceneMainHandler.Instance.CharacterResetRotate();
        //隐藏角色
        SceneMainHandler.Instance.manager.ShowCharacterObjByIndex(userDataIndex, false);
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
        CreatureCptCharacter character = GetCharacter();
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
            character.characterEquip.ChangeEquip(EquipTypeEnum.Clothes, itemsInfo.id);
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
        CreatureCptCharacter character = GetCharacter();
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
        CreatureCptCharacter character = GetCharacter();
        character.characterSkin.ChangeSex(sexType);

        ui_Man.interactable = true;
        ui_Woman.interactable = true;
        switch (sexType)
        {
            case SexTypeEnum.Man:
                ui_Man.interactable = false;
                ui_SexTitle.text = TextHandler.Instance.GetTextById(33);
                break;
            case SexTypeEnum.Woman:
                ui_Woman.interactable = false;
                ui_SexTitle.text = TextHandler.Instance.GetTextById(34);
                break;
        }
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <returns></returns>
    protected CreatureCptCharacter GetCharacter()
    {
        GameObject objCharacter = SceneMainHandler.Instance.manager.GetCharacterObjByIndex(userDataIndex);
        CreatureCptCharacter character = objCharacter.GetComponent<CreatureCptCharacter>();
        return character;
    }
}