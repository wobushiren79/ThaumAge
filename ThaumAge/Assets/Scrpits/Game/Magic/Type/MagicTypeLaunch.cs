using UnityEditor;
using UnityEngine;


public class MagicTypeLaunch : MagicTypeBase
{
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="magicData"></param>
    public override void SetData(MagicBean magicData)
    {
        base.SetData(magicData);
        ElementalTypeEnum elementalType = magicData.GetElementalType();
        //创建火球
        MagicHandler.Instance.manager.GetMagicCpt(magicData, $"Magic_Launch{elementalType}Ball_1.prefab", CallBackForCreateMagicCpt);
    }

    /// <summary>
    /// 回调 创建魔法预制成功后
    /// </summary>
    /// <param name="objMagic"></param>
    public virtual void CallBackForCreateMagicCpt(GameObject objMagic)
    {
        //获取魔法预制
        MagicCpt magicCpt = objMagic.GetComponent<MagicCpt>();
        //设置碰撞回调
        magicCpt.actionForTriggerEnter += CallBackForMagicTriggerEnter;
        //向指定方向发射
        magicCpt.rbMaigc.AddForce(magicData.direction * magicData.magicSpeed, ForceMode.VelocityChange);
        //添加到列表
        listMagicObj.Add(magicCpt.gameObject);
        //播放发射音效
        AudioHandler.Instance.PlaySound(52, magicData.createPosition);
    }
}