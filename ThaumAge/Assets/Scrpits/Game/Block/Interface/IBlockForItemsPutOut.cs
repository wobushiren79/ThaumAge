using UnityEditor;
using UnityEngine;

public interface IBlockForItemsPutOut
{
    /// <summary>
    /// 道具放入
    /// </summary>
    public void ItemsPut(Chunk chunk, Vector3Int localPosition,ItemsBean putItem);

    /// <summary>
    /// 检测是否能完全放下道具
    /// </summary>
    public bool ItemsPutCheck(Chunk chunk, Vector3Int localPosition, ItemsBean putItem);

    /// <summary>
    /// 道具取出
    /// </summary>
    public ItemsBean ItemsOut(Chunk chunk, Vector3Int localPosition);
}