using System.Collections;
using UnityEngine;

public class VolumeHandler : BaseHandler<VolumeHandler, VolumeManager>
{
    //远景模糊检测物体
    protected int layerDepthOfField;

    public override void Awake()
    {
        base.Awake();
        layerDepthOfField =
            1 << LayerInfo.Character |
            1 << LayerInfo.Creature |
            1 << LayerInfo.ChunkCollider |
            1 << LayerInfo.ChunkTrigger;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {

        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        manager.SetShadowsDistance(gameConfig.shadowDis);
    }

    protected float lastDisDepthOfField = 0;
    /// <summary>
    /// 设置远景模糊
    /// </summary>
    /// <param name="worldType"></param>
    public void SetDepthOfField(GameStateEnum gameState)
    {
        if (gameState == GameStateEnum.Main)
        {
            manager.SetDepthOfField(2, 3, 10, 20);
        }
        else if (gameState == GameStateEnum.Gaming)
        {
            float dis = 5000;
            //摄像头发射射线 检测距离
            RayUtil.RayToScreenPointForScreenCenter(dis, layerDepthOfField, out bool isCollider, out RaycastHit hit);
            if (isCollider)
            {
                Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
                dis = Vector3.Distance(mainCamera.transform.position, hit.point);
                if (dis <= 4)
                {
                    manager.SetDepthOfField(0, 0, 4, 14);
                    lastDisDepthOfField = dis;
                    return;
                }
            }
            if (dis == lastDisDepthOfField)
            {
                return;
            }
            manager.SetDepthOfField(1f, 4f, dis + 10, dis + 30);
            lastDisDepthOfField = dis;
        }
    }

    /// <summary>
    /// 设置颜色调整
    /// </summary>
    public void SetColorAdjustments(Color colorFilter, float postExposure = 0, float contrast = 0, float hueShift = 0, float saturation = 0)
    {
        manager.SetColorAdjustments(colorFilter, postExposure, contrast, hueShift, saturation);
    }
}