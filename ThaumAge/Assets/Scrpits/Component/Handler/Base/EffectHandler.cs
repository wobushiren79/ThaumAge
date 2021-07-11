using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

public class EffectHandler : BaseHandler<EffectHandler, EffectManager>
{

    /// <summary>
    /// 播放粒子特效
    /// </summary>
    /// <param name="name"></param>
    /// <param name="effectPosition"></param>
    public void PlayEffect(GameObject objContainer, string name, Vector3 effectPosition, float delayTime, Action<GameObject> callBack = null)
    {
        manager.CreateEffect(objContainer, name, (objEffect) =>
        {
            if (objEffect == null)
            {
                callBack?.Invoke(null);
                return;
            }
            objEffect.transform.position = effectPosition;
            ParticleSystem[] listParticleSystem = objEffect.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < listParticleSystem.Length; i++)
            {
                ParticleSystem particleSystem = listParticleSystem[i];
                ParticleSystem.MainModule psMain = particleSystem.main;
                psMain.loop = false;
                //psMain.stopAction = ParticleSystemStopAction.Callback;
                particleSystem.Play();
            }
            if (delayTime <= 0)
            {
                callBack?.Invoke(objEffect);
                return;
            }
            StartCoroutine(CoroutineForDelayDestroy(objEffect, delayTime));
            callBack?.Invoke(objEffect);
        });
    }

    public void PlayEffect(string name, Vector3 effectPosition, float delayTime, Action<GameObject> callBack = null)
    {
        PlayEffect(null, name, effectPosition, delayTime, callBack);
    }

    public void PlayEffect(string name, Vector3 effectPosition, Action<GameObject> callBack = null)
    {
        PlayEffect(name, effectPosition, 5, callBack);
    }

    public IEnumerator CoroutineForDelayDestroy(GameObject objEffect, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(objEffect);
    }

}