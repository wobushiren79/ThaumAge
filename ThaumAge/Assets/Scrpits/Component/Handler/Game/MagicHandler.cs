using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicHandler : BaseHandler<MagicHandler, MagicManager>
{
    public List<MagicTypeBase> listMagic = new List<MagicTypeBase>();

    protected float timeUpdate;
    protected float timeUpdateMax = 1;
    public void Update()
    {
        timeUpdate += Time.deltaTime;
        if (timeUpdate >= timeUpdateMax)
        {
            timeUpdate = 0;
            HandleForUpdateMagic();
        }
    }

    /// <summary>
    /// 创建魔法
    /// </summary>
    public void CreateMagic(MagicBean magicData)
    {
        MagicTypeEnum magicType = magicData.GetMagicType();
        //通过反射获取类
        MagicTypeBase magic = ReflexUtil.CreateInstance<MagicTypeBase>($"MagicType{magicType}");
        magic.SetData(magicData);

        listMagic.Add(magic);
    }

    /// <summary>
    /// 处理每秒魔法更新
    /// </summary>
    public void HandleForUpdateMagic()
    {
        if (listMagic.Count == 0)
            return;
        for (int i = 0; i < listMagic.Count; i++)
        {
            MagicTypeBase magic = listMagic[i];
            magic.UpdateMagic();
        }
    }

    /// <summary>
    /// 删除魔法
    /// </summary>
    public void DestoryMagic(MagicTypeBase magic)
    {
        listMagic.Remove(magic);
    }
}