using System;
	[Serializable]
	public partial class BookModelDetailsInfoBean : BaseBean
	{
		/// <summary>
		///模块ID
		/// </summary>
		public long model_id;
		/// <summary>
		///图标（图集，名字）
		/// </summary>
		public string icon_key;
		/// <summary>
		///位置
		/// </summary>
		public string map_position;
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
		///解锁拥有道具
		/// </summary>
		public string unlock_items;
		/// <summary>
		///备注
		/// </summary>
		public string remark;
	}
