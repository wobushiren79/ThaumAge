using UnityEditor;
using UnityEngine;


public class BlockTypeComponent : BaseMonoBehaviour
{
    public Vector3Int blockWorldPosition;

    public void SetBlockWorldPosition(Vector3Int blockWorldPosition)
    {
        this.blockWorldPosition = blockWorldPosition;
    }

    public Vector3Int GetBlockWorldPosition(Vector3Int blockWorldPosition)
    {
        return blockWorldPosition;
    }

    public void GetBlock(out Block block, out Chunk chunk)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out block, out chunk);
    }

    public void GetBlock(out Block block, out BlockDirectionEnum blockDirection, out Chunk chunk)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out block, out blockDirection, out chunk);
    }
}