using System;
using UnityEditor;
using UnityEngine;

public class ItemCptLaunch : BaseMonoBehaviour
{
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    protected ItemLaunchBean itemLaunchData;//发射数据

    protected Vector3 launchMoveSpeed;//初速度向量
    protected Vector3 gritySpeed;//当前重力速度

    protected bool isCheckShot = false;

    protected float timeForCheckShot = 0.02f;
    protected float timeForCheckShotEnd = 0.5f;

    protected float timeUpdateForCheckShot = 0;

    private float timeUpdateForLaunch;//已经过去的时间

    protected Collider shotCollder;//射中的物体

    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// 清除数据
    /// </summary>
    public void ClearData()
    {
        timeUpdateForLaunch = 0;
        timeUpdateForCheckShot = 0;
        shotCollder = null;
        isCheckShot = false;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemLaunchData"></param>
    public void SetData(ItemLaunchBean itemLaunchData)
    {
        this.itemLaunchData = itemLaunchData;
        //设置发射点
        transform.position = itemLaunchData.launchStartPosition;
        //通过一个公式计算出初速度向量
        //角度*力度
        launchMoveSpeed = itemLaunchData.launchDirection.normalized * itemLaunchData.launchPower;
        ClearData();
        ItemsHandler.Instance.manager.GetItemsIconById(itemLaunchData.itemId, (data) =>
        {
            Texture2D itemTex = TextureUtil.SpriteToTexture2D(data);
            //设置材质的贴图
            meshRenderer.material.mainTexture = itemTex;
            //获取道具的mesh
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemLaunchData.itemId);
            MeshUtil.MeshUtilData meshUtilData = new MeshUtil.MeshUtilData(itemTex, itemsInfo.GetItemsColor());
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
        this.WaitExecuteSeconds(0.06f, () =>
         {
             isCheckShot = true;
         });
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
        timeUpdateForLaunch += Time.deltaTime;
        timeUpdateForCheckShot += Time.deltaTime;

        gritySpeed = itemLaunchData.grity * (timeUpdateForLaunch += Time.fixedDeltaTime);
        //位移模拟轨迹
        Vector3 launchDirection = (launchMoveSpeed + gritySpeed) * Time.fixedDeltaTime;
        //设置位移和朝向
        transform.position += launchDirection;
        transform.forward = launchDirection;
        transform.eulerAngles += new Vector3(90,0,0);
        //检测是否射中
        if (timeUpdateForCheckShot >= timeForCheckShot)
        {
            timeUpdateForCheckShot = 0;
            Vector3 checkCenterPosition = transform.position;
            Collider[] collider = RayUtil.OverlapToSphere(checkCenterPosition, itemLaunchData.checkShotRange, itemLaunchData.checkShotLayer);
            if (isCheckShot && !collider.IsNull())
            {
                itemLaunchData.launchState = 2;
                shotCollder = collider[0];
                LaunchShotTarget(shotCollder);
                transform.SetParent(shotCollder.transform);
            }
        }
    }

    /// <summary>
    /// 处理-射中后
    /// </summary>
    public void HandleForLaunchEnd()
    {
        timeUpdateForLaunch += Time.deltaTime;
        timeUpdateForCheckShot += Time.deltaTime;
        //每隔一段时间检测一下
        if (timeUpdateForCheckShot >= timeForCheckShotEnd)
        {
            timeUpdateForCheckShot = 0;
            Vector3 checkCenterPosition = transform.position;
            bool hasParent = RayUtil.CheckToSphere(checkCenterPosition, itemLaunchData.checkShotRange, itemLaunchData.checkShotLayer);
            if (hasParent == false || shotCollder.gameObject.activeSelf == false)
            {
                LaunchDestory();
            }
        }
    }

    /// <summary>
    /// 处理发射物体消失
    /// </summary>
    public void HandleForDestory()
    {
        Player player = GameHandler.Instance.manager.player;
        float dis = Vector3.Distance(player.transform.position, transform.position);
        //如果和玩家的距离大于最大渲染距离 或者存在时间大于60s
        if (dis >= 512 || timeUpdateForLaunch > itemLaunchData.timeForDestroy)
        {
            LaunchDestory();
        }
    }

    /// <summary>
    /// 伤害处理
    /// </summary>
    public void LaunchShotTarget(Collider targetCollider)
    {
        itemLaunchData.actionShotTarget?.Invoke(targetCollider);
    }

    /// <summary>
    /// 删除发射物
    /// </summary>
    public void LaunchDestory()
    {
        Destroy(gameObject);
    }

}