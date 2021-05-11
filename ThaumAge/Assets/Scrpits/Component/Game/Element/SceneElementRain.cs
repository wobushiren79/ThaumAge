using System.Collections;
using UnityEngine;

public class SceneElementRain : SceneElementBase
{
    public ParticleSystem psRain;

    private void Awake()
    {
        psRain = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        HandleForPosition();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="rateOverTime">雨的规模</param>
    public void SetData(int rateOverTime)
    {
        ParticleSystem.EmissionModule emission = psRain.emission;
        //设置雨的规模
        emission.rateOverTime = rateOverTime;     
        psRain.Play();
    }
}