using UnityEditor;
using UnityEngine;

public class EffectDamageText : EffectBase
{
    public void Update()
    {
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        transform.LookAt(mainCamera.transform.position);
    }
}