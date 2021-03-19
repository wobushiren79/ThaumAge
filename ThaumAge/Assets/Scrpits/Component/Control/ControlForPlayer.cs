using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlForPlayer : ControlForBase
{
    public Rigidbody rbCharacter;

    private void Update()
    {
        HandlerForMove();
        HandlerForJump();
        HandlerForUse();
    }

    /// <summary>
    /// 移动处理
    /// </summary>
    public void HandlerForMove()
    {
        InputAction moveAction = InputHandler.Instance.manager.GetMoveData();
        Vector2 moveData = moveAction.ReadValue<Vector2>();
        if (moveData == Vector2.zero)
            return;
        //旋转角色
        RotateCharacter(moveData, 5);
        //移动角色
        MoveCharacter(moveData, 20);
    }

    /// <summary>
    /// 跳跃处理
    /// </summary>
    public void HandlerForJump()
    {
        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);
        InputAction jumpAction = InputHandler.Instance.manager.GetJumpData();
        if (jumpAction.phase == InputActionPhase.Started)
        {
            JumpCharacter(1000);
        }
    }

    /// <summary>
    /// 使用处理
    /// </summary>
    public void HandlerForUse()
    {
        InputAction useAction = InputHandler.Instance.manager.GetUseData();
        if (useAction.phase == InputActionPhase.Started)
        {
            RayUtil.RayToScreenPoint(10, 1 << LayerInfo.Chunk, out bool isCollider, out RaycastHit hit);
            if (isCollider)
            {
                Chunk chunk = hit.collider.GetComponent<Chunk>();
                if (chunk)
                {
                    if (Mathf.Abs(hit.normal.y) > 0.01)
                    {
                        Debug.Log("上表面");
                        chunk.RemoveBlock(Vector3Int.CeilToInt(hit.point) - Vector3Int.up);
                    }
                    else
                    {
                        Debug.Log("侧面");
                        chunk.RemoveBlock(Vector3Int.CeilToInt(hit.point));
                    }
     
                }
            }
        }
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    /// <param name="movePosition"></param>
    public void MoveCharacter(Vector2 moveOffset, float moveSpeed)
    {
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        //获取摄像机方向 高度为0
        forward.y = 0;
        right.y = 0;
        //朝摄像头方向移动
        rbCharacter.transform.Translate(forward * Time.deltaTime * moveSpeed * moveOffset.y, Space.World);
        rbCharacter.transform.Translate(right * Time.deltaTime * moveSpeed * moveOffset.x, Space.World);
    }

    /// <summary>
    /// 角色跳跃
    /// </summary>
    /// <param name="jumpForce"></param>
    public void JumpCharacter(float jumpForce)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, 1 << LayerInfo.Ground);
        rbCharacter.AddForce(Vector3.up * jumpForce * Time.deltaTime);
    }

    /// <summary>
    /// 角色旋转
    /// </summary>
    /// <param name="targetPosition"></param>
    public void RotateCharacter(Vector2 moveOffset, float rotateSpeed)
    {
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        Vector3 rotateAngles = new Vector3(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        //前进后退的旋转
        if (moveOffset.y > 0)
        {
            rotateAngles.y += 0;
        }
        else if (moveOffset.y < 0)
        {
            rotateAngles.y += 180;
        }

        //左右移动的旋转
        if (moveOffset.x > 0)
        {
            rotateAngles.y += 90;
        }
        else if (moveOffset.x < 0)
        {
            rotateAngles.y += -90;
        }

        Quaternion rotate = Quaternion.Euler(rotateAngles);
        //朝摄像头方向移动
        rbCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);
    }
}