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

    public override void Awake()
    {
        base.Awake();
        ui_RRotate.AddLongClickListener(OnLongClickForRoateR);
        ui_LRotate.AddLongClickListener(OnLongClickForRoateL);

        InitData();
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Back)
        {
            HandleForBack();
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

        listHairInfoData = CreatureHandler.Instance.manager.GetCharacterInfoEye(new List<long>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        List<string> listHairData = CharacterInfoBean.GetNameList(listHairInfoData);
        ui_ViewSelectChange_Hair.SetListData(listHairData);

        listEyeInfoData = CreatureHandler.Instance.manager.GetCharacterInfoEye(new List<long>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        List<string> listEyeData = CharacterInfoBean.GetNameList(listEyeInfoData);
        ui_ViewSelectChange_Eye.SetListData(listEyeData);

        listMouthInfoData = CreatureHandler.Instance.manager.GetCharacterInfoEye(new List<long>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        List<string> listMouthData = CharacterInfoBean.GetNameList(listMouthInfoData);
        ui_ViewSelectChange_Mouth.SetListData(listMouthData);

        List<string> listClothesData = new List<string>();
        ui_ViewSelectChange_Clothes.SetListData(listClothesData);
    }

    /// <summary>
    /// 设置用户数据序号
    /// </summary>
    /// <param name="userDataIndex"></param>
    public void SetUserDataIndex(int userDataIndex)
    {
        this.userDataIndex = userDataIndex;
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
        GameObject objCharacter = SceneMainHandler.Instance.manager.GetCharacterObjByIndex(userDataIndex);
        Character character = objCharacter.GetComponent<Character>();
        if (character == null)
        {
            LogUtil.LogError("没有找到Character组件");
            return;
        }
        if (selectView == ui_ViewSelectChange_Hair)
        {

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
        GameObject objCharacter = SceneMainHandler.Instance.manager.GetCharacterObjByIndex(userDataIndex);
        Character character = objCharacter.GetComponent<Character>();
        if (character == null)
        {
            LogUtil.LogError("没有找到Character组件");
            return;
        }
        if (colorView == ui_ViewSelectColorChange_Hair)
        {

        }
        else if (colorView == ui_ViewSelectColorChange_Skin)
        {

        }
    }
}