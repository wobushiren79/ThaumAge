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
    /// 设置移动
    /// </summary>
    public bool SetMovePosition(Vector3 position)
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
        //检测目标点是否再寻路面以内
        bool isTargetPositionIn = NavMesh.SamplePosition(position, out NavMeshHit navigationHit, 5, aiAgent.areaMask);

        aiAgent.isStopped = false;
        if (isTargetPositionIn)
        {        
            //如果目标点在寻路面上
            aiAgent.SetDestination(position);
            return true;
        }
        else
        {   
            if (navigationHit.hit)
            {
                //如果目标点不在寻路面上 则使用附近的点
                aiAgent.SetDestination(navigationHit.position);
                return true;
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