using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIHandler<T, M> : BaseHandler<T,M>
    where M : BaseManager
    where T : BaseMonoBehaviour
{
    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public GraphicRaycaster graphicRaycaster;

    public int sortingOrder = 0;

    protected override void Awake()
    {
        canvas = CptUtil.AddCpt<Canvas>(gameObject);
        canvasScaler = CptUtil.AddCpt<CanvasScaler>(gameObject);
        graphicRaycaster = CptUtil.AddCpt<GraphicRaycaster>(gameObject);

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        ChangeUIRenderMode(RenderMode.ScreenSpaceOverlay);
    }


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