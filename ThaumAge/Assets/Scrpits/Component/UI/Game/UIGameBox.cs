﻿using UnityEditor;
using UnityEngine;
public partial class UIGameBox : UIGameCommonNormal
{
    protected BlockBaseBox blockBox;
    protected BlockBean blockData;
    protected Vector3Int blockWorldPosition;

    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
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
        blockBox = block as BlockBaseBox;
        //获取方块数据
        blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);
        //设置数据
        ui_ViewBoxList.SetData(worldPosition, blockData);
    }

    /// <summary>
    /// 退出箱子
    /// </summary>
    public override void HandleForBackMain()
    {
        base.HandleForBackMain();
        //关闭箱子
        blockBox.CloseBox(blockWorldPosition);
    }


}