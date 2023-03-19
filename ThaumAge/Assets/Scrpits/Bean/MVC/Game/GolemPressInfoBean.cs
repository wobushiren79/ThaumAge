using System;
using System.Collections.Generic;
[Serializable]
public partial class GolemPressInfoBean : BaseBean
{
	/// <summary>
	///傀儡部件名字（1材料 2头 3手 3脚 4附件）
	/// </summary>
	public int golem_part_type;
	/// <summary>
	///贴图资源
	/// </summary>
	public string tex;
	/// <summary>
	///材料（&分隔不同材料） （:分隔道具数量） （|分隔可替换的材料）
	/// </summary>
	public string materials;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class GolemPressInfoCfg : BaseCfg<long, GolemPressInfoBean>
{
	public static string fileName = "GolemPressInfo";
	protected static Dictionary<long, GolemPressInfoBean> dicData = null;
	public static Dictionary<long, GolemPressInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			GolemPressInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static GolemPressInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			GolemPressInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(GolemPressInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, GolemPressInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			GolemPressInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
