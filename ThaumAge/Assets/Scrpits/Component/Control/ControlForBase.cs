using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlForBase : BaseMonoBehaviour
{
    /// <summary>
    /// ���ؿ���
    /// </summary>
    /// <param name="enabled"></param>
    public virtual void EnabledControl(bool enabled)
    {
        this.enabled = enabled;
    }
}
