using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockLiquid : Block
{

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public override bool CheckNeedBuildFace(Vector3Int position, out Block closeBlock)
    {
        closeBlock = null;
        if (position.y < 0) return false;
        //检测旋转
        Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition(position, localPosition));
        //获取方块
        closeBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition);
        if (closeBlock == null)
            return false;
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return false;
            case BlockShapeEnum.Liquid:
                if (closeBlock.blockData.GetBlockType() == blockData.GetBlockType())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            default:
                return true;
        }
    }

    /// <summary>
    /// 构建方块的六个面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public override void BuildBlock(Chunk.ChunkData chunkData)
    {
        base.BuildBlock(chunkData);

        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            //int contactLevel = blockData.contactLevel;
            //float itemContactHeight = 1f / 4f;
            // float subHeight = (contactLevel * itemContactHeight);
            float subHeight = 0;
            float leftSubHeight = subHeight;
            float rightSubHeight = subHeight;
            float frontSubHeight = subHeight;
            float backSubHeight = subHeight;
            //Left
            bool isBuildLeftFace = CheckNeedBuildFace(localPosition + new Vector3Int(-1, 0, 0), out Block leftCloseBlock);
            //if (leftCloseBlock.blockData.GetBlockType() == blockData.GetBlockType() && leftCloseBlock.blockData.contactLevel > contactLevel)
            //{
            //    leftSubHeight += ((leftCloseBlock.blockData.contactLevel - blockData.contactLevel) * itemContactHeight);
            //}
            if (isBuildLeftFace)
            {  
                BuildFace(DirectionEnum.Left, blockData,
                    localPosition,
                    localPosition + Vector3.up - new Vector3(0, leftSubHeight, 0),
                    localPosition + Vector3.up - new Vector3(0, leftSubHeight, 0) + Vector3.forward,
                    localPosition + Vector3.forward,
                    chunkData);
            }
            //Right
            bool isBuildRightFace = CheckNeedBuildFace(localPosition + new Vector3Int(1, 0, 0), out Block rightCloseBlock);
            //if (rightCloseBlock.blockData.GetBlockType() == blockData.GetBlockType() && rightCloseBlock.blockData.contactLevel > contactLevel)
            //{
            //    rightSubHeight += ((rightCloseBlock.blockData.contactLevel - blockData.contactLevel) * itemContactHeight);
            //}
            if (isBuildRightFace)
            {
           
                BuildFace(DirectionEnum.Right, blockData,
                    localPosition + Vector3Int.right,
                    localPosition + Vector3Int.right + Vector3.up - new Vector3(0, rightSubHeight, 0),
                    localPosition + Vector3Int.right + Vector3.up - new Vector3(0, rightSubHeight, 0) + Vector3.forward,
                    localPosition + Vector3Int.right + Vector3.forward,
                    chunkData);
            }
            //Front
            bool isBuildFrontFace = CheckNeedBuildFace(localPosition + new Vector3Int(0, 0, -1), out Block frontCloseBlock);
            //if (frontCloseBlock.blockData.GetBlockType() == blockData.GetBlockType() && frontCloseBlock.blockData.contactLevel > contactLevel)
            //{
            //    frontSubHeight += ((frontCloseBlock.blockData.contactLevel - blockData.contactLevel) * itemContactHeight);
            //}
            if (isBuildFrontFace)
            {
        
                BuildFace(DirectionEnum.Forward, blockData,
                    localPosition,
                    localPosition + Vector3.up - new Vector3(0, frontSubHeight, 0),
                    localPosition + Vector3.up - new Vector3(0, frontSubHeight, 0) + Vector3.right,
                    localPosition + Vector3.right,
                    chunkData);
            }
            //Back
            bool isBuildBackFace = CheckNeedBuildFace(localPosition + new Vector3Int(0, 0, 1), out Block backCloseBlock);
            //if (backCloseBlock.blockData.GetBlockType() == blockData.GetBlockType() && backCloseBlock.blockData.contactLevel > contactLevel)
            //{
            //    backSubHeight += ((backCloseBlock.blockData.contactLevel - blockData.contactLevel) * itemContactHeight);
            //}
            if (isBuildBackFace)
            {    
                BuildFace(DirectionEnum.Back, blockData,
                    localPosition + Vector3Int.forward,
                    localPosition + Vector3Int.forward + Vector3.up - new Vector3(0, backSubHeight, 0),
                    localPosition + Vector3Int.forward + Vector3.up - new Vector3(0, backSubHeight, 0) + Vector3.right,
                    localPosition + Vector3Int.forward + Vector3.right,
                    chunkData);
            }
            //Bottom
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, -1, 0)))
            {
                BuildFace(DirectionEnum.Down, blockData,
                   localPosition,
                   localPosition + Vector3.forward,
                   localPosition + Vector3.forward + Vector3.right,
                   localPosition + Vector3.right,
                   chunkData);
            }
            //Top
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, 1, 0)))
            {
                float subOneHeight = subHeight;
                float subTwoHeight = subHeight;
                float subThreeHeight = subHeight;
                float subFourHeight = subHeight;
                //if (leftSubHeight> subHeight || frontSubHeight > subHeight)
                //{
                //    subOneHeight = leftSubHeight > frontSubHeight ? leftSubHeight : frontSubHeight;
                //}
                //if (leftSubHeight > subHeight || backSubHeight > subHeight)
                //{
                //    subTwoHeight = leftSubHeight > backSubHeight ? leftSubHeight : backSubHeight;
                //}
                //if (backSubHeight > subHeight || rightSubHeight > subHeight)
                //{
                //    subThreeHeight = backSubHeight > rightSubHeight ? backSubHeight : rightSubHeight;
                //}
                //if (rightSubHeight > subHeight || frontSubHeight > subHeight)
                //{
                //    subFourHeight = rightSubHeight > frontSubHeight ? rightSubHeight : frontSubHeight;
                //}
                BuildFace(DirectionEnum.UP, blockData,
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, subOneHeight, 0),
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, subTwoHeight, 0) + Vector3.forward,
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, subThreeHeight, 0) + Vector3.right + Vector3.forward,
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, subFourHeight, 0) + Vector3.right,
                    chunkData);
            }
        }
    }

    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="blockData"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    /// <param name="vertsCollider"></param>
    /// <param name="trisCollider"></param>
    public void BuildFace(DirectionEnum direction, BlockBean blockData, Vector3 corner, Vector3 one, Vector3 two, Vector3 three, Chunk.ChunkData chunkData)
    {
        AddTris(chunkData);
        AddVerts(corner, one, two, three, chunkData);
        AddUVs(direction, blockData, chunkData);
    }

    public virtual void AddVerts(Vector3 corner, Vector3 one, Vector3 two, Vector3 three, Chunk.ChunkData chunkData)
    {
        chunkData.verts.Add(corner);
        chunkData.verts.Add(one);
        chunkData.verts.Add(two);
        chunkData.verts.Add(three);

        chunkData.vertsTrigger.Add(corner);
        chunkData.vertsTrigger.Add(one);
        chunkData.vertsTrigger.Add(two);
        chunkData.vertsTrigger.Add(three);
    }

    public void AddUVs(DirectionEnum direction, BlockBean blockData, Chunk.ChunkData chunkData)
    {
        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockData.GetBlockType());
        List<Vector2Int> listData = blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (CheckUtil.ListIsNull(listData))
        {
            uvStartPosition = Vector2.zero;
        }
        else if (listData.Count == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
        }
        else if (listData.Count == 3)
        {
            //3种面  上 中 下
            switch (direction)
            {
                case DirectionEnum.UP:
                    uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
                    break;
                case DirectionEnum.Down:
                    uvStartPosition = new Vector2(uvWidth * listData[2].y, uvWidth * listData[2].x);
                    break;
                default:
                    uvStartPosition = new Vector2(uvWidth * listData[1].y, uvWidth * listData[1].x);
                    break;
            }
        }
        else
        {
            uvStartPosition = Vector2.zero;
        }
        //chunkData.uvs.Add(uvStartPosition);
        //chunkData.uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        //chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        //chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));

        chunkData.uvs.Add(Vector2.zero);
        chunkData.uvs.Add(Vector2.zero + new Vector2(0, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 0));
    }

    public override void AddTris(Chunk.ChunkData chunkData)
    {
        int index = chunkData.verts.Count;
        int triggerIndex = chunkData.vertsTrigger.Count;

        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 1);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 2);

        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 2);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 3);

        chunkData.trisTrigger.Add(triggerIndex + 0);
        chunkData.trisTrigger.Add(triggerIndex + 1);
        chunkData.trisTrigger.Add(triggerIndex + 2);

        chunkData.trisTrigger.Add(triggerIndex + 0);
        chunkData.trisTrigger.Add(triggerIndex + 2);
        chunkData.trisTrigger.Add(triggerIndex + 3);
    }
}