using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class BlockTypeCrucible : Block
{

    /// <summary>
    /// 对着坩埚使用
    /// </summary>
    /// <param name="targetChunk"></param>
    /// <param name="targetWorldPosition"></param>
    public override void TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        base.TargetUseBlock(user, itemData, targetChunk, targetWorldPosition);
        int water = Random.Range(0, 6);
        SetWater(targetWorldPosition, water, true);
    }

    /// <summary>
    /// 设置水面
    /// </summary>
    /// <param name="level">0没有水 5满水</param>
    /// <param name="isBoiling">是否沸腾</param>
    public void SetWater(Vector3Int blockWorldPosition, int level, bool isBoiling)
    {
        GameObject objBlock = GetBlockObj(blockWorldPosition);
        if (objBlock == null)
        {
            return;
        }

        Transform tfWaterPlane = objBlock.transform.Find("WaterPlane");
        Transform tfBoiling = objBlock.transform.Find("WaterPlane/Effect_WaterBoiling_1");
        float waterPlaneY = 0;
        switch (level)
        {
            case 0:
                tfWaterPlane.ShowObj(false);
                tfBoiling.ShowObj(false);
                tfWaterPlane.DOKill();
                return;
            case 1:
                waterPlaneY = 0.05f;
                break;
            case 2:
                waterPlaneY = 0.1f;
                break;
            case 3:
                waterPlaneY = 0.2f;
                break;
            case 4:
                waterPlaneY = 0.3f;
                break;
            case 5:
                waterPlaneY = 0.4f;
                break;
        }
        tfWaterPlane.DOLocalMoveY(waterPlaneY, 1);
        tfWaterPlane.ShowObj(true);
        if (isBoiling)
        {
            tfBoiling.ShowObj(true);
        }
        else
        {
            tfBoiling.ShowObj(false);
        }
    }
}