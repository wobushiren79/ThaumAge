using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewCharacterEquip : BaseUIView
{
    //装备类型
    public Dictionary<EquipTypeEnum, UIViewItemContainer> dicEquip = new Dictionary<EquipTypeEnum, UIViewItemContainer>();
    //渲染对象
    protected GameObject objRender;
    //展示的角色
    protected CreatureCptCharacter showCharacter;
    public override void Awake()
    {
        base.Awake();

        ui_RotateLeft.AddLongClickListener(OnLongClickForRoateR);
        ui_RotateRight.AddLongClickListener(OnLongClickForRoateL);

        InitRenderer();
        InitEquip();
        InitCharacterStatus();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Destroy(objRender);
        Resources.UnloadUnusedAssets();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (objRender != null)
            objRender.ShowObj(true);
        //设置角色数据
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        showCharacter.SetCharacterData(userData.characterData);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (objRender != null)
            objRender.ShowObj(false);
    }

    /// <summary>
    /// 初始化装备
    /// </summary>
    public void InitEquip()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        dicEquip.Clear();
        dicEquip.Add(EquipTypeEnum.Hats, ui_Equip_Hats);
        dicEquip.Add(EquipTypeEnum.Gloves, ui_Equip_Gloves);
        dicEquip.Add(EquipTypeEnum.Clothes, ui_Equip_Clothes);
        dicEquip.Add(EquipTypeEnum.Trousers, ui_Equip_Trousers);
        dicEquip.Add(EquipTypeEnum.Shoes, ui_Equip_Shoes);

        dicEquip.Add(EquipTypeEnum.Headwear, ui_Equip_Headwear);
        dicEquip.Add(EquipTypeEnum.LeftRing, ui_Equip_LeftRing);
        dicEquip.Add(EquipTypeEnum.RightRing, ui_Equip_RightRing);
        dicEquip.Add(EquipTypeEnum.Cape, ui_Equip_Cape);

        foreach (var itemContainer in dicEquip)
        {
            ItemsBean itemData = userData.characterData.characterEquip.GetEquipByType(itemContainer.Key);
            itemContainer.Value.SetLimitType(itemContainer.Key);
            itemContainer.Value.SetData(UIViewItemContainer.ContainerType.Equip, itemData);
            itemContainer.Value.SetHintText(CharacterEquipBean.GetEquipName(itemContainer.Key));
            itemContainer.Value.SetCallBackForSetViewItem(CallBackForSetEquip);
        }
    }

    /// <summary>
    /// 初始化渲染
    /// </summary>
    public void InitRenderer()
    {
        //获取渲染摄像头
        GameObject objRenderModel = LoadResourcesUtil.SyncLoadData<GameObject>("UI/Render/RenderCharacterUI");
        //实例化渲染
        objRender = Instantiate(UIHandler.Instance.manager.GetUITypeContainer(UITypeEnum.Model3D).gameObject, objRenderModel);
        //获取展示的角色
        showCharacter = objRender.GetComponentInChildren<CreatureCptCharacter>();
        //获取渲染摄像头
        Camera cameraRender = objRender.GetComponent<Camera>();
        //设置渲染
        ui_CharacterRT.texture = cameraRender.targetTexture;
    }

    /// <summary>
    /// 初始化角色状态
    /// </summary>
    public void InitCharacterStatus()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        CharacterStatusBean characterStatusData = userData.characterData.GetCharacterStatus();
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.Health), characterStatusData.maxHealth, AttributeBean.GetAttributeText(AttributeTypeEnum.Health));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.Stamina), Mathf.RoundToInt(characterStatusData.maxStamina), AttributeBean.GetAttributeText(AttributeTypeEnum.Stamina));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.Magic), characterStatusData.maxMagic, AttributeBean.GetAttributeText(AttributeTypeEnum.Magic));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.Saturation), Mathf.RoundToInt(characterStatusData.maxSaturation), AttributeBean.GetAttributeText(AttributeTypeEnum.Saturation));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.Air), Mathf.RoundToInt(characterStatusData.maxAir), AttributeBean.GetAttributeText(AttributeTypeEnum.Air));

        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.Def), characterStatusData.def, AttributeBean.GetAttributeText(AttributeTypeEnum.Def));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.DefMagic), characterStatusData.defMagic, AttributeBean.GetAttributeText(AttributeTypeEnum.DefMagic));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.DefMetal), characterStatusData.defMetal, AttributeBean.GetAttributeText(AttributeTypeEnum.DefMetal));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.DefWood), characterStatusData.defWooden, AttributeBean.GetAttributeText(AttributeTypeEnum.DefWood));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.DefWater), characterStatusData.defWater, AttributeBean.GetAttributeText(AttributeTypeEnum.DefWater));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.DefFire), characterStatusData.defFire, AttributeBean.GetAttributeText(AttributeTypeEnum.DefFire));
        CreateCharacterStatusItem(AttributeBean.GetAttributeIconKey(AttributeTypeEnum.DefEarth), characterStatusData.defWooden, AttributeBean.GetAttributeText(AttributeTypeEnum.DefEarth));
    }

    /// <summary>
    ///  创建角色状态Item
    /// </summary>
    public void CreateCharacterStatusItem(string iconKey, int statusData, string popupShowStr)
    {
        ui_ViewItemCharacterStatus.ShowObj(false);
        GameObject objItemStatus = Instantiate(ui_StatusContent.gameObject, ui_ViewItemCharacterStatus.gameObject);
        UIViewItemCharacterStatus itemStatus = objItemStatus.GetComponent<UIViewItemCharacterStatus>();
        itemStatus.SetData(iconKey, $"{statusData}",popupShowStr);
    }

    /// <summary>
    /// 设置装备回调
    /// </summary>
    /// <param name="changeContainer"></param>
    /// <param name="itemId"></param>
    public void CallBackForSetEquip(UIViewItemContainer changeContainer, ItemsBean changeItemData)
    {
        foreach (var itemContainer in dicEquip)
        {
            if (changeContainer == itemContainer.Value)
            {
                //更换装备
                Player player = GameHandler.Instance.manager.player;
                CreatureCptCharacter character = player.GetCharacter();
                character.characterEquip.ChangeEquip(itemContainer.Key, changeItemData);

                //设置渲染摄像头
                Action<GameObject> callBack = (objModel) =>
                {
                    showCharacter.SetLayerAllChild(LayerInfo.RenderCamera);
                };
                Action<IList<GameObject>> callBackModelRemark = (objModelRemark) =>
                {
                    showCharacter.SetLayerAllChild(LayerInfo.RenderCamera);
                };
                //UI显示也修改
                showCharacter.characterEquip.ChangeEquip(itemContainer.Key, changeItemData, callBack, callBackModelRemark);
            }
        }
    }

    /// <summary>
    /// 处理-角色旋转
    /// </summary>
    /// <param name="direction"></param>
    public void HandleForCharacterRotate(DirectionEnum direction)
    {
        if (direction == DirectionEnum.Left)
        {
            showCharacter.transform.localEulerAngles += new Vector3(0, -100 * Time.deltaTime, 0);
        }
        else
        {
            showCharacter.transform.localEulerAngles += new Vector3(0, 100 * Time.deltaTime, 0);
        }
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
}