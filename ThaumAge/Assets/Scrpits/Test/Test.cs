
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
    public Transform tfTest;
    public Vector3 punch;
    public float punchTime;
    public int vibrato;//震动次
    public float elascity;//
    public void Start()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            float timeCount = 0;
            DOTween.To(
                () => { return timeCount; },
                (data)=> { timeCount = data; },
                1,10).OnUpdate(()=> {
                    tfTest.transform.position += (punch * Time.deltaTime);
                });
            //tfTest.DOPunchPosition(punch, punchTime, vibrato, elascity);
        }

        if (GUILayout.Button("Test2"))
        {
            tfTest.transform.position -= new Vector3(0,0.1f,0);
        }
    }



}
