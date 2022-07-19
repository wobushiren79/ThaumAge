using UnityEditor;
using UnityEngine;

public class EffectDeadBody : EffectBase
{
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(Mesh bodyMesh, Texture2D bodyTex)
    {
        ParticleSystem bodyPS = listPS[0];
        var psShape = bodyPS.shape;
        psShape.mesh = bodyMesh;
        psShape.texture = bodyTex;
        //var renderer= bodyPS.gameObject.GetComponent<ParticleSystemRenderer>();
        //renderer.material = bodyMat;

    }
}