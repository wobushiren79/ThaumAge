using System;
using System.Collections.Generic;
[Serializable]
public partial class ItemsSynthesisBean : BaseBean
{
	/// <summary>
	///魔力消耗
	/// </summary>
	public int magic_pay;
	/// <summary>
	///合成结果:分隔道具数量
	/// </summary>
	public string results;
	/// <summary>
	///资源（&分隔不同材料） （:分隔道具数量） （|分隔可替换的材料）
	/// </summary>
	public string materials;
	/// <summary>
	///消耗元素（& 分割不同元素 ：分割道具数量）
	/// </summary>
	public string elemental;
	/// <summary>
	///合成类型(用|分割) 0自带 1自身 2基础制造台 11神秘制造台 21坩埚
	/// </summary>
	public string type_synthesis;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class ItemsSynthesisCfg : BaseCfg<long, ItemsSynthesisBean>
{
	public static string fileName = "ItemsSynthesis";
	protected static Dictionary<long, ItemsSynthesisBean> dicData = null;
	public static Dictionary<long, ItemsSynthesisBean> GetAllData()
	{
		if (dicData == null)
		{
			ItemsSynthesisBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static ItemsSynthesisBean GetItemData(long key)
	{
		if (dicData == null)
		{
			ItemsSynthesisBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(ItemsSynthesisBean[] arrayData)
	{
		dicData = new Dictionary<long, ItemsSynthesisBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			ItemsSynthesisBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
