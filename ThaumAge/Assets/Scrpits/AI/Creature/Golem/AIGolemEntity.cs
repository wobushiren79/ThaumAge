using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGolemEntity : AICreateEntity
{
    //������ͼ
    public Queue<AIIntentEnum> queueWorkIntent = new Queue<AIIntentEnum>();

    protected override void InitIntentEnum(List<AIIntentEnum> listIntentEnum)
    {
        queueWorkIntent.Clear();

        //���ݲ�ͬ�ĺ��� ��Ӳ�ͬ����ͼ
        listIntentEnum.Add(AIIntentEnum.GolemIdle);
        listIntentEnum.Add(AIIntentEnum.GolemStandby);

        CreatureCptBaseGolem golemCreature = creatureCpt as CreatureCptBaseGolem;
        if (golemCreature.golemMetaData == null)
            return;
        var listGolemCore = golemCreature.golemMetaData.listGolemCore;
        for (int i = 0; i < listGolemCore.Count; i++)
        {
            var itemGolemCore = listGolemCore[i];
            if (itemGolemCore.itemId == 0)
                continue;
            switch (itemGolemCore.itemId)
            {
                case 4400002:
                    listIntentEnum.Add(AIIntentEnum.GolemPick);
                    queueWorkIntent.Enqueue(AIIntentEnum.GolemPick);
                    break;
                case 4400003:
                    listIntentEnum.Add(AIIntentEnum.GolemPut);
                    queueWorkIntent.Enqueue(AIIntentEnum.GolemPut);
                    break;
                case 4400004:
                    listIntentEnum.Add(AIIntentEnum.GolemTake);
                    queueWorkIntent.Enqueue(AIIntentEnum.GolemTake);
                    break;
            }
        }
    }

    public override void SetData(CreatureCptBase creatureCpt)
    {
        base.SetData(creatureCpt);
        //Ĭ������
        ChangeIntent(AIIntentEnum.GolemIdle);
    }
}
