using System;
using System.Collections.Generic;
[Serializable]
public partial class BiomeInfoBean : BaseBean
{
	/// <summary>
	///出现频率 数值越大 波峰越多
	/// </summary>
	public float frequency;
	/// <summary>
	///振幅 数值越大 越宽 （0-1）
	/// </summary>
	public float amplitude;
	/// <summary>
	///间隙性
	/// </summary>
	public float lacunarity;
	/// <summary>
	///噪音循环迭代次数 复杂度
	/// </summary>
	public int octaves;
	/// <summary>
	///洞穴最小高度
	/// </summary>
	public int caveMinHeight;
	/// <summary>
	///洞穴最大高度
	/// </summary>
	public int caveMaxHeight;
	/// <summary>
	///洞穴大小
	/// </summary>
	public float caveScale;
	/// <summary>
	///洞穴的阈值（0-1）
	/// </summary>
	public float caveThreshold;
	/// <summary>
	///洞穴出现频率 数值越大 波峰越多
	/// </summary>
	public float caveFrequency;
	/// <summary>
	///洞穴振幅 数值越大 越宽
	/// </summary>
	public float caveAmplitude;
	/// <summary>
	///洞穴循环迭代次数 复杂度
	/// </summary>
	public int caveOctaves;
	/// <summary>
	///地面的最低高度
	/// </summary>
	public int groundMinHeigh;
	/// <summary>
	///海水最低高度
	/// </summary>
	public int oceanMinHeight;
	/// <summary>
	///海水最高高度
	/// </summary>
	public int oceanMaxHeight;
	/// <summary>
	///水的大小
	/// </summary>
	public float oceanScale;
	/// <summary>
	///水的阈值
	/// </summary>
	public float oceanThreshold;
	/// <summary>
	///水的振幅
	/// </summary>
	public float oceanAmplitude;
	/// <summary>
	///水的频率
	/// </summary>
	public float oceanFrequency;
	/// <summary>
	///矿石数据(id_density_minH_maxH&id_density_minH_max)density在0-1 越接近1 矿石越稀少
	/// </summary>
	public string oreData;
	/// <summary>
	///生物信息id_createRange_rangeMaxNum
	/// </summary>
	public string creatureData;
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
