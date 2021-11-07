using UnityEditor;
using UnityEngine;

public class EffectBlockBreak : EffectBase
{
    /// <summary>
    /// 设置粒子颜色
    /// </summary>
    public void SetEffectColor()
    {
        if (!listPS.IsNull())
        {
            for (int i = 0; i < listPS.Count; i++)
            {
                ParticleSystem itemPS = listPS[i];
                ParticleSystem.MainModule settings = itemPS.main;
                Gradient gradient = new Gradient();
                gradient.colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(Color.red,0f),
                    new GradientColorKey(Color.blue,0.3f),
                    new GradientColorKey(Color.green,0.6f)
                };
                settings.startColor = new ParticleSystem.MinMaxGradient(gradient);
            }
        }
    }
}