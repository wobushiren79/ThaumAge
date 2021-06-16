using UnityEditor;
using UnityEngine;

public class PlayerTargetBlock :BaseMonoBehaviour
{
    public void Show(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}