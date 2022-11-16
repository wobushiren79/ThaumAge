using System;
using System.Collections.Generic;
[Serializable]
public partial class CreatureInfoBean : BaseBean
{
	/// <summary>
	///模型资源名字
	/// </summary>
	public string model_name;
	/// <summary>
	///备注
	/// </summary>
	public string remark;
	/// <summary>
	///生物类型
	/// </summary>
	public int creature_type;
	/// <summary>
	///基础生命值
	/// </summary>
	public int life;
	/// <summary>
	///掉落
	/// </summary>
	public string drop;
	/// <summary>
	///资源产出（牛奶 羽毛之类的）
	/// </summary>
	public string output_res;
	/// <summary>
	///攻击范围（近战）
	/// </summary>
	public string range_damage;
	/// <summary>
	///视野距离
	/// </summary>
	public float sight_range;
	/// <summary>
	///移动速度
	/// </summary>
	public float speed_move;
	/// <summary>
	///目标丢失距离
	/// </summary>
	public float dis_loss;
	/// <summary>
	///近战攻击距离
	/// </summary>
	public float dis_attack_melee;
	/// <summary>
	///远程攻击距离
	/// </summary>
	public float dis_attack_remote;
	/// <summary>
	///近战攻击间隔
	/// </summary>
	public float time_interval_attack_melee;
	/// <summary>
	///远程攻击间隔
	/// </summary>
	public float time_interval_attack_remote;
	/// <summary>
	///伤害数据
	/// </summary>
	public string damage_data;
}
public partial class CreatureInfoCfg : BaseCfg<long, CreatureInfoBean>
{
	public static string fileName = "CreatureInfo";
	protected static Dictionary<long, CreatureInfoBean> dicData = null;
	public static Dictionary<long, CreatureInfoBean> GetAllData()
	{
		if (dicData == null)
		{
			CreatureInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return dicData;
	}
	public static CreatureInfoBean GetItemData(long key)
	{
		if (dicData == null)
		{
			CreatureInfoBean[] arrayData = GetInitData(fileName);
			InitData(arrayData);
		}
		return GetItemData(key, dicData);
	}
	public static void InitData(CreatureInfoBean[] arrayData)
	{
		dicData = new Dictionary<long, CreatureInfoBean>();
		for (int i = 0; i < arrayData.Length; i++)
		{
			CreatureInfoBean itemData = arrayData[i];
			dicData.Add(itemData.id, itemData);
		}
	}
}
