using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation
{
    protected AIBaseEntity aiEntity;
    protected NavMeshAgent aiAgent;
    public AINavigation(AIBaseEntity aiEntity)
    {
        this.aiEntity = aiEntity;
        aiAgent = aiEntity.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 随机获取附近范围内可以行走的点
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public bool GetRandomRangeMovePosition(Vector3 currentPosition, float range, out Vector3 targetPosition)
    {
        if (NavMesh.SamplePosition(currentPosition, out NavMeshHit navigationHit, range, aiAgent.areaMask))
        {
            targetPosition = navigationHit.position;
            return true;
        }
        else
        {
            targetPosition = Vector3.zero;
            return false;
        }
    }


    /// <summary>
    /// 设置移动
    /// </summary>
    /// <param name="position"></param>
    /// <param name="isUseRangePosition">是否使用附近的可移动点</param>
    /// <param name="rangePositionDis">附近可移动点的范围</param>
    /// <returns></returns>
    public bool SetMovePosition(Vector3 position, bool isUseRangePosition = false, float rangePositionDis = 1)
    {
        //代理是否在寻路面上
        if (!aiAgent.isOnNavMesh)
        {
            aiAgent.enabled = false;
            aiAgent.enabled = true;
        }
        //如果依旧不在寻路面上则设置路径失败
        if (!aiAgent.isOnNavMesh)
        {
            LogUtil.LogError("设置路径失败，代理不在寻路面上");
            return false;
        }
        aiAgent.isStopped = false;
        //检测目标点是否能到达
        bool canMove = aiAgent.SetDestination(position);
        if (canMove)
        {
            //如果目标点能到达
            return true;
        }
        else
        {         
            //如果目标点不能到达 
            //判断是否使用附近可走的点
            if (isUseRangePosition)
            {
                //先搜索附近1m范围能到达的地点
                bool isTargetPositionIn = GetRandomRangeMovePosition(position, rangePositionDis, out Vector3 targetPosition);
                if (isTargetPositionIn)
                {
                    //使用附近的点
                    aiAgent.SetDestination(targetPosition);
                    return true;
                }
                else
                {
                    //如果附近没有点 则设置失败
                    LogUtil.LogError("设置路径失败，目标点不在寻路面上");
                    return false;
                }
            }
            else
            {                    
                //如果附近没有点 则设置失败
                LogUtil.LogError("设置路径失败，目标点不在寻路面上");
                return false;
            } 
        }
    }

    /// <summary>
    /// 设置移动速度
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        aiAgent.speed = speed;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        //aiAgent.speed = 0;
        aiAgent.isStopped = true;
        //aiAgent.destination = aiAgent.transform.position;
    }

    /// <summary>
    /// 是否在移动
    /// </summary>
    /// <returns></returns>
    public bool IsMove()
    {
        if (!aiAgent.pathPending)
        {
            if (aiAgent.remainingDistance <= aiAgent.stoppingDistance)
            {
                if (!aiAgent.hasPath || aiAgent.velocity.sqrMagnitude == 0f)
                {
                    //移动停止
                    return false;
                }
            }
        }
        return true;
    }
}