using UnityEditor;
using UnityEngine;

public class PlayerTargetBlock : BaseMonoBehaviour
{
    //互动
    public GameObject objInteractive;

    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    public void Awake()
    {
        meshFilter = objInteractive.GetComponent<MeshFilter>();
        meshRenderer = objInteractive.GetComponent<MeshRenderer>();
    }
    public void Show(Vector3 position, Block tagetBlock ,bool isInteractive)
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