using UnityEditor;
using UnityEngine;

public class EffectBlockBreak : EffectBase
{
    /// <summary>
    /// 设置粒子颜色
    /// </summary>
    public void SetEffectColor(Color start, Color end)
    {
        if (!listPS.IsNull())
        {
            for (int i = 0; i < listPS.Count; i++)
            {
                ParticleSystem itemPS = listPS[i];
                ParticleSystem.MainModule settings = itemPS.main;
                //Gradient gradient = new Gradient();
                //gradient.colorKeys = new GradientColorKey[]
                //{
                //    new GradientColorKey(start,0f),
                //    new GradientColorKey(end,1f)
                //};
                settings.startColor = new ParticleSystem.MinMaxGradient(start, end);
            }
        }
    }
}