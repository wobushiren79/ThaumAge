using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockHandler : BaseHandler<BlockHandler, BlockManager>
{
    /// <summary>
    /// 构建方块
    /// </summary>
    /// <param name="chunk">所属chunk</param>
    /// <param name="position">所属chunk的内的位置</param>
    /// <param name="blockData">方块数据</param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public void BuildBlock(Chunk chunk, Vector3Int position, BlockBean blockData, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        BlockInfoBean blockInfo = manager.GetBlockInfo(blockData.GetBlockType());
        BlockShapeEnum blockShape = blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.None:
                break;
            case BlockShapeEnum.Cube:
                manager.BuildBlockForCube(chunk, position, blockData, verts, uvs, tris);
                break;
        }
    }

}