using System;
using System.Collections.Generic;
[Serializable]
public partial class MagicInstrumentInfoBean : BaseBean
{
	/// <summary>
	///道具ID
	/// </summary>
	public int item_id;
	/// <summary>
	///类型 1配件
	/// </summary>
	public int instrument_type;
	/// <summary>
	///法术核心 上限
	/// </summary>
	public int magic_core_num;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class MagicInstrumentInfoCfg : BaseCfg<long, MagicInstrumentInfoBean>
{
	public static string fileName = "MagicInstrumentInfo";
	protected static Dictionary<long, MagicInstrumentInfoBean> dicData = null;
	public static Dictionary<long, MagicInstrumentInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			MagicInstrumentInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static MagicInstrumentInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			MagicInstrumentInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(MagicInstrumentInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, MagicInstrumentInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			MagicInstrumentInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
