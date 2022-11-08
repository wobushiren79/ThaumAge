using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewCharacterEquip : BaseUIView
{
    //装备类型
    public Dictionary<EquipTypeEnum, UIViewItemContainer> dicEquip = new Dictionary<EquipTypeEnum, UIViewItemContainer>();
    //属性数据
    public Dictionary<AttributeTypeEnum, UIViewItemCharacterStatus> dicAttribute = new Dictionary<AttributeTypeEnum, UIViewItemCharacterStatus>();

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

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshCharacterStatus();
        RefreshEquip();
    }

    /// <summary>
    /// 初始化装备
    /// </summary>
    public void InitEquip()
    {
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

        RefreshEquip();
    }

    /// <summary>
    /// 刷新装备
    /// </summary>
    public void RefreshEquip()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        CharacterEquipBean characterEquipData = userData.characterData.GetCharacterEquip();
        foreach (var itemContainer in dicEquip)
        {
            ItemsBean itemData = characterEquipData.GetEquipByType(itemContainer.Key);
            itemContainer.Value.SetLimitType(itemContainer.Key);
            itemContainer.Value.SetViewItemByData(UIViewItemContainer.ContainerType.Equip, itemData);
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
        dicAttribute.Clear();
        CreateCharacterStatusItem(AttributeTypeEnum.Health);
        CreateCharacterStatusItem(AttributeTypeEnum.Stamina);
        CreateCharacterStatusItem(AttributeTypeEnum.Magic);
        CreateCharacterStatusItem(AttributeTypeEnum.Saturation);
        CreateCharacterStatusItem(AttributeTypeEnum.Air);

        CreateCharacterStatusItem(AttributeTypeEnum.Damage);
        CreateCharacterStatusItem(AttributeTypeEnum.DamageMagic);

        CreateCharacterStatusItem(AttributeTypeEnum.Def);
        CreateCharacterStatusItem(AttributeTypeEnum.DefMagic);
        CreateCharacterStatusItem(AttributeTypeEnum.DefMetal);
        CreateCharacterStatusItem(AttributeTypeEnum.DefWood);
        CreateCharacterStatusItem(AttributeTypeEnum.DefWater);
        CreateCharacterStatusItem(AttributeTypeEnum.DefFire);
        CreateCharacterStatusItem(AttributeTypeEnum.DefEarth);
    }

    /// <summary>
    /// 刷新角色状态显示数据
    /// </summary>
    public void RefreshCharacterStatus()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        CharacterBean characterData = userData.characterData;

        foreach (var itemStatus in dicAttribute)
        {
            //获取数据
            string iconKey = AttributeBean.GetAttributeIconKey(itemStatus.Key);
            int statusData = characterData.GetAttributeValue(itemStatus.Key);
            string popupShowStr = AttributeBean.GetAttributeText(itemStatus.Key);

            itemStatus.Value.SetData(iconKey, $"{statusData}", popupShowStr);
        }
    }

    /// <summary>
    ///  创建角色状态Item
    /// </summary>
    public void CreateCharacterStatusItem(AttributeTypeEnum attributeType)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        CharacterBean characterData = userData.characterData;
        //获取数据
        string iconKey = AttributeBean.GetAttributeIconKey(attributeType);
        int statusData = characterData.GetAttributeValue(attributeType);
        string popupShowStr = AttributeBean.GetAttributeText(attributeType);
        //设置数据
        ui_ViewItemCharacterStatus.ShowObj(false);
        GameObject objItemStatus = Instantiate(ui_StatusContent.gameObject, ui_ViewItemCharacterStatus.gameObject);
        UIViewItemCharacterStatus itemStatus = objItemStatus.GetComponent<UIViewItemCharacterStatus>();
        itemStatus.SetData(iconKey, $"{statusData}",popupShowStr);

        dicAttribute.Add(attributeType, itemStatus);
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
        RefreshCharacterStatus();
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