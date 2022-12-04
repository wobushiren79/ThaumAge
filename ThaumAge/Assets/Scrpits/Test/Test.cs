
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using System.Collections.Generic;
using DG.Tweening;
public class Test : BaseMonoBehaviour
{
    public MagicBean magicData;
    private void OnGUI()
    {
        if (GUILayout.Button("Fire"))
        {
            magicData.createPosition = new Vector3(UnityEngine.Random.Range(-10,10), magicData.createPosition.y, UnityEngine.Random.Range(-10, 10));
            MagicHandler.Instance.CreateMagic(magicData);
        }
    }


}
