using TMPro;
using UnityEditor;
using UnityEngine;

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

        this.maxLife = maxLife;
        this.currentLife = currentLife;
        this.lifePro = currentLife / (float)maxLife;
        //设置进度
        srCurrentLife.size = new Vector2(lifePro, 1f);
        //设置文字显示
        tvLife.text = $"{currentLife}/{maxLife}";
        //设置10s时间隐藏
        this.WaitExecuteSeconds(10, () =>
        {
            this.ShowObj(false);
        });
    }
}