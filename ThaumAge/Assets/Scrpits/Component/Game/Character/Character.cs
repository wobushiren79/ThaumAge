using UnityEditor;
using UnityEngine;

public class Character : BaseMonoBehaviour
{
    [Header("角色头部（需要指定）")]
    public GameObject characterHead;
    [Header("角色身体（需要指定）")]
    public GameObject characterBody;

    [HideInInspector]
    public CharacterSkin characterSkin;

    private void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        InitCharacterSkin();
    }

    /// <summary>
    /// 初始化角色皮肤
    /// </summary>
    private void InitCharacterSkin()
    {
        if (characterHead == null)
        {
            LogUtil.LogError($"初始化角色失败，{gameObject.name}的角色 缺少 Head 部件");
            return;
        }
        if (characterBody == null)
        {
            LogUtil.LogError($"初始化角色失败，{gameObject.name}的角色 缺少 Body 部件");
            return;
        }
        MeshRenderer headRender = characterHead.GetComponent<MeshRenderer>();
        SkinnedMeshRenderer bodyRender = characterBody.GetComponentInChildren<SkinnedMeshRenderer>();
        characterSkin = new CharacterSkin(headRender, bodyRender);
    }
}