using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EffectBase : BaseMonoBehaviour
{
    public List<ParticleSystem> listPS = new List<ParticleSystem>();

    [HideInInspector]
    public EffectBean effectData;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="effectData"></param>
    public virtual void SetData(EffectBean effectData)
    {
        if (effectData == null)
            return;
        this.effectData = effectData;
        transform.position = effectData.effectPosition;
    }

    /// <summary>
    /// 播放粒子
    /// </summary>
    public virtual void PlayEffect()
    {
        if (!listPS.IsNull())
        {
            for (int i = 0; i < listPS.Count; i++)
            {
                ParticleSystem itemPS = listPS[i];
                itemPS.Play();
            }
        }
    }
}