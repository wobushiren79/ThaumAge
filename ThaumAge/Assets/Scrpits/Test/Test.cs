
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Test : BaseMonoBehaviour
{
    public MagicBean magicData;

    protected List<Vector3Int> listData1 = new List<Vector3Int>();
    protected HashSet<Vector3Int> listData2 = new HashSet<Vector3Int>();
    protected Dictionary<Vector3Int, string> listData3 = new Dictionary<Vector3Int, string>();

    public void Start()
    {
        // Assuming you have a reference to the MeshRenderer component
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // Set the culling mode to Front
        //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        //meshRenderer.receiveShadows = true;
        //meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes;
        //meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
        //meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
        //meshRenderer.allowOcclusionWhenDynamic = true;
        //meshRenderer.sortingLayerName = "Default";
        //meshRenderer.sortingOrder = 0;
        //meshRenderer.renderingLayerMask = 1;

        // Set the culling mode to Front or Off
        //meshRenderer.material.SetInteger("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            //Debug.Log("tEST1");
            //Player player = GameHandler.Instance.manager.player;
            //magicData.createPosition = player.transform.position + Vector3.up;
            //magicData.direction = Camera.main.transform.forward;
            //magicData.createTargetId = player.gameObject.GetInstanceID();
            //magicData.createTargetObj = player.gameObject;
            //MagicHandler.Instance.CreateMagic(magicData);
            listData1.Clear();
            listData2.Clear();
            listData3.Clear();
            Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
            stopwatch.Start();
            Vector3Int itemTest = new Vector3Int(123, 167, 12361);
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        listData2.Add(new Vector3Int(x, y, z));
                    }
                }
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("HashSetAdd " + stopwatch.ElapsedTicks);
            stopwatch.Reset();
            stopwatch.Restart();


            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        listData1.Add(new Vector3Int(x, y, z));
                    }
                }
            }

            stopwatch.Stop();
            UnityEngine.Debug.Log("ListAdd " + stopwatch.ElapsedTicks);
            //stopwatch.Reset();
            //stopwatch.Restart();
            //for (int i = 0; i < listData2.Count; i++)
            //{
            //    var item = listData2.ElementAt(i);
            //}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("HashSet " + stopwatch.ElapsedTicks);

            //stopwatch.Reset();
            //stopwatch.Restart();
            //for (int i = 0; i < listData1.Count; i++)
            //{
            //    var item = listData1[i];
            //}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("List " + stopwatch.ElapsedTicks);

            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < listData2.Count; i++)
            {
                bool isA = listData2.Contains(Vector3Int.zero);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("HashSet " + stopwatch.ElapsedTicks);

            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < listData1.Count; i++)
            {
                bool isA = listData1.Contains(Vector3Int.zero);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("List " + stopwatch.ElapsedTicks);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            LogUtil.LogError("TEst");
            SceneUtil.SceneChange(ScenesEnum.MainScene);
        }
    }


}
