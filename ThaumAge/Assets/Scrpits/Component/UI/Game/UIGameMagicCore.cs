﻿using UnityEditor;
using UnityEngine;

public partial class UIGameMagicCore : UIGameCommonNormal
{
    protected BlockBaseBox blockBox;
    protected BlockBean blockData;
    protected Vector3Int blockWorldPosition;

    public override void OpenUI()
    {
        base.OpenUI();
        UIViewShortcuts.CanChangeItem = false;
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_ViewMagicCoreExchange.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        UIViewShortcuts.CanChangeItem = true;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int worldPosition, int boxSize)
    {
        this.blockWorldPosition = worldPosition;
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out Chunk chunk);
        blockBox = block as BlockBaseBox;
        //获取方块数据
        blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);
    }

}