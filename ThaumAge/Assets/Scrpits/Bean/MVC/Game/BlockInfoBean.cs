using System;
using System.Collections.Generic;
[Serializable]
public partial class BlockInfoBean : BaseBean
{
	/// <summary>
	///颜色
	/// </summary>
	public string color;
	/// <summary>
	///mesh名字
	/// </summary>
	public string mesh_name;
	/// <summary>
	///预制名字
	/// </summary>
	public string model_name;
	/// <summary>
	///关联类名字
	/// </summary>
	public string link_class;
	/// <summary>
	///形状
	/// </summary>
	public int shape;
	/// <summary>
	///UV位置
	/// </summary>
	public string uv_position;
	/// <summary>
	///边界便宜
	/// </summary>
	public string offset_border;
	/// <summary>
	///重量
	/// </summary>
	public float weight;
	/// <summary>
	///生命值
	/// </summary>
	public int life;
	/// <summary>
	///旋转状态： 0：不能旋转(永远只能UpForward) 1：能旋转(各个方向) 2：只能LRFB方向旋转(只能是 UpLeft,UpRight,UpForward,UpBack　4个方向) 3：只能UD 方向（只能是Up,Down）
	/// </summary>
	public int rotate_state;
	/// <summary>
	///耕地状态 0不能耕地 1能耕地
	/// </summary>
	public int plough_state;
	/// <summary>
	///种植状态 0不能种植 1能种植
	/// </summary>
	public int plant_state;
	/// <summary>
	///掉落
	/// </summary>
	public string items_drop;
	/// <summary>
	///材质类型1
	/// </summary>
	public int material_type;
	/// <summary>
	///材质类型2
	/// </summary>
	public int material_type2;
	/// <summary>
	///是否碰撞 0没有碰撞 1有碰撞
	/// </summary>
	public int collider_state;
	/// <summary>
	///是否触碰 0没有触碰 1有触碰
	/// </summary>
	public int trigger_state;
	/// <summary>
	///指定破坏类型
	/// </summary>
	public string break_type;
	/// <summary>
	///指定破坏等级
	/// </summary>
	public string break_level;
	/// <summary>
	///互动状态 0不可互动 1可互动(F键)
	/// </summary>
	public int interactive_state;
	/// <summary>
	///备注int
	/// </summary>
	public int remark_int;
	/// <summary>
	///备注string
	/// </summary>
	public string remark_string;
	/// <summary>
	///方块被破坏的音效
	/// </summary>
	public string sound_break;
}
public partial class BlockInfoCfg : BaseCfg<long, BlockInfoBean>
{
	public static string fileName = "BlockInfo";
	protected static Dictionary<long, BlockInfoBean> dicData = null;
	public static Dictionary<long, BlockInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			BlockInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static BlockInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			BlockInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(BlockInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, BlockInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			BlockInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
