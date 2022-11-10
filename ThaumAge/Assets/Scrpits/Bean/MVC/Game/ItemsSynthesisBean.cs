using System;
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
		///合成类型(用|分割) 0自身  1基础制造台 11神秘制造台 21坩埚
		/// </summary>
		public string type_synthesis;
		/// <summary>
		///备注
		/// </summary>
		public string remark;
	}
