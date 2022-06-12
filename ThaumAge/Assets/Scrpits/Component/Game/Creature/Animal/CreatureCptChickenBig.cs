using UnityEditor;
using UnityEngine;

public class CreatureCptChickenBig : CreatureCptBaseAnimal
{

    protected CreatureMetaChicken creatureMetaChicken;
    public override void SetData(CreatureInfoBean creatureInfo)
    {
        base.SetData(creatureInfo);
        creatureMetaChicken = new CreatureMetaChicken();
        creatureMetaChicken.proGrow = 1;
        creatureMetaChicken.proEgg = 0;
    }

    /// <summary>
    /// 处理 更新数据
    /// </summary>
    public override void HandleForUpdateData()
    {
        base.HandleForUpdateData();
        creatureMetaChicken.proEgg += 0.02f;
        if (creatureMetaChicken.proEgg >= 1)
        {
            creatureMetaChicken.proEgg = 0;
            //产出鸡蛋
            CreateOutputItems();
        }
    }
}