/*
* FileName: BlockInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-11-15:39:10 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BlockInfoService : BaseMVCService
{
    public BlockInfoService() : base("block_info", "block_info_details_"+ GameDataHandler.Instance.manager.GetGameConfig().language)
    {

    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BlockInfoBean> QueryAllData()
    {
        List<BlockInfoBean> listData = BaseQueryAllData<BlockInfoBean>();
        return listData; 
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public BlockInfoBean QueryData()
    {
        return null; 
    }

    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BlockInfoBean> QueryDataById(long id)
    {
        return BaseQueryData<BlockInfoBean>("link_id", "id", id + "");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateData(BlockInfoBean data)
    {
        bool deleteState = BaseDeleteDataById(data.id);
        if (deleteState)
        {
            bool insertSuccess = BaseInsertData(tableNameForMain, data);
            return insertSuccess;
        }
        return false;
    }
}