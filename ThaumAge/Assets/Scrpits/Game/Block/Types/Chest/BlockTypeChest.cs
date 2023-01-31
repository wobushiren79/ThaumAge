using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeChest : BlockBaseChest
{
    public override void AnimForChest(Vector3Int worldPosition, int state)
    {
        //获取箱子方块实例
        GameObject obj = BlockHandler.Instance.GetBlockObj(worldPosition);
        if (!obj) return;
        //设置关闭动画
        Animator blockAnim = obj.GetComponentInChildren<Animator>();
        if (!blockAnim) return;
        blockAnim.SetInteger("state", state);
    }
}