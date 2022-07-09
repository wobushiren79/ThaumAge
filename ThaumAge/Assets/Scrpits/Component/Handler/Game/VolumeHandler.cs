using System.Collections;
using UnityEngine;

public class VolumeHandler : BaseHandler<VolumeHandler, VolumeManager>
{
    //远景模糊检测物体
    protected int layerDepthOfField =
        1 << LayerInfo.Character |
        1 << LayerInfo.Creature |
        1 << LayerInfo.ChunkCollider |
        1 << LayerInfo.ChunkTrigger;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        manager.SetShadowsDistance(gameConfig.shadowDis);
    }

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
        else
        {
            //摄像头发射射线 检测距离
            RayUtil.RayToScreenPointForScreenCenter(5000, layerDepthOfField, out bool isCollider, out RaycastHit hit);
            if (isCollider)
            {
                Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
                float dis = Vector3.Distance(mainCamera.transform.position, hit.point);
                if (dis <= 4)
                {
                    manager.SetDepthOfField(0, 0, 4, 104);
                }
                else
                {
                    manager.SetDepthOfField(1f, 4f, dis, dis + 100);
                }
            }
            else
            {
                manager.SetDepthOfField(0, 0, 0, 0);
            }
        }
    }
}