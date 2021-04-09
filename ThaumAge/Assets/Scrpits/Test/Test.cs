using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public Vector2Int imageDim;
    public int regionAmount;
    public bool drawByDistance = false;


    private void Start()
    {
        Vector3 data = GetRotatedPosition(new Vector3(1f,0,1f),new Vector3(1,0,-1), new Vector3(0,90,0));
        LogUtil.Log(data+"");
    }


    public Vector3 GetRotatedPosition(Vector3 position,Vector3 centerPosition, Vector3 angles)
    {
        Vector3 direction = position - centerPosition;
        direction = Quaternion.Euler(angles) * direction; 
        return direction + centerPosition;
    }

}
