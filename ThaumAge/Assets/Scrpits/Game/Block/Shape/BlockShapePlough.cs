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

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector2[] uvsAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData != null && face == DirectionEnum.UP)
        {
            BlockBasePlough blockPlough = (BlockBasePlough)block;
            int rotate = (int)direction % 10;
            if (rotate == 3|| rotate == 4)
            {
                AddUVs(chunk.chunkMeshData.uvs, blockPlough.uvsAddUpRotate);
                return;
            }
        }
        base.BaseAddUVs(chunk, localPosition, direction, face, uvsAdd);
    }

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="closeDirection"></param>
    /// <returns></returns>
    public override bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk);
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            if (closeBlockChunk)
            {
                //只是空气方块
                return true;
            }
            else
            {
                //还没有生成chunk
                return false;
            }
        }
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.CubeCuboid:
            case BlockShapeEnum.Plough:
                return false;
            default:
                return true;
        }
    }
}