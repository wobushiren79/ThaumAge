using System;
	[Serializable]
	public partial class BookModelInfoBean : BaseBean
	{
		/// <summary>
		///内容
		/// </summary>
		public string content;
		/// <summary>
		///背景
		/// </summary>
		public string background;
		/// <summary>
		///解锁模块详情id(用&分割)
		/// </summary>
		public string unlock_model_details_id;
		/// <summary>
		///备注
		/// </summary>
		public string remark;
	}
