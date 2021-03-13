using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockManager : BaseManager, IBlockInfoView
{
    protected BlockInfoController controllerForBlock;

    protected Dictionary<BlockTypeEnum, BlockInfoBean> dicBlockInfo = new Dictionary<BlockTypeEnum, BlockInfoBean>();

    public virtual void Awake()
    {
        controllerForBlock = new BlockInfoController(this, this);
        controllerForBlock.GetAllBlockInfoData(InitBlockInfo);
    }

    /// <summary>
    /// 初始化方块信息
    /// </summary>
    /// <param name="listData"></param>
    public void InitBlockInfo(List<BlockInfoBean> listData)
    {
        dicBlockInfo.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            BlockInfoBean itemInfo = listData[i];
            dicBlockInfo.Add(itemInfo.GetBlockType(), itemInfo);
        }
    }

    /// <summary>
    /// 获取方块信息
    /// </summary>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public BlockInfoBean GetBlockInfo(BlockTypeEnum blockType)
    {
        if (dicBlockInfo.TryGetValue(blockType, out BlockInfoBean blockInfo))
        {
            return blockInfo;
        }
        return null;
    }
    public BlockInfoBean GetBlockInfo(long blockId)
    {
       return GetBlockInfo((BlockTypeEnum)blockId);
    }

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckNeedBuildFace(Chunk chunk, Vector3Int position)
    {
        if (position.y < 0) return false;
        BlockTypeEnum type = chunk.GetBlockType(position);
        switch (type)
        {
            case BlockTypeEnum.None:
                return true;
            default:
                return false;
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
    public void BuildBlockForCube(Chunk chunk, Vector3Int position, BlockBean blockData, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(chunk, position + new Vector3Int(-1, 0, 0)))
                BuildFaceForCube(blockType, position, Vector3.up, Vector3.forward, false, verts, uvs, tris);
            //Right
            if (CheckNeedBuildFace(chunk, position + new Vector3Int(1, 0, 0)))
                BuildFaceForCube(blockType, position + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, verts, uvs, tris);

            //Bottom
            if (CheckNeedBuildFace(chunk, position + new Vector3Int(0, -1, 0)))
                BuildFaceForCube(blockType, position, Vector3.forward, Vector3.right, false, verts, uvs, tris);
            //Top
            if (CheckNeedBuildFace(chunk, position + new Vector3Int(0, 1, 0)))
                BuildFaceForCube(blockType, position + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, verts, uvs, tris);

            //Front
            if (CheckNeedBuildFace(chunk, position + new Vector3Int(0, 0, -1)))
                BuildFaceForCube(blockType, position, Vector3.up, Vector3.right, true, verts, uvs, tris);
            //Back
            if (CheckNeedBuildFace(chunk, position + new Vector3Int(0, 0, 1)))
                BuildFaceForCube(blockType, position + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
        }
    }

    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    void BuildFaceForCube(BlockTypeEnum blockType, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        Vector2 uvWidth = new Vector2(0.25f, 0.25f);
        Vector2 uvCorner = new Vector2(0.00f, 0.75f);

        uvCorner.x += (float)(blockType - 1) / 4;
        uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));

        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
        }
        else
        {
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 3);
            tris.Add(index + 2);
        }
    }



    #region 方块数据回调
    public void GetBlockInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }


    public void GetBlockInfoFail(string failMsg, Action action)
    {
        LogUtil.Log("获取方块数据失败");
    }
    #endregion
}