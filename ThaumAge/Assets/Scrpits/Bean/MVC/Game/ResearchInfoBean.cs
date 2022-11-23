using System;
using System.Collections.Generic;
[Serializable]
public partial class ResearchInfoBean : BaseBean
{
	/// <summary>
	///类型 1镶嵌（元素）2镶嵌（生成）3镶嵌（射程）4镶嵌（范围）5镶嵌（威力）6镶嵌（消耗）
	/// </summary>
	public int type_research;
	/// <summary>
	///图标
	/// </summary>
	public string icon_key;
	/// <summary>
	///花费事件
	/// </summary>
	public int time;
	/// <summary>
	///解锁前置研究
	/// </summary>
	public string unlock_pre_research;
	/// <summary>
	///是否需要解锁 0不需要 1需要
	/// </summary>
	public int need_unlock;
	/// <summary>
	///资源（&分隔不同材料） （:分隔道具数量） （|分隔可替换的材料）
	/// </summary>
	public string materials;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class ResearchInfoCfg : BaseCfg<long, ResearchInfoBean>
{
	public static string fileName = "ResearchInfo";
	protected static Dictionary<long, ResearchInfoBean> dicData = null;
	public static Dictionary<long, ResearchInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			ResearchInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static ResearchInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			ResearchInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(ResearchInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, ResearchInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			ResearchInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
