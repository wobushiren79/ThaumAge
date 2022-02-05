using UnityEditor;
using UnityEngine;

public class PlayerTargetBlock : BaseMonoBehaviour
{
    //互动
    public GameObject objInteractive;

    public void Show(Vector3 position,bool isInteractive)
    {
        transform.position = position;
        gameObject.SetActive(true);

        objInteractive.ShowObj(isInteractive);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}