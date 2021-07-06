using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "资源脚本/GameInit")]
public class SOGameInitBean : ScriptableObject
{
    [Header("周边区块加载卸载间隔距离")]
    public float disForWorldUpdate = 16f;

    [Header("周边区块加载卸载删除范围")]
    public int rangeForWorldUpdateDestory = 10;
}