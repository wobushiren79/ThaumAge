using System;
	[Serializable]
	public partial class ItemsSynthesisBean : BaseBean
	{
		/// <summary>
		///资源id和数量 用|分割
		/// </summary>
		public string items;
		/// <summary>
		///合成类型(用,分割) 0自身  1基础合成台
		/// </summary>
		public string type_synthesis;
		/// <summary>
		///备注
		/// </summary>
		public string remark;
	}
