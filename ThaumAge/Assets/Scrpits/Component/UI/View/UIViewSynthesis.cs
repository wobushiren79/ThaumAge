using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public partial class UIViewSynthesis : BaseUIView
{
    //合成数据
    protected List<ItemsSynthesisBean> listSynthesisData;
    //当前选中项
    protected int indexSelect = -1;
    //素材UI
    protected List<UIViewSynthesisMaterial> listUIMaterial;

    protected Queue<GameObject> poolEffectItemSynthesis = new Queue<GameObject>();

    protected ItemsSynthesisTypeEnum itemsSynthesisType = ItemsSynthesisTypeEnum.Self;

    //目标方块位置
    protected Vector3Int targetBlockWorldPosition;

    //是否循环查询更新方块数据
    protected bool isLoopUpdateBlockMetaData = false;
    protected float timeUpdateBlockMetaData = 0;
    public override void Awake()
    {
        base.Awake();
        ui_SynthesisList.AddCellListener(OnCellForItemSynthesis);
        ui_ModelViewSynthesisMaterial.ShowObj(false);
        ui_ModelEffectItemSynthesis.ShowObj(false);
    }

    public void Update()
    {
        if (isLoopUpdateBlockMetaData)
        {
            timeUpdateBlockMetaData += Time.deltaTime;
            if (timeUpdateBlockMetaData >= 1)
            {
                RefreshMagicProgress();
                timeUpdateBlockMetaData = 0;
            }
        } 
    }

    public override void OpenUI()
    {
        indexSelect = -1;
        base.OpenUI();
        this.RegisterEvent<int>(EventsInfo.UIViewSynthesis_SetSelect, SetSelect);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ResetAnimArcaneTable();
        switch (itemsSynthesisType)
        {
            case ItemsSynthesisTypeEnum.Arcane:

                break;
            default:
                break;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ResetAnimArcaneTable();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        listSynthesisData = ItemsHandler.Instance.manager.GetItemsSynthesisByType(itemsSynthesisType);
        ui_SynthesisList.SetCellCount(listSynthesisData.Count);
        RefreshMaterials();
        RefreshUIText();
        if (indexSelect == -1)
        {
            SetSelect(0);
        }
        else
        {
            SetSelect(indexSelect);
        }
    }

    /// <summary>
    /// 刷新UI文本
    /// </summary>
    public void RefreshUIText()
    {
        ui_TVBtnSynthesis.text = TextHandler.Instance.GetTextById(311);
        ui_Null.text = TextHandler.Instance.GetTextById(10);
    }

    /// <summary>
    /// 刷新魔力进度
    /// </summary>
    public void RefreshMagicProgress()
    {
        //设置魔力进度
        switch (itemsSynthesisType)
        {
            case ItemsSynthesisTypeEnum.Arcane:
                //设置魔力进度
                GetBlockMetaData(targetBlockWorldPosition, out BlockBean blockData, out BlockMetaCraftingTable blockMetaData);
                SetMagicProgress(blockMetaData.maxMagic, blockMetaData.magic);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 设置数据类型
    /// </summary>
    /// <param name="itemsSynthesisType"></param>
    public void SetData(ItemsSynthesisTypeEnum itemsSynthesisType, Vector3Int blockWorldPosition)
    {
        this.targetBlockWorldPosition = blockWorldPosition;
        this.itemsSynthesisType = itemsSynthesisType;
        switch (itemsSynthesisType)
        {
            case ItemsSynthesisTypeEnum.Arcane:
                ui_ArcaneBackground.ShowObj(true);
                ui_MagicPay.ShowObj(true);
                ui_MagicTotal.ShowObj(true);
                isLoopUpdateBlockMetaData = true;
                break;
            default:
                ui_ArcaneBackground.ShowObj(false);
                ui_MagicPay.ShowObj(false);
                ui_MagicTotal.ShowObj(false);
                isLoopUpdateBlockMetaData = false;
                break;
        }
        RefreshMagicProgress();
    }

    /// <summary>
    /// 获取对应方块数据
    /// </summary>
    public void GetBlockMetaData(Vector3Int blockWorldPosition, out BlockBean blockData, out BlockMetaCraftingTable blockMetaData)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block targetBlock,out BlockDirectionEnum blockDirection, out Chunk targetChunk);
        blockData = targetChunk.GetBlockData(blockWorldPosition - targetChunk.chunkData.positionForWorld);
        if (blockData == null)
        {
            blockData = new BlockBean(blockWorldPosition - targetChunk.chunkData.positionForWorld, targetBlock.blockType, blockDirection);
        }
        blockMetaData = blockData.GetBlockMeta<BlockMetaCraftingTable>();
        int totalMagic = 0;
        if (targetBlock is BlockTypeCraftingTableArcane craftingTableArcane)
        {
            totalMagic = craftingTableArcane.GetAroundMagicTotal(blockWorldPosition);
        }
        if (blockMetaData == null)
        {
            blockMetaData = new BlockMetaCraftingTable();
            blockMetaData.magic = totalMagic;
            blockMetaData.maxMagic = totalMagic;
        }
        else
        {
            TimeBean timePlay = GameTimeHandler.Instance.manager.GetPlayTime();
            int addMagic = blockMetaData.GetTimeAddMagic(timePlay);
            blockMetaData.maxMagic = totalMagic;
            blockMetaData.AddMagic(addMagic);
        }
        blockData.meta = blockMetaData.ToJson();
        targetChunk.SetBlockData(blockData);
    }

    /// <summary>
    /// 保存对应方块数据
    /// </summary>
    public bool SaveBlockMetaData(Vector3Int blockWorldPosition, BlockBean blockData, BlockMetaCraftingTable blockMetaData)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block targetBlock, out Chunk targetChunk);
        if (targetChunk == null || targetBlock == null
            || targetBlock.blockType != BlockTypeEnum.CraftingTableArcane)
        {
            return false;
        }
        TimeBean timePlay = GameTimeHandler.Instance.manager.GetPlayTime();
        blockMetaData.closeTime = timePlay;
        blockData.meta = blockMetaData.ToJson();
        targetChunk.SetBlockData(blockData);
        return true;
    }

    /// <summary>
    /// 点击
    /// </summary>
    /// <param name="viewButton"></param>
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_BtnSynthesis)
        {
            OnClickForStartSynthesis();
        }
    }

    /// <summary>
    /// 单个数据
    /// </summary>
    /// <param name="itemView"></param>
    public void OnCellForItemSynthesis(ScrollGridCell itemView)
    {
        UIViewSynthesisItem itemSynthesis = itemView.GetComponent<UIViewSynthesisItem>();
        //item数据
        ItemsSynthesisBean itemData = listSynthesisData[itemView.index];
        //是否选中当前
        bool isSelect = (itemView.index == indexSelect ? true : false);
        //设置数据
        itemSynthesis.SetData(itemData, itemView.index, isSelect);
    }

    /// <summary>
    /// 点击-开始合成
    /// </summary>
    public void OnClickForStartSynthesis()
    {
        //当前选中的合成道具
        ItemsSynthesisBean itemsSynthesis = listSynthesisData[indexSelect];
        //检测当前道具是否能合成
        bool canSynthesis = itemsSynthesis.CheckSynthesis();

        if (!canSynthesis)
        {
            //素材不足 无法合成
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30002));
            return;
        }

        //如果是奥术制造台 还需要判断一下魔力
        if (itemsSynthesisType == ItemsSynthesisTypeEnum.Arcane)
        {
            GetBlockMetaData(targetBlockWorldPosition, out BlockBean blockData, out BlockMetaCraftingTable blockMetaData);
            if (blockMetaData.magic < itemsSynthesis.magic_pay)
            {
                //魔力不足 无法合成
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30004));
                return;
            }
            //扣除魔力
            blockMetaData.AddMagic(-itemsSynthesis.magic_pay);
            bool isSaveBlockMetaDat = SaveBlockMetaData(targetBlockWorldPosition, blockData, blockMetaData);
            if (!isSaveBlockMetaDat)
            {            
                //目标方块不存在 无法合成
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30005));
                return;
            }
        }
        //首先消耗素材
        BaseUIComponent currentUI = UIHandler.Instance.GetOpenUI();
        //获取素材
        List<ItemsArrayBean> listMaterials = itemsSynthesis.GetSynthesisMaterials();

        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

        //扣除素材
        for (int i = 0; i < listMaterials.Count; i++)
        {
            ItemsArrayBean itemMaterials = listMaterials[i];
            for (int f = 0; f < itemMaterials.itemIds.Length; f++)
            {
                //只要扣除其中一项素材就行
                long itemMaterialId = itemMaterials.itemIds[f];
                if (userData.HasEnoughItem(itemMaterialId, itemMaterials.itemNumber))
                {
                    userData.RemoveItem(itemMaterialId, itemMaterials.itemNumber);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        //添加道具
        itemsSynthesis.GetSynthesisResult(out long itemsId, out int itemNum);
        int moreNum = userData.AddItems(itemsId, itemNum, null);
        //如果还有多余的道具 则丢出来
        if (moreNum > 0)
        {
            Player player = GameHandler.Instance.manager.player;
            ItemDropBean itemDropData = new ItemDropBean(itemsId, player.transform.position + Vector3.up * 1.25f, moreNum, null, ItemDropStateEnum.DropNoPick);
            ItemsHandler.Instance.CreateItemCptDrop(itemDropData);
        }

        UIViewBackpackList backpackUI = currentUI.GetComponentInChildren<UIViewBackpackList>();
        backpackUI.RefreshUI();
        UIViewShortcuts shortcutsUI = currentUI.GetComponentInChildren<UIViewShortcuts>();
        shortcutsUI.RefreshUI();
        RefreshUI();

        //播放音效
        AudioHandler.Instance.PlaySound(201);
        //播放道具合成特效
        PlayEffectItemSynthesis();
        //刷新魔力
        RefreshMagicProgress();
    }

    /// <summary>
    /// 动画相关参数
    /// </summary>
    protected float timeAnimEffectItem = 0.5f;
    protected float scaleAnimEffectItem = 1.5f;
    protected float moveYAnimEffectItem = 100f;

    /// <summary>
    /// 播放道具合成特效
    /// </summary>
    public void PlayEffectItemSynthesis()
    {
        GameObject objItem;
        if (poolEffectItemSynthesis.Count > 0)
        {
            objItem = poolEffectItemSynthesis.Dequeue();
        }
        else
        {
            objItem = Instantiate(gameObject, ui_ModelEffectItemSynthesis.gameObject);
        }
        objItem.ShowObj(true);
        objItem.transform.position = ui_SynthesisResults.transform.position;
        RectTransform rtfItem = (RectTransform)objItem.transform;
        Image imgItem = objItem.GetComponent<Image>();
        imgItem.sprite = ui_SynthesisResults.ui_ItemIcon.sprite;
        imgItem.color = new Color(imgItem.color.r, imgItem.color.g, imgItem.color.b, 1);
        rtfItem.localScale = Vector3.one;
        //开始动画
        imgItem.DOFade(0, timeAnimEffectItem);
        rtfItem.DOScale(scaleAnimEffectItem, timeAnimEffectItem);
        rtfItem.DOAnchorPosY(rtfItem.anchoredPosition.y + moveYAnimEffectItem, timeAnimEffectItem).OnComplete(() =>
        {
            objItem.ShowObj(false);
            poolEffectItemSynthesis.Enqueue(objItem);
        });
    }

    /// <summary>
    /// 设置魔力
    /// </summary>
    public void SetMagicPay(int payMagic)
    {
        ui_MagicPay.text = $"{TextHandler.Instance.GetTextById(303)}:{payMagic}";
    }

    /// <summary>
    /// 设置魔力进度条
    /// </summary>
    public void SetMagicProgress(float maxData, float data)
    {
        ui_MagicTotal.SetData(maxData, data);
    }

    /// <summary>
    /// 设置选中第几项
    /// </summary>
    /// <param name="indexSelect"></param>
    public void SetSelect(int indexSelect)
    {
        ResetAnimArcaneTable();

        if (listSynthesisData.Count == 0)
        {
            ui_Top.ShowObj(false);
            ui_Bottom.ShowObj(false);
            ui_Null.ShowObj(true);
            return;
        }
        else
        {
            ui_Top.ShowObj(true);
            ui_Bottom.ShowObj(true);
            ui_Null.ShowObj(false);
        }
        bool isSameSelect;
        if (this.indexSelect == indexSelect)
        {
            isSameSelect = true;
        }
        else
        {
            isSameSelect = false;
        }
        this.indexSelect = indexSelect;
        ItemsSynthesisBean curSelectItemsSynthesis = listSynthesisData[indexSelect];
        //刷新结果
        ui_SynthesisResults.SetData(curSelectItemsSynthesis, -1, false);
        //刷新列表
        ui_SynthesisList.RefreshAllCells();
        //检测当前道具是否能合成
        bool canSynthesis = curSelectItemsSynthesis.CheckSynthesis();
        if (canSynthesis)
        {
            ui_TVBtnSynthesis.color = Color.green;
            //如果是奥术制造台 开启特效
            if (itemsSynthesisType == ItemsSynthesisTypeEnum.Arcane)
            {
                ui_ArcaneBackgroundTable_2.DOColor(Color.green, 2).SetLoops(-1, LoopType.Yoyo);
            }
        }
        else
        {
            ui_TVBtnSynthesis.color = Color.gray;
        }
        if (isSameSelect)
        {

        }
        else
        {
            //刷新素材
            SetSynthesisMaterials();
        }
        //设置魔力消耗
        SetMagicPay(curSelectItemsSynthesis.magic_pay);
    }

    /// <summary>
    /// 还原动画
    /// </summary>
    protected void ResetAnimArcaneTable()
    {
        //还原颜色
        ColorUtility.TryParseHtmlString("#044816", out Color colorDefArance);
        ui_ArcaneBackgroundTable_2.DOKill();
        ui_ArcaneBackgroundTable_2.color = colorDefArance;
    }

    /// <summary>
    /// 设置素材
    /// </summary>
    public void SetSynthesisMaterials()
    {
        if (listUIMaterial == null)
            listUIMaterial = new List<UIViewSynthesisMaterial>();
        listUIMaterial.Clear();

        //先删除所有素材
        ui_SynthesisMaterials.DestroyAllChild();
        //获取当前选中合成道具
        ItemsSynthesisBean itemsSynthesis = listSynthesisData[indexSelect];
        List<ItemsArrayBean> listMaterials = itemsSynthesis.GetSynthesisMaterials();
        //获取起始点位置
        Vector2[] listCirclePosition = VectorUtil.GetListCirclePosition(listMaterials.Count, 0, Vector2.zero, 120);
        //创建所有素材
        int itemAngle = 360 / listMaterials.Count;

        for (int i = 0; i < listMaterials.Count; i++)
        {
            GameObject objMaterial = Instantiate(ui_SynthesisMaterials.gameObject, ui_ModelViewSynthesisMaterial.gameObject);
            UIViewSynthesisMaterial itemMaterial = objMaterial.GetComponent<UIViewSynthesisMaterial>();

            ItemsArrayBean itemData = listMaterials[i];
            itemMaterial.SetData(itemData, itemAngle * i);
            itemMaterial.rectTransform.anchoredPosition = listCirclePosition[i];
            listUIMaterial.Add(itemMaterial);
        }
    }

    /// <summary>
    /// 刷新素材显示
    /// </summary>
    public void RefreshMaterials()
    {
        if (listUIMaterial == null)
            listUIMaterial = new List<UIViewSynthesisMaterial>();
        for (int i = 0; i < listUIMaterial.Count; i++)
        {
            UIViewSynthesisMaterial itemUIMaterial = listUIMaterial[i];
            itemUIMaterial.RefreshUI();
        }
    }
}