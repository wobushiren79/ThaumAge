using TMPro;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class CreatureCptLifeProgress : BaseMonoBehaviour
{
    public SpriteRenderer srCurrentLife;
    public TextMeshPro tvLife;

    public int maxLife;
    public int currentLife;
    public float lifePro;

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(int maxLife, int currentLife)
    {
        this.StopAllCoroutines();
        this.ShowObj(true);
        RefreshData(maxLife, currentLife);
        //设置10s时间隐藏
        this.WaitExecuteSeconds(10, () =>
        {
            this.ShowObj(false);
        });
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="maxLife"></param>
    /// <param name="currentLife"></param>
    public void RefreshData(int maxLife, int currentLife)
    {
        this.maxLife = maxLife;
        this.currentLife = currentLife;
        this.lifePro = currentLife / (float)maxLife;
        //设置进度
        AnimForLifeChange(lifePro);
        //设置文字显示
        tvLife.text = $"{currentLife}/{maxLife}";
    }

    /// <summary>
    /// 生命值修改动画
    /// </summary>
    public void AnimForLifeChange(float lifePro)
    {
        float timeAnim = 0.2f;
        srCurrentLife.material
            .DOKill();
        srCurrentLife.material
            .DOFloat(lifePro, "_Progress", timeAnim)
            .SetEase(Ease.OutCubic);
    }
}