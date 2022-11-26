using System;
using System.Collections.Generic;
[Serializable]
public partial class ResearchInfoBean : BaseBean
{
	/// <summary>
	///类型 1.法术核心镶嵌
	/// </summary>
	public int type_research;
	/// <summary>
	///具体类型
	/// </summary>
	public int type_details;
	/// <summary>
	///图标
	/// </summary>
	public string icon_key;
	/// <summary>
	///花费时间
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
	///解锁资源（&分隔不同材料） （:分隔道具数量） （|分隔可替换的材料）
	/// </summary>
	public string unlock_materials;
	/// <summary>
	///资源（&分隔不同材料） （:分隔道具数量） （|分隔可替换的材料）
	/// </summary>
	public string materials;
	/// <summary>
	///研究数据
	/// </summary>
	public string data_research;
	/// <summary>
	///内容描述中文
	/// </summary>
	public string content_cn;
	/// <summary>
	///内容描述英文
	/// </summary>
	public string content_en;
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
