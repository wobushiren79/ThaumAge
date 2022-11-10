using System;
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
