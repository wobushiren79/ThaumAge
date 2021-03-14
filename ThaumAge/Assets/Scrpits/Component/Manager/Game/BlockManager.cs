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