using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "资源脚本/GameInit")]
public class SOGameInitBean : ScriptableObject
{
    [Header("周边区块加载卸载间隔距离")]
    public float disForWorldUpdate = 16f;

    [Header("道具删除的最远距离")]
    public float disForItemsDestory = 50;

    [Header("周边区块加载卸载删除范围")]
    public int rangeForWorldUpdateDestory = 10;

    [Header("创建角色 浏览 旋转速度")]
    public float speedForCreateCharacterRotate = 100;
}