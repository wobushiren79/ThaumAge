using UnityEngine;
using UnityEngine.UI;

public class BaseUIHandler<T, M> : BaseHandler<T, M>
    where M : BaseManager
    where T : BaseMonoBehaviour
{
    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public GraphicRaycaster graphicRaycaster;

    public int sortingOrder = 0;

    protected override void Awake()
    {
        canvas = gameObject.AddComponentEX<Canvas>();
        canvasScaler = gameObject.AddComponentEX<CanvasScaler>();
        graphicRaycaster = gameObject.AddComponentEX<GraphicRaycaster>();

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        canvasScaler.referenceResolution = new Vector2(1920 * gameConfig.uiSize, 1080 * gameConfig.uiSize);
        ChangeUIRenderMode(RenderMode.ScreenSpaceOverlay);
    }

    /// <summary>
    /// 修改UI的大小
    /// </summary>
    /// <param name="size"></param>
    public void ChangeUISize(float size)
    {
        canvasScaler.referenceResolution = new Vector2(1920 * size, 1080 * size);
    }

    /// <summary>
    /// 修改渲染模式
    /// </summary>
    /// <param name="renderMode"></param>
    public void ChangeUIRenderMode(RenderMode renderMode)
    {
        canvas.pixelPerfect = true;
        canvas.gameObject.layer = LayerInfo.UI;
        canvas.renderMode = renderMode;
        switch (renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                break;
            case RenderMode.ScreenSpaceCamera:
                canvas.planeDistance = 1;
                canvas.worldCamera = CameraHandler.Instance.manager.uiCamera;
                break;
            case RenderMode.WorldSpace:
                break;
        }
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = sortingOrder;
    }
}