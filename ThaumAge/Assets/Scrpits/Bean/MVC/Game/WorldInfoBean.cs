using System;
using System.Collections.Generic;
[Serializable]
public partial class WorldInfoBean : BaseBean
{
	/// <summary>
	///生态内容
	/// </summary>
	public string biome_content;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class WorldInfoCfg : BaseCfg<long, WorldInfoBean>
{
	public static string fileName = "WorldInfo";
	protected static Dictionary<long, WorldInfoBean> dicData = null;
	public static Dictionary<long, WorldInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			WorldInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static WorldInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			WorldInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(WorldInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, WorldInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			WorldInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
