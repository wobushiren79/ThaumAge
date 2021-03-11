/*
* FileName: BlockInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-11-15:39:10 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockInfoController : BaseMVCController<BlockInfoModel, IBlockInfoView>
{

    public BlockInfoController(BaseMonoBehaviour content, IBlockInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public BlockInfoBean GetBlockInfoData(Action<BlockInfoBean> action)
    {
        BlockInfoBean data = GetModel().GetBlockInfoData();
        if (data == null) {
            GetView().GetBlockInfoFail("没有数据",null);
            return null;
        }
        GetView().GetBlockInfoSuccess<BlockInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllBlockInfoData(Action<List<BlockInfoBean>> action)
    {
        List<BlockInfoBean> listData = GetModel().GetAllBlockInfoData();
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetBlockInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBlockInfoSuccess<List<BlockInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetBlockInfoDataById(long id,Action<BlockInfoBean> action)
    {
        List<BlockInfoBean> listData = GetModel().GetBlockInfoDataById(id);
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetBlockInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBlockInfoSuccess(listData[0], action);
        }
    }
} 