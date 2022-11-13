using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public partial class UIViewFocalManipulator : BaseUIView
{
    protected BlockBean blockData;
    protected BlockMetaFocalManipulator blockMetaData;
    protected Vector3Int blockWorldPosition;

    public override void Awake()
    {
        base.Awake();
        ui_ItemMagicCore.SetCallBackForSetViewItem(CallBackForItemChange);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        //事件通知更新
        this.RegisterEvent<Vector3Int>(EventsInfo.BlockTypeFocalManipulator_UpdateWork, CallBackForBlockUpdate);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_ItemMagicCore.ClearViewItem();
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

        ui_ItemMagicCore.SetViewItemByData(blockMetaData.itemMagicCore);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_SubmitBtn)
        {
            HandleForSubmit();
        }
    }

    /// <summary>
    /// 处理-点击
    /// </summary>
    public void HandleForSubmit()
    {
        //生成特效
        if (blockMetaData.workPro != 0)
            return;
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (chunk == null || block == null || block.blockType != BlockTypeEnum.FocalManipulator)
            return;
        var blockFocalManipulator = block as BlockTypeFocalManipulator;
        blockFocalManipulator.StartWork(chunk, blockWorldPosition);
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

}