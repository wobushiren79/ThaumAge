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

    /// <summary>
    /// 处理和方块的碰撞
    /// </summary>
    /// <param name="magicCpt"></param>
    /// <param name="collider"></param>
    public override void HandleForTriggerBlock(MagicCpt magicCpt, Collider collider)
    {
        ElementalTypeEnum elementalType = magicData.GetElementalType();
        //获取碰撞点
        Vector3 closePosition = collider.bounds.ClosestPoint(magicCpt.transform.position);
        switch (elementalType)  
        {
            case ElementalTypeEnum.Metal:
                //波坏地形
                WorldCreateHandler.Instance.SetBlockRange(closePosition, range: 2, setShape: 1);
                //播放爆炸音效
                AudioHandler.Instance.PlaySound(151, closePosition);
                break;
            case ElementalTypeEnum.Wood:

                break;
            case ElementalTypeEnum.Water:
                //在空气方块里生成水
                WorldCreateHandler.Instance.SetBlockRange(closePosition,blockType: BlockTypeEnum.Water, range: 2, setShape: 1,isOnlySetAir:true);
                break;
            case ElementalTypeEnum.Fire:

                break;
            case ElementalTypeEnum.Earth:
                //在空气方块里生成土
                WorldCreateHandler.Instance.SetBlockRange(closePosition, blockType: BlockTypeEnum.Dirt, range: 2, setShape: 1, isOnlySetAir: true);
                break;
        }
    }


    /// <summary>
    /// 处理和生物的碰撞
    /// </summary>
    /// <param name="magicCpt"></param>
    /// <param name="collider"></param>
    public override void HandleForTriggerCreature(MagicCpt magicCpt, Collider collider)
    {

    }
}