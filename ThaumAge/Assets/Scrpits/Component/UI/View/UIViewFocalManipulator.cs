using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public partial class UIViewFocalManipulator : BaseUIView, SelectView.ICallBack
{
    protected BlockBean blockData;
    protected BlockMetaFocalManipulator blockMetaData;
    protected Vector3Int blockWorldPosition;

    //元素选项
    protected List<ResearchInfoBean> listElementsOptionsInfo = new List<ResearchInfoBean>();
    protected List<string> listElementsOptionsName = new List<string>();
    protected int indexSelectElements;
    //法术生成选项
    protected List<ResearchInfoBean> listCreateOptionsInfo = new List<ResearchInfoBean>();
    protected List<string> listCreateOptionsName = new List<string>();
    protected int indexSelectCreate;
    //射程选项
    protected List<ResearchInfoBean> listRangeOptionsInfo = new List<ResearchInfoBean>();
    protected List<string> listRangeOptionsName = new List<string>();
    protected int indexSelectRange;
    //范围选项
    protected List<ResearchInfoBean> listScopeOptionsInfo = new List<ResearchInfoBean>();
    protected List<string> listScopeOptionsName = new List<string>();
    protected int indexSelectScope;
    //威力选项
    protected List<ResearchInfoBean> listPowerOptionsInfo = new List<ResearchInfoBean>();
    protected List<string> listPowerOptionsName = new List<string>();
    protected int indexSelectPower;
    //魔法消耗选项
    protected List<ResearchInfoBean> listMagicPayOptionsInfo = new List<ResearchInfoBean>();
    protected List<string> listMagicPayOptionsName = new List<string>();
    protected int indexSelectMagicPay;

    //选择的材料数据
    protected List<ItemsBean> listSelectMaterialsData = new List<ItemsBean>();

    public override void Awake()
    {
        base.Awake();
        ui_ItemMagicCore.SetCallBackForSetViewItem(CallBackForItemChange);
        ui_Option_Element.SetCallBack(this);
        ui_Option_CreateWay.SetCallBack(this);
        ui_Option_Range.SetCallBack(this);
        ui_Option_Scope.SetCallBack(this);
        ui_Option_Power.SetCallBack(this);
        ui_Option_MagicPay.SetCallBack(this);
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        ui_SubmitTex.text = TextHandler.Instance.GetTextById(500);

        ui_OptionsTitle.text = TextHandler.Instance.GetTextById(501);
        ui_MaterialTitle.text = TextHandler.Instance.GetTextById(502);

        ui_Option_Element.SetTitle(TextHandler.Instance.GetTextById(503));
        ui_Option_CreateWay.SetTitle(TextHandler.Instance.GetTextById(504));
        ui_Option_Range.SetTitle(TextHandler.Instance.GetTextById(505));
        ui_Option_Scope.SetTitle(TextHandler.Instance.GetTextById(506));
        ui_Option_Power.SetTitle(TextHandler.Instance.GetTextById(507));
        ui_Option_MagicPay.SetTitle(TextHandler.Instance.GetTextById(508));
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_ChangePro.value = 0;
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
        //获取方块数据
        blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);
        blockMetaData = blockData.GetBlockMeta<BlockMetaFocalManipulator>();
        if (blockMetaData == null)
            blockMetaData = new BlockMetaFocalManipulator();

        ui_ItemMagicCore.SetViewItemByData(blockMetaData.itemMagicCore,false);

        //设置选项
        InitOptions();

        //事件通知更新
        this.RegisterEvent<Vector3Int>(EventsInfo.BlockTypeFocalManipulator_UpdateWork, CallBackForBlockUpdate);
    }

    /// <summary>
    /// 设置选项
    /// </summary>
    public void InitOptions()
    {
        InitOptionsForElements();
        InitOptionsForCreate();
        InitOptionsForRange();
        InitOptionsForScope();
        InitOptionsForPower();
        InitOptionsForMagicPay();

        ui_Option_Element.SetPosition(0);
        ui_Option_CreateWay.SetPosition(0);
        ui_Option_Range.SetPosition(0);
        ui_Option_Scope.SetPosition(0);
        ui_Option_Power.SetPosition(0);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_SubmitBtn)
        {
            HandleForSubmit();
        }
    }

    protected void GetOptionsData(int researchDetailsType, List<ResearchInfoBean> listTargetData, List<string> listTargetDataName)
    {
        List<ResearchInfoBean> listData = ResearchInfoCfg.GetResearchInfoByType(1, researchDetailsType);
        listTargetData.Clear();
        listTargetDataName.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            var itemData = listData[i];
            if (itemData.need_unlock == 0)
            {
                listTargetDataName.Add(itemData.GetName());
                listTargetData.Add(itemData);
                continue;
            }
            else
            {
                //判断是否解锁
                UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                bool isUnlock = userData.userAchievement.CheckUnlockResearch((int)itemData.id);
                if (isUnlock)
                {
                    listTargetDataName.Add(itemData.GetName());
                    listTargetData.Add(itemData);
                }
            }
        }
    }

    /// <summary>
    /// 法术元素选项
    /// </summary>
    public void InitOptionsForElements()
    {
        GetOptionsData(1, listElementsOptionsInfo, listElementsOptionsName);
        ui_Option_Element.SetListData(listElementsOptionsName);
    }

    /// <summary>
    /// 法术生成选项
    /// </summary>
    public void InitOptionsForCreate()
    {
        GetOptionsData(2, listCreateOptionsInfo, listCreateOptionsName);
        ui_Option_CreateWay.SetListData(listCreateOptionsName);
    }

    /// <summary>
    ///  //射程选项
    /// </summary>
    public void InitOptionsForRange()
    {
        GetOptionsData(3, listRangeOptionsInfo, listRangeOptionsName);
        ui_Option_Range.SetListData(listRangeOptionsName);
    }

    /// <summary>
    /// 范围选项
    /// </summary>
    public void InitOptionsForScope()
    {
        GetOptionsData(4, listScopeOptionsInfo, listScopeOptionsName);
        ui_Option_Scope.SetListData(listScopeOptionsName);
    }

    /// <summary>
    /// 威力选项
    /// </summary>
    public void InitOptionsForPower()
    {
        GetOptionsData(5, listPowerOptionsInfo, listPowerOptionsName);
        ui_Option_Power.SetListData(listPowerOptionsName);
    }


    /// <summary>
    /// 魔法消耗选项
    /// </summary>
    public void InitOptionsForMagicPay()
    {
        //暂时取消魔法消耗
        ui_Option_MagicPay.ShowObj(false);
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
        var itemElements = listElementsOptionsInfo[indexSelectElements];
        ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemElements.materials));

        var itemCreate = listCreateOptionsInfo[indexSelectCreate];
        ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemCreate.materials));

        var itemRange = listRangeOptionsInfo[indexSelectRange];
        ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemRange.materials));

        var itemScope = listScopeOptionsInfo[indexSelectScope];
        ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemScope.materials));

        var itemPower = listPowerOptionsInfo[indexSelectPower];
        ItemsBean.CombineItems(listSelectMaterialsData, ItemsBean.GetListItemsBean(itemPower.materials));

        //var itemMagicPay = listMagicPayOptionsInfo[indexSelectMagicPay];
        //HandleMaterialsList(listSelectMaterialsData, ItemsBean.GetListItemsArrayBean(itemMagicPay.materials));

        //延迟一帧。防止多次设置
        this.WaitExecuteEndOfFrame(1, () =>
        {
            ui_ViewMaterialsShow.SetData(listSelectMaterialsData);
            isSetMaterials = false;
        });
    }
    /// <summary>
    /// 处理-点击
    /// </summary>
    public void HandleForSubmit()
    {
        AudioHandler.Instance.PlaySound(1);
        //生成特效
        if (blockMetaData.workPro != 0)
            return;
        //判断是否有放入核心
        if (blockMetaData.itemMagicCore == null || blockMetaData.itemMagicCore.itemId == 0)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30009));
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
        //扣除材料
        userData.RemoveItem(listSelectMaterialsData);

        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (chunk == null || block == null || block.blockType != BlockTypeEnum.FocalManipulator)
            return;
        var blockFocalManipulator = block as BlockTypeFocalManipulator;

        //设置相关数据
        ItemMetaMagicCore itemMetaMagic = new ItemMetaMagicCore();
        var dataElemental = listElementsOptionsInfo[indexSelectElements];
        var dataCreate = listCreateOptionsInfo[indexSelectCreate];
        var dataRange = listRangeOptionsInfo[indexSelectRange];
        var dataScope = listScopeOptionsInfo[indexSelectScope];
        var dataPower = listScopeOptionsInfo[indexSelectPower];

        itemMetaMagic.elemental = int.Parse(dataElemental.data_research);
        itemMetaMagic.create = int.Parse(dataCreate.data_research);
        itemMetaMagic.range = int.Parse(dataRange.data_research);
        itemMetaMagic.scope = int.Parse(dataScope.data_research);
        itemMetaMagic.power = int.Parse(dataPower.data_research);

        blockMetaData.itemMagicCoreWorkTemp.itemId = blockMetaData.itemMagicCore.itemId;
        blockMetaData.itemMagicCoreWorkTemp.number = blockMetaData.itemMagicCore.number;
        blockMetaData.itemMagicCoreWorkTemp.SetMetaData(itemMetaMagic);

        blockMetaData.workPro = 0.01f;
        blockData.SetBlockMeta(blockMetaData);
        chunk.SetBlockData(blockData);

        blockFocalManipulator.StartWork(chunk, blockWorldPosition);
        //刷新UI
        UIHandler.Instance.RefreshUI();
    }

    /// <summary>
    /// 道具回调
    /// </summary>
    public void CallBackForItemChange(UIViewItemContainer changeItemContainer, ItemsBean changeItemData)
    {
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (chunk == null || block == null || block.blockType != BlockTypeEnum.FocalManipulator)
            return;
        if (changeItemContainer != ui_ItemMagicCore)
            return;
        chunk.UnRegisterEventUpdate(blockWorldPosition - chunk.chunkData.positionForWorld, TimeUpdateEventTypeEnum.Sec);
        //重置工作进度
        blockMetaData.workPro = 0;
        ui_ChangePro.value = 0;
        //设置一下核心
        var blockFocalManipulator = block as BlockTypeFocalManipulator;
        blockFocalManipulator.SetMagicCore(blockWorldPosition, changeItemData);
        SaveBlockData(chunk);
    }


    /// <summary>
    /// 方块工作更新回调
    /// </summary>
    /// <param name="blockPosition"></param>
    public void CallBackForBlockUpdate(Vector3Int blockPosition)
    {
        if (blockPosition != blockWorldPosition)
            return;
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (chunk == null || block == null || block.blockType != BlockTypeEnum.FocalManipulator)
            return;
        //获取方块数据
        blockData = chunk.GetBlockData(blockPosition - chunk.chunkData.positionForWorld);
        blockMetaData = blockData.GetBlockMeta<BlockMetaFocalManipulator>();
        if (blockMetaData == null)
            return;
        ui_ChangePro.value = blockMetaData.workPro;
        //如果进度为0 说明工作已经结束，需要刷新一下UI
        if (ui_ChangePro.value == 0)
        {
            ui_ItemMagicCore.SetViewItemByData(blockMetaData.itemMagicCore, false);
        }
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public void SaveBlockData(Chunk chunk)
    {
        var magicView = ui_ItemMagicCore.GetViewItem();

        if (magicView != null)
        {
            blockMetaData.itemMagicCore = ui_ItemMagicCore.itemsData;
        }
        else
        {
            blockMetaData.itemMagicCore = null;
        }
        blockData.SetBlockMeta(blockMetaData);
        chunk.SetBlockData(blockData);
    }

    #region 选择回调
    public void ChangeSelectPosition(SelectView selectView, int position)
    {

        if (selectView == ui_Option_Element)
        {
            indexSelectElements = position;
        }
        else if (selectView == ui_Option_CreateWay)
        {
            indexSelectCreate = position;
        }
        else if (selectView == ui_Option_Range)
        {
            indexSelectRange = position;
        }
        else if (selectView == ui_Option_Scope)
        {
            indexSelectScope = position;
        }
        else if (selectView == ui_Option_Power)
        {
            indexSelectPower = position;
        }
        else if (selectView == ui_Option_MagicPay)
        {
            indexSelectMagicPay = position;
        }

        SetMaterials();
    }
    #endregion
}