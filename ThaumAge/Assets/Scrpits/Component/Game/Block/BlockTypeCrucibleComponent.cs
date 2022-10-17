using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class BlockTypeCrucibleComponent : BlockTypeComponent
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerInfo.Items)
        {
            Destroy(other.gameObject);
        }
    }
}