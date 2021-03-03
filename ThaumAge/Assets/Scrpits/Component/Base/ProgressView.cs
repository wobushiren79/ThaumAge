using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using TMPro;

public class ProgressView : BaseMonoBehaviour
{

    public enum ProgressType
    {
        Percentage,//百分比
        Degree//进度
    }

    public ProgressType progressType;
    public TextMeshProUGUI tvContent;
    public Slider sliderPro;
    public string completeContent;


    protected ICallBack callBack;

    private void Start()
    {
        sliderPro.onValueChanged.AddListener(OnSliderValueChange);
    }

    public void SetData(string data, float value)
    {
        SetContent(data);
        SetSlider(value);
    }

    public void SetData(float value)
    {
        SetContent((Math.Round(value, 4) * 100) + "%");
        SetSlider(value);
    }



    public void SetData(float maxData, float data)
    {
        float pro = 0;
        if (maxData == 0)
        {
            pro = 0;
        }
        else
        {
            pro = data / maxData;
        }
        switch (progressType)
        {
            case ProgressType.Percentage:
                SetContent((Math.Round(pro, 4) * 100) + "%");
                break;
            case ProgressType.Degree:
                SetContent(data + "/" + maxData);
                break;
        }
        SetSlider(pro);
    }


    public void SetCompleteContent(string content)
    {
        completeContent = content;
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 设置文字显示
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
        {
            if (sliderPro.value == 1 && !CheckUtil.StringIsNull(completeContent))
            {
                tvContent.text = completeContent;
            }
            else
            {
                tvContent.text = content;
            }
        }
    }
    public void SetContent(string content, Color color)
    {
        SetContent(content);
        SetContentColor(color);
    }

    public void SetContentColor(Color color)
    {
        if (tvContent != null)
        {
            tvContent.color = color;
        }
    }



    /// <summary>
    /// 设置进度条
    /// </summary>
    /// <param name="pro"></param>
    public void SetSlider(float pro)
    {
        if (sliderPro != null)
            sliderPro.value = pro;
    }


    public void OnSliderValueChange(float value)
    {
        //是否可互动，如果是可互动的 则按百分比显示
        if (sliderPro.IsInteractable())
        {
            SetContent((Math.Round(value, 4) * 100) + "%");
        }
        if (callBack != null)
        {
            callBack.OnProgressViewValueChange(this, value);
        }
    }

    public interface ICallBack
    {
        void OnProgressViewValueChange(ProgressView progressView, float value);
    }
}