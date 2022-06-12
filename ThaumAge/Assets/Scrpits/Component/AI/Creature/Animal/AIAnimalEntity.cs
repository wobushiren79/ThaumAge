using UnityEditor;
using UnityEngine;

public class AIAnimalEntity : AIBaseEntity<AIAnimalIntentEnum>
{
    public AINavigation aiNavigation;
    public CreatureCptBase creatureCpt;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="creatureCpt"></param>
    public void SetData(CreatureCptBase creatureCpt)
    {
        this.creatureCpt = creatureCpt;
        //初始化寻路
        aiNavigation = new AINavigation(this);
        //默认闲置
        ChangeIntent(AIAnimalIntentEnum.Idle);
        //下一帧 初始化寻一次路，到当前点，防止模型下陷
        this.WaitExecuteEndOfFrame(1, () => 
        {
            aiNavigation.SetMovePosition(transform.position, true, 5);
        });
    }

}