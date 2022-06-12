using System;
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
		///基础生命值
		/// </summary>
		public int life;
		/// <summary>
		///掉落
		/// </summary>
		public string drop;
		/// <summary>
		///产出
		/// </summary>
		public string output;
	}
