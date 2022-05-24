using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameSign : UIGameCommonNormal, SelectColorView.ICallBack
{
    protected Vector3Int blockWorldPosition;

    public override void Awake()
    {
        base.Awake();
        ui_ViewSelectColorChange.SetCallBack(this);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockWorldPosition"></param>
    public void SetData(Vector3Int blockWorldPosition)
    {
        this.blockWorldPosition = blockWorldPosition;

        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (block != null && chunk != null)
        {
            Vector3Int localPosition = blockWorldPosition - chunk.chunkData.positionForWorld;
            BlockBean blockData = chunk.GetBlockData(localPosition);
            if (blockData != null)
            {
                BlockMetaSign blockMetaSignData = Block.FromMetaData<BlockMetaSign>(blockData.meta);
                if (blockMetaSignData != null)
                {
                    ui_ViewSelectColorChange.SetData(blockMetaSignData.texColor.r, blockMetaSignData.texColor.g, blockMetaSignData.texColor.b);
                    ui_TextInput.text = blockMetaSignData.texContent;
                    return;
                }
            }
        }
        ui_ViewSelectColorChange.SetData(1, 1, 1);
        ui_TextInput.text = "";
    }

    public override void CloseUI()
    {
        base.CloseUI();
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        if (block == null || chunk == null)
            return;
        if (block is BlockBaseSign blockSign)
        {
            string texContent = ui_TextContent.text;
            Color texColor = ui_ViewSelectColorChange.GetColor();
            blockSign.SetData(chunk, blockWorldPosition - chunk.chunkData.positionForWorld, texContent, texColor);
        }
    }

    /// <summary>
    /// 颜色选择回调
    /// </summary>
    public void SelectColorChange(SelectColorView colorView, float r, float g, float b)
    {
        if (colorView == ui_ViewSelectColorChange)
        {
            ui_TextContent.color = new Color(r, g, b, 1);
        }
    }
}