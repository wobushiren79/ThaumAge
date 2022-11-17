using System;
using System.Collections.Generic;
[Serializable]
public partial class BookModelDetailsInfoBean : BaseBean
{
	/// <summary>
	///模块ID
	/// </summary>
	public int model_id;
	/// <summary>
	///图标（图集0UI 1Item 3Sky，名字）
	/// </summary>
	public string icon_key;
	/// <summary>
	///位置
	/// </summary>
	public string map_position;
	/// <summary>
	///展示前置
	/// </summary>
	public string show_pre;
	/// <summary>
	///展示前置线（0不展示 1展示）
	/// </summary>
	public int show_pre_line;
	/// <summary>
	///标题
	/// </summary>
	public string title_cn;
	/// <summary>
	///标题
	/// </summary>
	public string title_en;
	/// <summary>
	///内容
	/// </summary>
	public string content_cn;
	/// <summary>
	///内容
	/// </summary>
	public string content_en;
	/// <summary>
	///解锁（持有道具 | 分割：表示数量）
	/// </summary>
	public string unlock_items;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
}
public partial class BookModelDetailsInfoCfg : BaseCfg<long, BookModelDetailsInfoBean>
{
	public static string fileName = "BookModelDetailsInfo";
	protected static Dictionary<long, BookModelDetailsInfoBean> dicData = null;
	public static Dictionary<long, BookModelDetailsInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			BookModelDetailsInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static BookModelDetailsInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			BookModelDetailsInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(BookModelDetailsInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, BookModelDetailsInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			BookModelDetailsInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
