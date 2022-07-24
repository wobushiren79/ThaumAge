
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
    public Vector3 worldPos;
    public GameObject objContainer;
    protected float _edgeLen = 10;
    protected float _halfHeight = (Mathf.Sqrt(3) * 0.5f) * 10;
    public Vector3 Origin { get; set; } = Vector3.zero;

    public void Start()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Test2"))
        {
            objContainer.transform.DestroyAllChild();
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Vector3 vector3 = GetWorldPos(x, y);
                    GameObject objItem = GameObject.CreatePrimitive( PrimitiveType.Capsule);
                    objItem.transform.SetParent(objContainer.transform);
                    objItem.transform.position = vector3;
                }
            }
        }

        if (GUILayout.Button("Test3"))
        {
            Vector2Int pos = MathUtil.GetHexagonIndex(worldPos.x,worldPos.y, Origin, _edgeLen, _halfHeight);
            UnityEngine.Debug.Log("GetGridPos:"+ pos);
        }
    }


    public Vector3 GetWorldPos(int x, int y)
    {
        float wx = x * _edgeLen * 1.5f;
        float wz = (y * 2 + x % 2) * _halfHeight;
        return Origin + new Vector3(wx, 0, wz);
    }
}
