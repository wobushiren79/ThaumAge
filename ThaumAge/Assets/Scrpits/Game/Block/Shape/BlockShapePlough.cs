using UnityEditor;
using UnityEngine;

public class BlockShapePlough : BlockShapeCubeCuboid
{
    public override void InitData(Block block)
    {
        base.InitData(block);

        //获取耕地不同的UV
        BlockBasePlough blockPlough = (BlockBasePlough)block;
        Vector2 uvStart = GetUVStartPosition(block,DirectionEnum.UP);
        blockPlough.uvsAddUpRotate = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y),
            new Vector2(uvStart.x,uvStart.y),
        };
    }

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum face, Vector2[] uvsAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData != null && face == DirectionEnum.UP)
        {
            BlockBasePlough blockPlough = (BlockBasePlough)block;
            BlockBasePlough.FromMetaData(blockData.meta, out int rotate);
            if (rotate == 1)
            {
                AddUVs(chunk.chunkMeshData.uvs, blockPlough.uvsAddUpRotate);
                return;
            }
        }
        base.BaseAddUVs(chunk, localPosition, direction, face, uvsAdd);
    }

}