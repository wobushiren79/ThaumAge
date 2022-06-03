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
        //获取渲染摄像头
        GameObject objRenderModel = LoadResourcesUtil.SyncLoadData<GameObject>("UI/Render/RenderCharacterUI");
        //实例化渲染
        objRender = Instantiate(UIHandler.Instance.manager.GetUITypeContainer(UITypeEnum.Model3D).gameObject, objRenderModel);
        //获取展示的角色
        showCharacter = objRender.GetComponentInChildren<CreatureCptCharacter>();

        ui_RotateLeft.AddLongClickListener(OnLongClickForRoateR);
        ui_RotateRight.AddLongClickListener(OnLongClickForRoateL);

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
        dicEquip.Add(EquipTypeEnum.Shoes, ui_Equip_Shoes);
        dicEquip.Add(EquipTypeEnum.Headwear, ui_Equip_Headwear);
        dicEquip.Add(EquipTypeEnum.LeftRing, ui_Equip_LeftRing);
        dicEquip.Add(EquipTypeEnum.RightRing, ui_Equip_RightRing);
        dicEquip.Add(EquipTypeEnum.Cape, ui_Equip_Cape);

        foreach (var itemContainer in dicEquip)
        {
            ItemsBean itemData = userData.userEquip.GetEquipByType(itemContainer.Key);
            itemContainer.Value.SetLimitType(itemContainer.Key);
            itemContainer.Value.SetData(UIViewItemContainer.ContainerType.Equip, itemData);
            itemContainer.Value.SetHintText(UserEquipBean.GetEquipName(itemContainer.Key));
            itemContainer.Value.SetCallBackForSetViewItem(CallBackForSetEquip);
        }
    }

    /// <summary>
    /// 初始化角色状态
    /// </summary>
    public void InitCharacterStatus()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        CharacterStatusBean characterStatusData = userData.characterData.GetCharacterStatus();
        CreateCharacterStatusItem("ui_life_1", characterStatusData.health, "生命值");
        CreateCharacterStatusItem("ui_life_2", Mathf.RoundToInt(characterStatusData.stamina), "耐力值");
        CreateCharacterStatusItem("ui_life_4", characterStatusData.magic, "魔力值");
        CreateCharacterStatusItem("ui_life_3", Mathf.RoundToInt(characterStatusData.saturation), "饱食值");
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
                character.characterEquip.ChangeEquip(itemContainer.Key, changeItemData.itemId);

                //设置渲染摄像头
                Action<GameObject> callBack = (objModel) =>
                {
                    showCharacter.SetLayerAllChild(LayerInfo.RenderCamera);
                };

                //UI显示也修改
                showCharacter.characterEquip.ChangeEquip(itemContainer.Key, changeItemData.itemId, callBack);
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