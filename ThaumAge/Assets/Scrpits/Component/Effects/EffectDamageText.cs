using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class EffectDamageText : EffectBase
{
    protected float timeForStart;
    protected float timeForEnd;

    protected TextMeshPro damageText;

    public void Awake()
    {
        timeForStart = 0.2f;
        timeForEnd = 0.5f;

        damageText = GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="damageText"></param>
    public void SetData(string textContent)
    {
        damageText.text = textContent;

        AnimForInit();
        AnimForStart();
        AnimForShow();
    }

    //动画相关数据
    protected Tween animAlphaStart;
    protected Tween animScaleStart;

    protected Tween animAlphaEnd;
    protected Tween animScaleEnd;

    protected Tween animMoveShow;

    /// <summary>
    /// 动画初始化
    /// </summary>
    public void AnimForInit()
    {
        StopAllCoroutines();
        animAlphaStart?.Kill();
        animScaleStart?.Kill();
        animAlphaEnd?.Kill();
        animScaleEnd?.Kill();
        animMoveShow?.Kill();
        //还原数值
        damageText.color = damageText.color.SetColor(a: 1);
        transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// 展示动画
    /// </summary>
    public void AnimForShow()
    {
        //向上移动动画
        animMoveShow = transform
            .DOLocalMoveY(transform.localPosition.y + 2f, effectData.timeForShow)
            .SetEase(Ease.Linear);
        //开始结束动画
        this.WaitExecuteSeconds(effectData.timeForShow - timeForEnd, () =>
        {
            AnimForEnd();
        });
    }

    /// <summary>
    /// 开始动画
    /// </summary>
    public void AnimForStart()
    {
        //缩放动画
        animScaleStart = transform
            .DOScale(1, timeForStart)
            .SetEase(Ease.OutBack);
        //显示动画
        //animAlphaStart = DOTween
        //    .ToAlpha(() => { return damageText.color; }, (data) =>
        //    {
        //        damageText.color = data;
        //    }, 1, timeForStart);
    }

    /// <summary>
    /// 隐藏动画
    /// </summary>
    public void AnimForEnd()
    {
        //缩放动画
        animScaleEnd = transform
            .DOScale(1.2f, timeForEnd)
            .SetEase(Ease.Linear);
        //隐藏动画
        animAlphaEnd = DOTween
            .ToAlpha(() => { return damageText.color; }, (data) =>
            {
                damageText.color = data;
            }, 0, timeForEnd);
    }


}