using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class BlockTypeShineLightWhite : Block
{
    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);
        VisualEffect visualEffect = obj.GetComponent<VisualEffect>();
        Light light = obj.GetComponent<Light>();
        //设置粒子颜色
        Color colorLight = blockInfo.GetBlockColor();
        visualEffect.SetVector4("ShineColor", new Vector4(colorLight.r, colorLight.g, colorLight.b, colorLight.a));
        light.color = colorLight;
    }
}