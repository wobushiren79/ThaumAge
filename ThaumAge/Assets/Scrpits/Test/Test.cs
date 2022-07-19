
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
    public GameObject objContainer;
    public float _edgeLen = 1;
    protected float _halfHeight = Mathf.Sqrt(3) * 0.5f;
    public Vector3 Origin { get; set; } = Vector3.zero;

    public void Start()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Test"))
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
    }


    public Vector3 GetWorldPos(int x, int y)
    {
        float wx = x * _edgeLen * 1.5f;
        float wz = (y * 2 + x % 2) * _halfHeight;
        return Origin + new Vector3(wx, 0, wz);
    }

    public Vector2Int GetGridPos(float worldX, float worldY)
    {
        var tx = (worldX - Origin.x) / (_edgeLen * 1.5f);
        int cx = Mathf.RoundToInt(tx);
        var ty = ((worldY - Origin.z) / _halfHeight - cx % 2) / 2f;
        int cy = Mathf.RoundToInt(ty);
        return new Vector2Int(cx, cy);

    }
}
