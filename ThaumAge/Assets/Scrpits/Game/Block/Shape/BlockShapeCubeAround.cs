using UnityEditor;
using UnityEngine;

public class BlockShapeCubeAround : BlockShapeCube
{

    public BlockShapeCubeAround(Block block) : base(block)
    {

    }

    /// <summary>
    /// 检测是否生成面
    /// </summary>
    /// <returns></returns>
    protected override bool CheckNeedBuildFaceDef(Block closeBlock, Chunk closeBlockChunk, Vector3Int closeLocalPosition, DirectionEnum closeDirection)
    {
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.CubeTransparent:
            case BlockShapeEnum.CubeAround:
                return false;
            default:
                return true;
        }
    }

    /// <summary>
    /// 获取周围方块类型  
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <returns>
    /// 0000周围无方块  
    /// 左右上下
    /// 1111 四周都有
    /// </returns>
    public int GetAroundBlockType(Chunk chunk, Vector3Int localPosition,
        DirectionEnum leftDirection, DirectionEnum rightDirection, DirectionEnum upDirection, DirectionEnum downDirection)
    {
        int blockType = 0;
        //获取上下左右4个方向的方块
        block.GetCloseBlockByDirection(chunk, localPosition, leftDirection, out Block leftBlock, out Chunk leftChunk, out Vector3Int leftBlockLocalPosition);
        block.GetCloseBlockByDirection(chunk, localPosition, rightDirection, out Block rightBlock, out Chunk rightChunk, out Vector3Int rightBlockLocalPosition);
        block.GetCloseBlockByDirection(chunk, localPosition, upDirection, out Block upBlock, out Chunk upChunk, out Vector3Int upBlockLocalPosition);
        block.GetCloseBlockByDirection(chunk, localPosition, downDirection, out Block downBlock, out Chunk downChunk, out Vector3Int downBlockLocalPosition);

        if (leftChunk != null && leftBlock != null && leftBlock.blockType == block.blockType)
        {
            blockType += 1000;
        }
        if (rightChunk != null && rightBlock != null && rightBlock.blockType == block.blockType)
        {
            blockType += 100;
        }
        if (upChunk != null && upBlock != null && upBlock.blockType == block.blockType)
        {
            blockType += 10;
        }
        if (downChunk != null && downBlock != null && downBlock.blockType == block.blockType)
        {
            blockType += 1;
        }
        return blockType;
    }

    public override void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        Vector2[] uvsAddNew;
        int blockAroundType;
        Vector2 uvStart;
        switch (face) 
        {
            case DirectionEnum.Left:
                blockAroundType = GetAroundBlockType(chunk, localPosition,DirectionEnum.Back,DirectionEnum.Forward,DirectionEnum.UP,DirectionEnum.Down);
                uvStart = GetUVStartPosition(block, blockAroundType);
                uvsAddNew = new Vector2[]
                {
                    new Vector2(uvStart.x ,uvStart.y + uvWidth),
                    new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
                    new Vector2(uvStart.x + uvWidth ,uvStart.y),
                    new Vector2(uvStart.x ,uvStart.y)
                };
                break;
            case DirectionEnum.Right:
                blockAroundType = GetAroundBlockType(chunk, localPosition, DirectionEnum.Forward, DirectionEnum.Back, DirectionEnum.UP, DirectionEnum.Down);
                uvStart = GetUVStartPosition(block, blockAroundType);
                uvsAddNew = new Vector2[]
                {
                    new Vector2(uvStart.x,uvStart.y),
                    new Vector2(uvStart.x,uvStart.y + uvWidth),
                    new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
                    new Vector2(uvStart.x+ uvWidth,uvStart.y)
                };
                break;
            case DirectionEnum.UP:
                blockAroundType = GetAroundBlockType(chunk, localPosition, DirectionEnum.Left, DirectionEnum.Right, DirectionEnum.Back, DirectionEnum.Forward);
                uvStart = GetUVStartPosition(block, blockAroundType);
                uvsAddNew = new Vector2[]
                {
                    new Vector2(uvStart.x,uvStart.y),
                    new Vector2(uvStart.x,uvStart.y + uvWidth),
                    new Vector2(uvStart.x + uvWidth,uvStart.y+ uvWidth),
                    new Vector2(uvStart.x + uvWidth,uvStart.y)
                };
                break;
            case DirectionEnum.Down:
                blockAroundType = GetAroundBlockType(chunk, localPosition, DirectionEnum.Left, DirectionEnum.Right, DirectionEnum.Forward, DirectionEnum.Back);
                uvStart = GetUVStartPosition(block, blockAroundType);
                uvsAddNew = new Vector2[]
                {
                    new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
                    new Vector2(uvStart.x,uvStart.y + uvWidth),
                    new Vector2(uvStart.x,uvStart.y),
                    new Vector2(uvStart.x+ uvWidth,uvStart.y)
                };
                break;
            case DirectionEnum.Forward:
                blockAroundType = GetAroundBlockType(chunk, localPosition, DirectionEnum.Left, DirectionEnum.Right, DirectionEnum.UP, DirectionEnum.Down);
                uvStart = GetUVStartPosition(block, blockAroundType);
                uvsAddNew = new Vector2[]
                {
                    new Vector2(uvStart.x,uvStart.y),
                    new Vector2(uvStart.x,uvStart.y + uvWidth),
                    new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
                    new Vector2(uvStart.x+ uvWidth,uvStart.y)
                };
                break;
            case DirectionEnum.Back:
                blockAroundType = GetAroundBlockType(chunk, localPosition, DirectionEnum.Right, DirectionEnum.Left, DirectionEnum.UP, DirectionEnum.Down);
                uvStart = GetUVStartPosition(block, blockAroundType);
                uvsAddNew = new Vector2[]
                {
                    new Vector2(uvStart.x ,uvStart.y + uvWidth),
                    new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
                    new Vector2(uvStart.x + uvWidth ,uvStart.y),
                    new Vector2(uvStart.x ,uvStart.y)
                };
                break;
            default:
                uvsAddNew = new Vector2[0];
                break;
        }
        base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAddNew, colorsAdd);
    }

    /// <summary>
    /// 获取起始UV
    /// </summary>
    /// <param name="buildDirection"></param>
    /// <returns></returns>
    public virtual Vector2 GetUVStartPosition(Block block,int aroundBlockType)
    {
        Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
        int index = 0;
        switch (aroundBlockType)
        {
            case 1111:
                index = 0;
                break;

            case 0111:
                index = 1;
                break;
            case 1011:
                index = 2;
                break;
            case 1101:
                index = 3;
                break;
            case 1110:
                index = 4;
                break;

            case 0011:
                index = 5;
                break;
            case 1100:
                index = 6;
                break;

            case 0101:
                index = 7;
                break;

            case 1001:
                index = 8;
                break;
            case 1010:
                index = 9;
                break;
            case 0110:
                index = 10;
                break;



            case 1000:
                index = 11;
                break;
            case 0100:
                index = 12;
                break;
            case 0010:
                index = 13;
                break;
            case 0001:
                index = 14;
                break;

            case 0:
                index = 15;
                break;
        }
        Vector2 uvStartPosition = new Vector2(uvWidth * arrayUVData[index].y, uvWidth * arrayUVData[index].x);
        return uvStartPosition;
    }
}