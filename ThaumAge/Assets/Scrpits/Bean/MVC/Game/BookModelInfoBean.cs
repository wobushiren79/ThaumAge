using System;
using System.Collections.Generic;
[Serializable]
public partial class BookModelInfoBean : BaseBean
{
	/// <summary>
	///内容
	/// </summary>
	public string content;
	/// <summary>
	///背景
	/// </summary>
	public string background;
	/// <summary>
	///解锁模块详情id(用&分割)
	/// </summary>
	public string unlock_model_details_id;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class BookModelInfoCfg : BaseCfg<long, BookModelInfoBean>
{
	public static string fileName = "BookModelInfo";
	protected static Dictionary<long, BookModelInfoBean> dicData = null;
	public static Dictionary<long, BookModelInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			BookModelInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static BookModelInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			BookModelInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(BookModelInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, BookModelInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			BookModelInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
