using UnityEditor;
using UnityEngine;
using DG.Tweening;

public partial class UIViewSynthesisMaterial : BaseUIView
{
    public ItemsSynthesisMaterialsBean materialsData;

    protected Material matIcon;
    public override void Awake()
    {
        base.Awake();
        //重新实例化一份材质球
        matIcon = new Material(ui_Icon.material);
        ui_Icon.material = matIcon;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        rectTransform.DOKill();
        StopAllCoroutines();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (materialsData == null)
            return;
        SetCanHasItems(materialsData.itemIds, materialsData.itemNumber);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="materialsData"></param>
    public void SetData(ItemsSynthesisMaterialsBean materialsData, float startAngle)
    {
        this.materialsData = materialsData;
        AnimForMove(startAngle);
        AnimForChangeMaterial(materialsData.itemIds, 0);
        RefreshUI();
    }

    /// <summary>
    /// 改变素材
    /// </summary>
    /// <param name="itemsId"></param>
    public void ChangeMaterial(long itemsId)
    {
        SetIcon(itemsId);
        SetPopupInfo(itemsId);
        SetNumber(materialsData.itemNumber);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="itemsId"></param>
    public void SetIcon(long itemsId)
    {
        ItemsHandler.Instance.SetItemsIconById(ui_Icon, itemsId);
    }

    /// <summary>
    /// 设置弹出信息
    /// </summary>
    /// <param name="itemsId"></param>
    public void SetPopupInfo(long itemsId)
    {
        ui_ViewSynthesisMaterial.SetItemId(itemsId);
    }

    /// <summary>
    /// 设置合成数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(long number)
    {
        ui_TVNumber.text = $"{number}";
    }

    /// <summary>
    /// 设置是否有道具
    /// </summary>
    public void SetCanHasItems(long[] listItemsId, long itemsNumber)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        bool hasEnoughItem = false;
        for (int i = 0; i < listItemsId.Length; i++)
        {
            long itemsId = listItemsId[i];
            if (userData.HasEnoughItem(itemsId, itemsNumber))
            {
                hasEnoughItem = true;
                break;
            }
        }
        if (hasEnoughItem)
        {
            ui_TVNumber.color = Color.green;
            ui_Icon.material.SetFloat("_EffectAmount", 0);
        }
        else
        {
            ui_TVNumber.color = Color.white;
            ui_Icon.material.SetFloat("_EffectAmount", 1);
        }
    }
    /// <summary>
    /// 移动动画
    /// </summary>
    /// <param name="startAngle"></param>
    public void AnimForMove(float startAngle)
    {
        //获取移动的路径
        Vector2[] path = VectorUtil.GetListCirclePosition(36, startAngle, Vector2.zero, 120, true);
        rectTransform.DOKill();
        rectTransform
            .DOLocalPath(path.ToVector3(), 72)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    /// <summary>
    /// 改变素材动画，用于可替换材料的合成
    /// </summary>
    public void AnimForChangeMaterial(long[] listItemsId, int startIndex)
    {
        ChangeMaterial(listItemsId[startIndex]);
        this.WaitExecuteSeconds(2, () =>
        {
            startIndex++;
            if (startIndex >= listItemsId.Length)
                startIndex = 0;
            ChangeMaterial(listItemsId[startIndex]);
            AnimForChangeMaterial(listItemsId, startIndex);
        });
    }
}