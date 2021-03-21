using UnityEngine;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{

    CharacterController cc;
    public float speed = 20;
    public float viewRange = 30;
    public Chunk chunkPrefab;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        transform.rotation *= Quaternion.Euler(0f, x, 0f);
        transform.rotation *= Quaternion.Euler(-y, 0f, 0f);

        if (Input.GetButton("Jump"))
        {
            cc.Move((transform.right * h + transform.forward * v + transform.up) * speed * Time.deltaTime);
        }
        else
        {
            cc.SimpleMove(transform.right * h + transform.forward * v * speed);
        }
    }


}
