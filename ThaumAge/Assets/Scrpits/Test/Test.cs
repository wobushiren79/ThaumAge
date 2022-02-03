using UnityEngine;

public class Test : BaseMonoBehaviour
{
    public float angle = 0;
    private void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            TestM();
        }
    }

    public void TestM()
    {
        int cout = 100;
        for (int x = 0; x < cout; x++)
        {
            for (int y = 0; y < cout; y++)
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.position = new Vector3(x, 0, y);
                Light light = gameObject.AddComponent<Light>();
                light.type = LightType.Point;
                light.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
    }
}
