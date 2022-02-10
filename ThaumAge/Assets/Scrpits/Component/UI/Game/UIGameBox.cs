using UnityEditor;
using UnityEngine;
public partial class UIGameBox : UIGameCommonNormal
{
    protected BlockBaseBox blockBox;
    protected Vector3Int blockWorldPosition;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int worldPosition)
    {
        this.blockWorldPosition = worldPosition;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out Chunk chunk);
        blockBox = block as BlockBaseBox;
    }

    public override void HandleForBackMain()
    {
        base.HandleForBackMain();
        blockBox.CloseBox(blockWorldPosition);
    }

}