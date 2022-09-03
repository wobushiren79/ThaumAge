using UnityEditor;
using UnityEngine;

public class BlockBaseBox : Block
{
    protected int boxSize = 7 * 7;
    public override void Interactive(GameObject user, Vector3Int worldPosition,BlockDirectionEnum blockDirection)
    {
        base.Interactive(user, worldPosition, blockDirection);
        //只有player才能打开
        if (user == null || user.GetComponent<Player>() == null)
            return;
        //打开箱子UI
        UIGameBox uiGameBox = UIHandler.Instance.OpenUIAndCloseOther<UIGameBox>(UIEnum.GameBox);
        uiGameBox.SetData(worldPosition, boxSize);
        //打开盒子
        OpenBox(worldPosition);
    }

    public virtual void OpenBox(Vector3Int worldPosition)
    {
        AnimForBox(worldPosition, 1);
    }

    public virtual void CloseBox(Vector3Int worldPosition)
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