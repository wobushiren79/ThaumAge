using System;
	[Serializable]
	public partial class AudioInfoBean : BaseBean
	{
		/// <summary>
		///内容
		/// </summary>
		public string name;
		/// <summary>
		///备注
		/// </summary>
		public string remark;
		/// <summary>
		///类型0音乐 1音效 2环境音
		/// </summary>
		public int audio_type;
	}
