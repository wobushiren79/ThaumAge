using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "资源脚本/GameInit")]
public class SOGameInitBean : ScriptableObject
{
    [Header("周边区块加载卸载间隔")]
    public float timeForWorldUpdate = 0.2f;

}