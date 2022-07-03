using UnityEditor;
using UnityEngine;

public class ItemCptLaunch : BaseMonoBehaviour
{
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    protected ItemLaunchBean itemLaunchData;//发射数据

    public Vector3 launchMoveSpeed;//初速度向量
    public Vector3 gritySpeed;//当前重力速度

    private float timeForLaunch;//已经过去的时间

    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemLaunchData"></param>
    public void SetData(ItemLaunchBean  itemLaunchData)
    {
        this.itemLaunchData = itemLaunchData;

        //通过一个公式计算出初速度向量
        //角度*力度
        launchMoveSpeed = itemLaunchData.launchDirection.normalized * itemLaunchData.launchPower;
        timeForLaunch = 0;

        ItemsHandler.Instance.manager.GetItemsIconById(itemLaunchData.itemId, (data) =>
        {
            Texture2D itemTex = TextureUtil.SpriteToTexture2D(data);
            //设置材质的贴图
            meshRenderer.material.mainTexture = itemTex;
            //获取道具的mesh
            MeshUtil.MeshUtilData meshUtilData = new MeshUtil.MeshUtilData(itemTex);
            Mesh picMesh = MeshUtil.GenerateMeshPicture(meshUtilData);
            meshFilter.mesh = picMesh;
        });
    }

    /// <summary>
    /// 发射
    /// </summary>
    public void Launch()
    {
        itemLaunchData.launchState = 1;
    }

    /// <summary>
    /// 数据更新
    /// </summary>
    void FixedUpdate()
    {
        HandleForDestory();
        switch (itemLaunchData.launchState)
        {
            case 0:
                HandleForLaunchPre();
                break;
            case 1:
                HandleForLaunching();
                break;
            case 2:
                HandleForLaunchEnd();
                break;
        }
    }

    /// <summary>
    /// 处理-未发射
    /// </summary>
    public void HandleForLaunchPre()
    {

    }

    /// <summary>
    /// 处理-发射中
    /// </summary>
    public void HandleForLaunching()
    {
        //计算物体的重力速度
        //v = at ;
        gritySpeed = itemLaunchData.grity * (timeForLaunch += Time.fixedDeltaTime);
        //位移模拟轨迹
        Vector3 launchDirection = (launchMoveSpeed + gritySpeed) * Time.fixedDeltaTime;
        //设置位移和朝向
        transform.position += launchDirection;
        transform.forward = launchDirection;
    }

    /// <summary>
    /// 处理-射中后
    /// </summary>
    public void HandleForLaunchEnd()
    {

    }

    /// <summary>
    /// 处理发射物体消失
    /// </summary>
    public void HandleForDestory()
    {
        Player player = GameHandler.Instance.manager.player;
        float dis = Vector3.Distance(player.transform.position, transform.position);
        //如果和玩家的距离大于最大渲染距离 或者存在时间大于60s
        if (dis >= 512 || timeForLaunch > 60) 
        {
            Destroy(gameObject);
        }
    }
}