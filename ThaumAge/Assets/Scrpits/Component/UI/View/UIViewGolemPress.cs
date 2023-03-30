using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGolemPress : BaseUIView, SelectView.ICallBack
{
    protected Vector3Int blockWorldPosition;

    protected int indexSelectMaterial;
    protected int indexSelectHead;
    protected int indexSelectHand;
    protected int indexSelectFoot;
    protected int indexSelectAccessory;

    protected List<string> listNameOptionsMaterial = new List<string>();
    protected List<string> listNameOptionsHead = new List<string>();
    protected List<string> listNameOptionsHand = new List<string>();
    protected List<string> listNameOptionsFoot = new List<string>();
    protected List<string> listNameOptionsAccessory = new List<string>();

    protected List<GolemPressInfoBean> listGolemPressMaterial;
    protected List<GolemPressInfoBean> listGolemPressHead;
    protected List<GolemPressInfoBean> listGolemPressHand;
    protected List<GolemPressInfoBean> listGolemPressFoot;
    protected List<GolemPressInfoBean> listGolemPressAccessory;
    //选择的材料数据
    protected List<ItemsBean> listSelectMaterialsData = new List<ItemsBean>();
    public override void Awake()
    {
        base.Awake();

        ui_ItemGolem.SetLimitType(ItemsTypeEnum.Gloem);
        ui_ItemGolem.SetCallBackForSetViewItem(CallBackForItemChange);

        ui_Option_Material.SetCallBack(this);
        ui_Option_Head.SetCallBack(this);
        ui_Option_Hand.SetCallBack(this);
        ui_Option_Foot.SetCallBack(this);
        ui_Option_Accessory.SetCallBack(this);
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        ui_SubmitTex.text = TextHandler.Instance.GetTextById(550);

        ui_OptionsTitle.text = TextHandler.Instance.GetTextById(501);
        ui_MaterialTitle.text = TextHandler.Instance.GetTextById(502);

        ui_Option_Material.SetTitle(TextHandler.Instance.GetTextById(551));
        ui_Option_Head.SetTitle(TextHandler.Instance.GetTextById(552));
        ui_Option_Hand.SetTitle(TextHandler.Instance.GetTextById(553));
        ui_Option_Foot.SetTitle(TextHandler.Instance.GetTextById(554));
        ui_Option_Accessory.SetTitle(TextHandler.Instance.GetTextById(555));
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int worldPosition)
    {
        this.blockWorldPosition = worldPosition;
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out Chunk chunk);
        if (chunk != null && block != null && block.blockType == BlockTypeEnum.GolemPress)
        {
            block.GetBlockMetaData(chunk, worldPosition - chunk.chunkData.positionForWorld, out BlockBean blockData, out BlockMetaGolemPress blockMetaGolemPress);
            //设置选项
            ui_ItemGolem.SetViewItemByData(blockMetaGolemPress.itemsGolem);
        }
        else
        {
            ui_ItemGolem.SetViewItemByData(null);
        }
        InitOptions();
    }

    /// <summary>
    /// 初始化选项
    /// </summary>
    public void InitOptions()
    {
        InitOptionsForMaterial();
        InitOptionsForHead();
        InitOptionsForHand();
        InitOptionsForFoot();
        InitOptionsForAccessory();

        ui_Option_Material.SetPosition(0);
        ui_Option_Head.SetPosition(0);
        ui_Option_Hand.SetPosition(0);
        ui_Option_Foot.SetPosition(0);
        ui_Option_Accessory.SetPosition(0);
    }

    public void InitOptionsForMaterial()
    {
        listGolemPressMaterial = GolemPressInfoCfg.GetGolemPressInfoByType(GolemPressTypeEnum.Material);
        InitOptionsForBase(ui_Option_Material, listGolemPressMaterial, listNameOptionsMaterial, false);
    }
    public void InitOptionsForHead()
    {
        listGolemPressHead = GolemPressInfoCfg.GetGolemPressInfoByType(GolemPressTypeEnum.Head);
        InitOptionsForBase(ui_Option_Head, listGolemPressHead, listNameOptionsHead, true);
    }
    public void InitOptionsForHand()
    {
        listGolemPressHand = GolemPressInfoCfg.GetGolemPressInfoByType(GolemPressTypeEnum.Hand);
        InitOptionsForBase(ui_Option_Hand, listGolemPressHand, listNameOptionsHand, true);
    }
    public void InitOptionsForFoot()
    {
        listGolemPressFoot = GolemPressInfoCfg.GetGolemPressInfoByType(GolemPressTypeEnum.Foot);
        InitOptionsForBase(ui_Option_Foot, listGolemPressFoot, listNameOptionsFoot, true);
    }
    public void InitOptionsForAccessory()
    {
        listGolemPressAccessory = GolemPressInfoCfg.GetGolemPressInfoByType(GolemPressTypeEnum.Accessory);
        InitOptionsForBase(ui_Option_Accessory, listGolemPressAccessory, listNameOptionsAccessory, true);
    }

    public void InitOptionsForBase(SelectView selectView, List<GolemPressInfoBean> listData, List<string> listName, bool hasNull)
    {
        if (listData == null)
        {
            listData = new List<GolemPressInfoBean>();
        }
        listName.Clear();
        if (hasNull)
        {
            listName.Add(TextHandler.Instance.GetTextById(556));
        }
        for (int i = 0; i < listData.Count; i++)
        {
            var itemData = listData[i];
            string name = itemData.GetName();
            listName.Add(name);
        }
        selectView.SetListData(listName);
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="viewButton"></param>
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_SubmitBtn)
        {
            HandleForSubmit();
        }
    }

    /// <summary>
    /// 处理点击制造
    /// </summary>
    public void HandleForSubmit()
    {
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (chunk == null || block == null || block.blockType != BlockTypeEnum.GolemPress)
            return;

        //检测是否有傀儡放在那个位置
        block.GetBlockMetaData(chunk, blockWorldPosition - chunk.chunkData.positionForWorld, out BlockBean blockData, out BlockMetaGolemPress blockMetaGolemPress);
        if (blockMetaGolemPress.itemsGolem.itemId != 0)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30010));
            return;
        }

        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

        //检测是否有足够的材料
        bool hasEnoughItem = userData.HasEnoughItem(listSelectMaterialsData);
        if (!hasEnoughItem)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30002));
            return;
        }

        AudioHandler.Instance.PlaySound(201);
        //扣除材料
        userData.RemoveItem(listSelectMaterialsData);

        ItemMetaGolem itemMetaGolem = new ItemMetaGolem();
        itemMetaGolem.material = (int)listGolemPressMaterial[indexSelectMaterial].id;
        if (indexSelectHead != 0)
            itemMetaGolem.head = (int)listGolemPressHead[indexSelectHead].id;
        if (indexSelectHand != 0)
            itemMetaGolem.hand = (int)listGolemPressHand[indexSelectHand].id;
        if (indexSelectFoot != 0)
            itemMetaGolem.foot = (int)listGolemPressFoot[indexSelectFoot].id;
        if (indexSelectAccessory != 0)
            itemMetaGolem.accessory = (int)listGolemPressAccessory[indexSelectAccessory].id;

        //设置核心数量
        int golemCoreNum = 1;
        itemMetaGolem.listGolemCore.Clear();
        for (int i = 0; i < golemCoreNum; i++)
        {
            ItemsBean itemsGolemCore = new ItemsBean(0, 1);
            itemMetaGolem.listGolemCore.Add(itemsGolemCore);
        }

        var blockTypeGolemPress = block as BlockTypeGolemPress;
        blockMetaGolemPress.itemsGolem.itemId = 9900001;
        blockMetaGolemPress.itemsGolem.number = 1;
        blockMetaGolemPress.itemsGolem.SetMetaData(itemMetaGolem);

        //播放工作动画
        blockTypeGolemPress.PlayWorkAnim(blockWorldPosition);

        blockData.SetBlockMeta(blockMetaGolemPress);
        chunk.SetBlockData(blockData);
        //设置选项
        ui_ItemGolem.SetViewItemByData(blockMetaGolemPress.itemsGolem);
        //刷新UI
        UIHandler.Instance.RefreshUI();
    }

    //是否正在设置材质
    protected bool isSetMaterials = false;
    /// <summary>
    /// 设置材料
    /// </summary>
    public void SetMaterials()
    {
        if (isSetMaterials)
            return;
        isSetMaterials = true;

        listSelectMaterialsData.Clear();
        var itemMaterial = listGolemPressMaterial[indexSelectMaterial];
        ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemMaterial.materials));

        if (indexSelectHead != 0)
        {
            var itemHead = listGolemPressHead[indexSelectHead];
            ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemHead.materials));
        }

        if (indexSelectHand != 0)
        {
            var itemHand = listGolemPressHand[indexSelectHand];
            ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemHand.materials));
        }

        if (indexSelectFoot != 0)
        {
            var itemFoot = listGolemPressFoot[indexSelectFoot];
            ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemFoot.materials));
        }

        if (indexSelectAccessory != 0)
        {
            var itemAccessory = listGolemPressAccessory[indexSelectAccessory];
            ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemAccessory.materials));
        }

        //延迟一帧。防止多次设置
        this.WaitExecuteEndOfFrame(1, () =>
        {
            ui_ViewMaterialsShow.SetData(listSelectMaterialsData);
            isSetMaterials = false;
        });
    }

    /// <summary>
    /// 道具回调
    /// </summary>
    public void CallBackForItemChange(UIViewItemContainer changeItemContainer, ItemsBean changeItemData)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (chunk != null && block != null && block.blockType == BlockTypeEnum.GolemPress)
        {
            block.GetBlockMetaData(chunk, blockWorldPosition - chunk.chunkData.positionForWorld, out BlockBean blockData, out BlockMetaGolemPress blockMetaGolemPress);

            blockMetaGolemPress.itemsGolem = changeItemData;
            blockData.SetBlockMeta(blockMetaGolemPress);
            chunk.SetBlockData(blockData);
        }
    }



    #region 选择回调
    public void ChangeSelectPosition(SelectView selectView, int position)
    {
        if (selectView == ui_Option_Material)
        {
            indexSelectMaterial = position;
        }
        else if (selectView == ui_Option_Head)
        {
            indexSelectHead = position;
        }
        else if (selectView == ui_Option_Hand)
        {
            indexSelectHand = position;
        }
        else if (selectView == ui_Option_Foot)
        {
            indexSelectFoot = position;
        }
        else if (selectView == ui_Option_Accessory)
        {
            indexSelectAccessory = position;
        }
        SetMaterials();
    }
    #endregion



}