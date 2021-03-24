using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public AIPath aiPath;

    public Transform targetPosition;
    private void Start()
    {
        AstarPath.active.Scan();
        aiPath.isStopped = false;
        aiPath.destination = targetPosition.position;
        aiPath.SearchPath();
        aiPath.onSearchPath = () =>
        {
            aiPath.canMove = true;
        };
    }

    void Update()
    {
    }

}
