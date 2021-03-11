/*
* FileName: BlockInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-11-15:39:10 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BlockInfoModel : BaseMVCModel
{
    protected BlockInfoService serviceBlockInfo;

    public override void InitData()
    {
        serviceBlockInfo = new BlockInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<BlockInfoBean> GetAllBlockInfoData()
    {
        List<BlockInfoBean> listData = serviceBlockInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public BlockInfoBean GetBlockInfoData()
    {
        BlockInfoBean data = serviceBlockInfo.QueryData();
        if (data == null)
            data = new BlockInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BlockInfoBean> GetBlockInfoDataById(long id)
    {
        List<BlockInfoBean> listData = serviceBlockInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetBlockInfoData(BlockInfoBean data)
    {
        serviceBlockInfo.UpdateData(data);
    }

}