using UnityEditor;
using UnityEngine;

public class CreatureHandler : BaseHandler<CreatureHandler, CreatureManager>
{

    /// <summary>
    /// 创建生物
    /// </summary>
    public void CreateCreature(long creatureId, Vector3 position)
    {
        CreatureInfoBean creatureInfo = manager.GetCreatureInfo(creatureId);
        if (creatureInfo == null)
            return;
        manager.GetCreatureModel(creatureInfo.model_name, (data) =>
        {
             GameObject objCreature = Instantiate(gameObject, data);
             objCreature.transform.position = position;
        });
    }

}