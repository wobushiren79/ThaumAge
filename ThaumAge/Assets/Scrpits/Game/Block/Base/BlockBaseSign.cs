using TMPro;
using UnityEditor;
using UnityEngine;

public class BlockBaseSign : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        base.Interactive(user, worldPosition, direction);
        //打开UI
        UIGameSign uiGameSign = UIHandler.Instance.OpenUIAndCloseOther<UIGameSign>(UIEnum.GameSign);
        uiGameSign.SetData(worldPosition);
    }

    /// <summary>
    /// 刷新方块
    /// </summary>
    public override void RefreshObjModel(Chunk chunk, Vector3Int localPosition)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData == null)
            return;

        BlockMetaSign blockMetaSignData = FromMetaData<BlockMetaSign>(blockData.meta);
        GameObject objSign = chunk.GetBlockObjForLocal(localPosition);
        Transform tfText = objSign.transform.Find("Model/Text");
        if (blockMetaSignData == null || blockMetaSignData.texContent.IsNull())
        {
            tfText.ShowObj(false);
        }
        else
        {
            tfText.ShowObj(true);
            TextMeshPro textMesh = tfText.GetComponent<TextMeshPro>();
            textMesh.text = blockMetaSignData.texContent;
            textMesh.color = blockMetaSignData.texColor.GetColor();
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(Chunk chunk, Vector3Int localPosition, string texContent, Color texColor)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData == null)
            return;
        BlockMetaSign blockMetaSignData = FromMetaData<BlockMetaSign>(blockData.meta);
        if (blockMetaSignData == null)
            blockMetaSignData = new BlockMetaSign();
        //设置数据
        blockMetaSignData.texContent = texContent;
        blockMetaSignData.texColor = TypeConversionUtil.ColorToColorBean(texColor);
        //保存数据
        blockData.meta = ToMetaData(blockMetaSignData);
        chunk.isSaveData = true;
        //刷新牌子
        RefreshObjModel(chunk, localPosition);
    }
}