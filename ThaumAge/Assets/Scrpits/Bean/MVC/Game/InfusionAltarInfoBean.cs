using System;
using System.Collections.Generic;
[Serializable]
public partial class InfusionAltarInfoBean : BaseBean
{
	/// <summary>
	///需要注魔的物品（:分隔道具数量） 
	/// </summary>
	public string item_before;
	/// <summary>
	///注魔之后的物品（:分隔道具数量） 
	/// </summary>
	public string item_after;
	/// <summary>
	///材料（&分隔不同材料） （:分隔道具数量） （|分隔可替换的材料）
	/// </summary>
	public string materials;
	/// <summary>
	///元素（&分隔不同材料） （:分隔道具数量） （|分隔可替换的材料）
	/// </summary>
	public string elements;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class InfusionAltarInfoCfg : BaseCfg<long, InfusionAltarInfoBean>
{
	public static string fileName = "InfusionAltarInfo";
	protected static Dictionary<long, InfusionAltarInfoBean> dicData = null;
	public static Dictionary<long, InfusionAltarInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			InfusionAltarInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static InfusionAltarInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			InfusionAltarInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(InfusionAltarInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, InfusionAltarInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			InfusionAltarInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
