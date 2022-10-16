using UnityEditor;
using UnityEngine;

public class BlockTypeGrinderSimple : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        base.Interactive(user, worldPosition, direction);

    }
}