using System;
using System.Collections.Generic;
[Serializable]
public partial class BiomeInfoBean : BaseBean
{
	/// <summary>
	///出现频率 数值越大 波峰越多
	/// </summary>
	public int frequency;
}
public partial class BiomeInfoCfg : BaseCfg<long, BiomeInfoBean>
{
	public static string fileName = "BiomeInfo";
	protected static Dictionary<long, BiomeInfoBean> dicData = null;
	public static Dictionary<long, BiomeInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			BiomeInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static BiomeInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			BiomeInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(BiomeInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, BiomeInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			BiomeInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
