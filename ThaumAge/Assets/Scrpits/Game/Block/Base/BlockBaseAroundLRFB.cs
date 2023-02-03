using UnityEditor;
using UnityEngine;

public class BlockBaseAroundLRFB : Block
{
    /// <summary>
    ///  检测该方向是否能链接
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckCanLink(Chunk chunk, Vector3Int localPosition, DirectionEnum faceDiection)
    {
        return false;
    }
}