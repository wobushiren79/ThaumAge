using UnityEditor;
using UnityEngine;

public class BlockBaseTorch : Block
{

    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        BlockShapeCustomDirection blockShapeCustomDirection = blockShape as BlockShapeCustomDirection;
        blockShapeCustomDirection.SetColorsEmission(2);
    }

    /// <summary>
    /// 获取旋转角度 不旋转
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public override Vector3 GetRotateAngles(BlockDirectionEnum direction)
    {
        return Vector3.zero;
    }

    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, direction, obj);
        Transform tfEffect = obj.transform.Find("Effect");
        int unitTen = MathUtil.GetUnitTen((int)direction);
        switch (unitTen)
        {
            case 1:
            case 2:
                tfEffect.localPosition = new Vector3(0, -0.15f, 0);
                break;
            case 3:
                tfEffect.localPosition = new Vector3(0.2f, 0.25f, 0);
                break;
            case 4:
                tfEffect.localPosition = new Vector3(-0.2f, 0.25f, 0);
                break;
            case 5:
                tfEffect.localPosition = new Vector3(0, 0.25f, -0.2f);
                break;
            case 6:
                tfEffect.localPosition = new Vector3(0, 0.25f, 0.2f);
                break;
        }

    }
}