using UnityEditor;
using UnityEngine;

public class BlockTypeFurnacesSimple : Block
{
    /// <summary>
    /// 打开熔炉
    /// </summary>
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        base.Interactive(user, worldPosition, direction);
    }
}