using UnityEditor;
using UnityEngine;

public class CreatureCptChickenSmall : CreatureCptBaseAnimal
{
    protected CreatureMetaChicken creatureMetaChicken;
    public override void SetData(CreatureInfoBean creatureInfo)
    {
        base.SetData(creatureInfo);
        creatureMetaChicken = new CreatureMetaChicken();
        creatureMetaChicken.proGrow = 0;
        creatureMetaChicken.proEgg = 0;
    }

    /// <summary>
    /// 处理 更新数据
    /// </summary>
    public override void HandleForUpdateData()
    {
        base.HandleForUpdateData();
        creatureMetaChicken.proGrow += 0.05f;
        if (creatureMetaChicken.proGrow >= 1)
        {
            creatureMetaChicken.proGrow = 0;
            //产出长大成鸡
            CreatureHandler.Instance.CreateCreature(creatureInfo.id - 1, transform.position);
            //删除小鸡
            Destroy(gameObject);
        }
    }
}