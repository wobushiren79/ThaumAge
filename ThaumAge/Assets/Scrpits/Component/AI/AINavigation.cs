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
    public void SetMovePosition(Vector3 position)
    {
        aiAgent.isStopped = false;
        aiAgent.SetDestination(position);
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