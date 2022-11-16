using System;
using System.Collections.Generic;
[Serializable]
public partial class Sheet1Bean : BaseBean
{
	/// <summary>
	///名字
	/// </summary>
	public string name;
}
public partial class Sheet1Cfg : BaseCfg<long, Sheet1Bean>
{
	public static string fileName = "Sheet1";
	protected static Dictionary<long, Sheet1Bean> dicData = null;
	public static Dictionary<long, Sheet1Bean> GetAllData()
	{
		if (dicData == null)
		{
			Sheet1Bean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static Sheet1Bean GetItemData(long key)
	{
		if (dicData == null)
		{
			Sheet1Bean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(Sheet1Bean[] arrayData)
	{
		dicData = new Dictionary<long, Sheet1Bean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			Sheet1Bean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
