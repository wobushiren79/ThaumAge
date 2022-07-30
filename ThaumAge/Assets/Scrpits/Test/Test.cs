
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
    public int seed;
    private void OnGUI()
    {

        if (GUILayout.Button("Test3"))
        {
            
            for (int x=-4;x<4;x++)
            {
                for (int z = -4; z < 4; z++)
                {
                    Vector3 position= GetHexagonBiomeWorldPos(x, z, 1, (float)Mathf.Sqrt(3) * 0.5f);
                    GameObject itemObj= GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    itemObj.transform.position = position;
                }
            }
        }
    }


    //获取六边形地形位置
    Vector3 GetHexagonBiomeWorldPos(int x, int y, float sideLength, float sideHeight)
    {
        float wx = x * (sideLength * 1.5f);
        float wz =( Mathf.Abs (x % 2)) * sideHeight + y * sideHeight * 2;
        return new Vector3(wx, 0, wz);
    }
}
