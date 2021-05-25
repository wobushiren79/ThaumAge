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
    public override bool CheckNeedBuildFace(DirectionEnum direction, out Block closeBlock)
    {
        closeBlock = null;
        bool hasChunk = false;
        switch (direction)
        {
            case DirectionEnum.Left:
                closeBlock = leftBlock;
                hasChunk = leftBlockHasChunk;
                break;
            case DirectionEnum.Right:
                closeBlock = rightBlock;
                hasChunk = rightBlockHasChunk;
                break;
            case DirectionEnum.UP:
                closeBlock = upBlock;
                hasChunk = upBlockHasChunk;
                break;
            case DirectionEnum.Down:
                closeBlock = downBlock;
                hasChunk = downBlockHasChunk;
                break;
            case DirectionEnum.Forward:
                closeBlock = forwardBlock;
                hasChunk = forwardBlockHasChunk;
                break;
            case DirectionEnum.Back:
                closeBlock = backBlock;
                hasChunk = backBlockHasChunk;
                break;
        }
        if (closeBlock == null)
        {
            if (hasChunk)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return false;
            case BlockShapeEnum.Liquid:
                if (closeBlock.blockType == blockType)
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
    public override void BuildBlock(Chunk.ChunkRenderData chunkData)
    {
        base.BuildBlock(chunkData);

        if (blockType != BlockTypeEnum.None)
        {
            float itemContactHeight = 1f / 4f;

            bool isBuildLeftFace = CheckNeedBuildFace(DirectionEnum.Left, out Block leftCloseBlock);
            bool isBuildRightFace = CheckNeedBuildFace(DirectionEnum.Right, out Block rightCloseBlock);
            bool isBuildForwardFace = CheckNeedBuildFace(DirectionEnum.Forward, out Block frontCloseBlock);
            bool isBuildBackFace = CheckNeedBuildFace(DirectionEnum.Back, out Block backCloseBlock);

            bool isBuildlfFace = CheckNeedBuildFace(localPosition + new Vector3Int(-1, 0, -1), out Block lfCloseBlock);
            bool isBuildlbFace = CheckNeedBuildFace(localPosition + new Vector3Int(-1, 0, 1), out Block lbCloseBlock);
            bool isBuildrfFace = CheckNeedBuildFace(localPosition + new Vector3Int(1, 0, -1), out Block rfCloseBlock);
            bool isBuildrbFace = CheckNeedBuildFace(localPosition + new Vector3Int(1, 0, 1), out Block rbCloseBlock);

            float lfSubHeight = GetAverageHeightForRange(itemContactHeight, leftCloseBlock, frontCloseBlock, lfCloseBlock);
            float lbSubHeight = GetAverageHeightForRange(itemContactHeight, leftCloseBlock, backCloseBlock, lbCloseBlock);
            float rfSubHeight = GetAverageHeightForRange(itemContactHeight, rightCloseBlock, frontCloseBlock, rfCloseBlock);
            float rbSubHeight = GetAverageHeightForRange(itemContactHeight, rightCloseBlock, backCloseBlock, rbCloseBlock);

            //Left
            if (isBuildLeftFace)
            {
                BuildFace(
                    localPosition,
                    localPosition + Vector3.up - new Vector3(0, lfSubHeight, 0),
                    localPosition + Vector3.up - new Vector3(0, lbSubHeight, 0) + Vector3.forward,
                    localPosition + Vector3.forward,
                    chunkData);
            }
            //Right
            if (isBuildRightFace)
            {
                BuildFace(
                    localPosition + Vector3Int.right,
                    localPosition + Vector3Int.right + Vector3.up - new Vector3(0, rfSubHeight, 0),
                    localPosition + Vector3Int.right + Vector3.up - new Vector3(0, rbSubHeight, 0) + Vector3.forward,
                    localPosition + Vector3Int.right + Vector3.forward,
                    chunkData);
            }
            //Front
            if (isBuildForwardFace)
            {
                BuildFace(
                    localPosition,
                    localPosition + Vector3.up - new Vector3(0, lfSubHeight, 0),
                    localPosition + Vector3.up - new Vector3(0, rfSubHeight, 0) + Vector3.right,
                    localPosition + Vector3.right,
                    chunkData);
            }
            //Back
            if (isBuildBackFace)
            {
                BuildFace(
                    localPosition + Vector3Int.forward,
                    localPosition + Vector3Int.forward + Vector3.up - new Vector3(0, lbSubHeight, 0),
                    localPosition + Vector3Int.forward + Vector3.up - new Vector3(0, rbSubHeight, 0) + Vector3.right,
                    localPosition + Vector3Int.forward + Vector3.right,
                    chunkData);
            }
            //Bottom
            if (CheckNeedBuildFace(DirectionEnum.Down))
            {
                BuildFace(
                   localPosition,
                   localPosition + Vector3.forward,
                   localPosition + Vector3.forward + Vector3.right,
                   localPosition + Vector3.right,
                   chunkData);
            }
            //Top
            if (CheckNeedBuildFace(DirectionEnum.UP))
            {
                BuildFace(
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, lfSubHeight, 0),
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, lbSubHeight, 0) + Vector3.forward,
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, rbSubHeight, 0) + Vector3.right + Vector3.forward,
                    localPosition + new Vector3Int(0, 1, 0) - new Vector3(0, rfSubHeight, 0) + Vector3.right,
                    chunkData);
            }
        }
    }

    /// <summary>
    /// 获取平均高度
    /// </summary>
    /// <param name="itemContactHeight"></param>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <param name="three"></param>
    /// <returns></returns>
    private float GetAverageHeightForRange(float itemContactHeight, Block one, Block two, Block three)
    {
        int number = 1;
        float subHeightThis = 0;
        float subHeightOne = 0;
        float subHeightTwo = 0;
        float subHeightThree = 0;
        if (contactLevel != 0)
        {
            subHeightThis = (this.contactLevel * itemContactHeight);
        }
        if (one.blockType == blockType)
        {
            subHeightOne = (one.contactLevel * itemContactHeight);
            number++;
        }
        if (two.blockType == blockType)
        {
            subHeightTwo = (two.contactLevel * itemContactHeight);
            number++;
        }
        if (three.blockType== blockType)
        {
            subHeightThree = (three.contactLevel * itemContactHeight);
            number++;
        }
        float subHeight = (subHeightOne + subHeightTwo + subHeightThree + subHeightThis) / number;
        return subHeight;
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
    public void BuildFace(Vector3 corner, Vector3 one, Vector3 two, Vector3 three, Chunk.ChunkRenderData chunkData)
    {
        AddTris(chunkData);
        AddVerts(corner, one, two, three, chunkData);
        AddUVs(chunkData);
    }

    public virtual void AddVerts(Vector3 corner, Vector3 one, Vector3 two, Vector3 three, Chunk.ChunkRenderData chunkData)
    {
        base.AddVerts(corner, chunkData);

        chunkData.verts.Add(corner);
        chunkData.verts.Add(one);
        chunkData.verts.Add(two);
        chunkData.verts.Add(three);

        chunkData.vertsTrigger.Add(corner);
        chunkData.vertsTrigger.Add(one);
        chunkData.vertsTrigger.Add(two);
        chunkData.vertsTrigger.Add(three);
    }

    public override void AddUVs(Chunk.ChunkRenderData chunkData)
    {
        base.AddUVs(chunkData);

        //List<Vector2Int> listData = blockInfo.GetUVPosition();
        //Vector2 uvStartPosition;
        //if (CheckUtil.ListIsNull(listData))
        //{
        //    uvStartPosition = Vector2.zero;
        //}
        //else if (listData.Count == 1)
        //{
        //    //只有一种面
        //    uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
        //}
        //else if (listData.Count == 3)
        //{
        //    //3种面  上 中 下
        //    switch (direction)
        //    {
        //        case DirectionEnum.UP:
        //            uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
        //            break;
        //        case DirectionEnum.Down:
        //            uvStartPosition = new Vector2(uvWidth * listData[2].y, uvWidth * listData[2].x);
        //            break;
        //        default:
        //            uvStartPosition = new Vector2(uvWidth * listData[1].y, uvWidth * listData[1].x);
        //            break;
        //    }
        //}
        //else
        //{
        //    uvStartPosition = Vector2.zero;
        //}
        //chunkData.uvs.Add(uvStartPosition);
        //chunkData.uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        //chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        //chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));

        chunkData.uvs.Add(Vector2.zero);
        chunkData.uvs.Add(Vector2.zero + new Vector2(0, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 0));
    }

    public override void AddTris(Chunk.ChunkRenderData chunkData)
    {
        base.AddTris(chunkData);

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