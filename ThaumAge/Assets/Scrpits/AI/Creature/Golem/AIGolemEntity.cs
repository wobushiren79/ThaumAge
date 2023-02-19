using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGolemEntity : AICreateEntity
{
    protected override void InitIntentEnum(List<AIIntentEnum> listIntentEnum)
    {
        //���ݲ�ͬ�ĺ��� ��Ӳ�ͬ����ͼ
        listIntentEnum.Add(AIIntentEnum.GolemIdle);
    }

    public override void SetData(CreatureCptBase creatureCpt)
    {
        base.SetData(creatureCpt);
        //Ĭ������
        ChangeIntent(AIIntentEnum.GolemIdle);
    }
}
