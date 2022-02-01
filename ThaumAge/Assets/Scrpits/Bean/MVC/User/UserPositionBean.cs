using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class UserPositionBean 
{
    public int worldType;

    public Vector3Bean position = new Vector3Bean(Vector3.zero);

    public void SetWorldType(WorldTypeEnum worldType)
    {
        this.worldType = (int)worldType;
    }

    public void SetPosition(Vector3 position)
    {
        this.position = new Vector3Bean(position);
    }

    /// <summary>
    /// 获取位置
    /// </summary>
    /// <param name="worldType"></param>
    /// <param name="position"></param>
    public void GetWorldPosition(out WorldTypeEnum worldType,out Vector3 position)
    {
        worldType = (WorldTypeEnum)this.worldType;
        position = this.position.GetVector3();
    }
}