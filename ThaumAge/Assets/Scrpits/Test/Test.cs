
using UnityEngine;

public class Test : BaseMonoBehaviour
{
    public Vector3Int position;
    public int index;
    private void OnGUI()
    {
        if (GUILayout.Button("Test1"))
        {
            TestM();
        }
        if (GUILayout.Button("Test2"))
        {
            TestM2();
        }
    }

    public void TestM()
    {
        index = position.x * 16 * 256 + position.y * 16 + position.z;
    }
    public void TestM2()
    {
        position = new Vector3Int((index % (16 * 256 * 16)) / (16 * 256), (index % (16 * 256)) / 16, index % 16);
    }
}
