using System;
	[Serializable]
	public partial class ElementalInfoBean : BaseBean
	{
		/// <summary>
		///图标名字
		/// </summary>
		public string icon_key;
		/// <summary>
		///元素等级
		/// </summary>
		public int level;
		/// <summary>
		///颜色
		/// </summary>
		public string color;
		/// <summary>
		///元素组成
		/// </summary>
		public string elemental_constitute;
		/// <summary>
		///备注
		/// </summary>
		public string remark;
	}
