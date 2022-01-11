using UnityEditor;
using UnityEngine;
using DG.Tweening;

public partial class UIViewSynthesisMaterial : BaseUIView
{
    public ItemsSynthesisMaterialsBean materialsData;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="materialsData"></param>
    public void SetData(ItemsSynthesisMaterialsBean materialsData, float startAngle)
    {
        this.materialsData = materialsData;
        AnimForMove(startAngle);
        SetIcon(materialsData.itemIds[0]);
    }

    public void SetIcon(long itemsId)
    {
        ItemsHandler.Instance.SetItemsIconById(ui_Icon, itemsId);
    }

    public void AnimForMove(float startAngle)
    {
        //获取移动的路径
        Vector2[] path = VectorUtil.GetListCirclePosition(36, startAngle, Vector2.zero, 95, true);
        transform.DOKill();

        rectTransform
            .DOLocalPath(path.ToVector3(), 90)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}