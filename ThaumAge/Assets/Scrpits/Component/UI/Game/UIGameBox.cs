using UnityEditor;
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
        ui_ViewBoxList.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();
        ui_ViewBoxList.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI();
        ui_ViewBackPack.RefreshUI();
        ui_ViewBoxList.RefreshUI();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int worldPosition,int boxSize)
    {
        this.blockWorldPosition = worldPosition;
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out Chunk chunk);
        blockBox = block as BlockBaseBox;
        //获取方块数据
        blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);
        //设置数据
        ui_ViewBoxList.SetData(worldPosition, blockData, boxSize);
    }

    /// <summary>
    /// 退出箱子
    /// </summary>
    public override void HandleForBackGameMain()
    {
        base.HandleForBackGameMain();
        //关闭箱子
        blockBox.CloseBox(blockWorldPosition);
    }


}