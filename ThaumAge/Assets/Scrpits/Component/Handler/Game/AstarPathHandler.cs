using UnityEditor;
using UnityEngine;

public class AstarPathHandler : BaseHandler<AstarPathHandler, AstarPathManager>
{
    public void RefreshPath()
    {
        //manager.proceduralGridMover.UpdateGraph();
        //AstarPath.active.Scan();
    }
    //public void RefreshPath(Vector3 center,Vector3 size)
    //{
    //    AstarPath.active.UpdateGraphs(new Bounds(center, size));
    //}
}