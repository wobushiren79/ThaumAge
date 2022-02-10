using UnityEditor;
using UnityEngine;

public class BlockBaseBox : Block
{
    public override void Interactive(Vector3Int worldPosition)
    {
        base.Interactive(worldPosition);
        //打开箱子UI
        UIGameBox uiGameBox = UIHandler.Instance.OpenUIAndCloseOther<UIGameBox>(UIEnum.GameBox);
        uiGameBox.SetData(worldPosition);
        //打开盒子
        OpenBox(worldPosition);
    }

    public void OpenBox(Vector3Int worldPosition)
    {
        AnimForBox(worldPosition, 1);
    }

    public void CloseBox(Vector3Int worldPosition)
    {
        AnimForBox(worldPosition, 0);
    }

    public void AnimForBox(Vector3Int worldPosition, int state)
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