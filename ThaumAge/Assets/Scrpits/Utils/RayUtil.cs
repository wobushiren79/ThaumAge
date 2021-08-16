using UnityEditor;
using UnityEngine;

public class RayUtil
{

    /// <summary>
    /// 屏幕中心射线检测
    /// </summary>
    /// <param name="isCollider"></param>
    /// <param name="hit"></param>
    public static void RayToScreenPointForScreenCenter(float maxDistance, int layerMask, out bool isCollider, out RaycastHit hit)
    {
        RayToScreenPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0), maxDistance, layerMask, out isCollider, out hit);
    }

    /// <summary>
    /// 屏幕点击射线检测
    /// </summary>
    /// <param name="isCollider"></param>
    /// <param name="hit"></param>
    public static void RayToScreenPoint(float maxDistance, int layerMask, out bool isCollider, out RaycastHit hit)
    {
        RayToScreenPoint(Input.mousePosition, maxDistance, layerMask, out isCollider, out hit);
    }

    public static void RayToScreenPoint(Vector3 position, float maxDistance, int layerMask, out bool isCollider, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        isCollider = Physics.Raycast(ray, out hit, maxDistance, layerMask);
    }

    /// <summary>
    /// 射线-球体
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Collider[] RayToSphere(Vector3 centerPosition, float radius, int layer)
    {
        return Physics.OverlapSphere(centerPosition, radius, layer);
    }


    /// <summary>
    /// 射线
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="maxDistance"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static RaycastHit[] RayToCastAll(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
    {
        return Physics.RaycastAll(origin, direction, maxDistance, layerMask);
    }

    /// <summary>
    /// 射线
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="maxDistance"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static bool RayToCast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask, out RaycastHit hit)
    {
        return Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);
    }
    public static bool RayToCast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
    {
        return Physics.Raycast(origin, direction, maxDistance, layerMask);
    }
}