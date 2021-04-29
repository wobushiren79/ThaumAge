using UnityEditor;
using UnityEngine;
using System.Collections;
public class AstarPathHandler : BaseHandler<AstarPathHandler, AstarPathManager>
{

    public void RefreshAllGraph()
    {
        AstarPath.active.Scan();
        //AstarPath.active.Scan();
    }

    public void RefreshGraph()
    {
        manager.proceduralGridMover.UpdateGraph();
        //AstarPath.active.Scan();
    }
    //public void RefreshPath(Vector3 center,Vector3 size)
    //{
    //    AstarPath.active.UpdateGraphs(new Bounds(center, size));
    //}
}