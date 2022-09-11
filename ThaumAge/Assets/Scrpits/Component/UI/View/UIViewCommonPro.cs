using UnityEditor;
using UnityEngine;

public partial class UIViewCommonPro : BaseUIView
{

    /// <summary>
    /// 设计数据
    /// </summary>
    public void SetData(int currentData, int maxData)
    {
        SetProText(currentData, maxData);
        SetPro((float)currentData / maxData);
    }

    /// <summary>
    /// 设置进度文本
    /// </summary>
    public void SetProText(int currentData, int maxData)
    {
        ui_ProText.text = $"{currentData}/{maxData}";
    }

    /// <summary>
    /// 设置进度
    /// </summary>
    public void SetPro(float pro)
    {
        ui_Pro.value = pro;
    }

}