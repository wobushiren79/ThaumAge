using System;
using System.Collections.Generic;
[Serializable]
public partial class ElementalInfoBean : BaseBean
{
	/// <summary>
	///图标名字
	/// </summary>
	public string icon_key;
	/// <summary>
	///元素等级
	/// </summary>
	public int level;
	/// <summary>
	///颜色
	/// </summary>
	public string color;
	/// <summary>
	///元素组成
	/// </summary>
	public string elemental_constitute;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class ElementalInfoCfg : BaseCfg<long, ElementalInfoBean>
{
	public static string fileName = "ElementalInfo";
	protected static Dictionary<long, ElementalInfoBean> dicData = null;
	public static Dictionary<long, ElementalInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			ElementalInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static ElementalInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			ElementalInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(ElementalInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, ElementalInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			ElementalInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
