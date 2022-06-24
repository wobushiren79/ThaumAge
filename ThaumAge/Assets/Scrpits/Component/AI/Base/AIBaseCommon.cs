using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AIBaseCommon
{

    /// <summary>
    /// 视线搜索-圆形范围
    /// </summary>
    /// <param name="sourcePosition">眼睛位置</param>
    /// <param name="searchRadius">搜索半径</param>
    /// <param name="layerSearchTarget">搜索物体的层级</param>
    /// <param name="layerObstacles">遮挡物体层级</param>
    public static List<Collider> SightSearchCircle(Vector3 sourcePosition, float searchRadius, int layerSearchTarget, int layerObstacles)
    {
        List<Collider> listData = new List<Collider>();
        //搜索圆形范围内的目标物体
        Collider[] searchTarget = RayUtil.RayToSphere(sourcePosition, searchRadius, layerSearchTarget);
        if (searchTarget.IsNull())
            return listData;

        for (int i = 0; i < searchTarget.Length; i++)
        {
            Collider itemTargetCollider = searchTarget[i];
            //获取目标距离
            float disTarget = Vector3.Distance(sourcePosition, itemTargetCollider.transform.position);
            //发射一条射线 检测是否有视野遮挡
            bool hasObstacles = RayUtil.RayToCast(sourcePosition, itemTargetCollider.transform.position, disTarget, layerObstacles);
            if (!hasObstacles)
            {
                listData.Add(itemTargetCollider);
            }
        }
        return listData;
    }
}