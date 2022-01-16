using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "资源脚本/GameInit")]
public class SOGameInitBean : ScriptableObject
{
    [Header("道具删除的最远距离")]
    public float disForItemsDestory = 50;

    [Header("道具不可拾取的距离")]
    public float disForDropNoPick = 3;

    [Header("道具删除的最长时间")]
    public float timeForItemsDestory = 100;

    [Header("周边区块加载卸载删除范围")]
    public int rangeForWorldUpdateDestory = 10;

    [Header("创建角色 浏览 旋转速度")]
    public float speedForCreateCharacterRotate = 100;
}