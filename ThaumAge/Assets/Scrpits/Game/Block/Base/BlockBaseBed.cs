using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseBed : BlockBaseLink
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        listLinkPosition = new List<Vector3Int>() { Vector3Int.forward };
    }

    /// <summary>
    /// 互动 
    /// </summary>
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        //暂时解除控制
        ControlForPlayer controlForPlayer = GameControlHandler.Instance.manager.controlForPlayer;
        ControlForCamera controlForCamera = GameControlHandler.Instance.manager.controlForCamera;
        controlForPlayer.EnabledControl(false);
        controlForCamera.EnabledControl(false);

        CameraHandler.Instance.SetCameraAxis(-70, 0);
        //获取链接数据
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        BlockBean blockData = targetChunk.GetBlockData(worldPosition - targetChunk.chunkData.positionForWorld);
        BlockMetaBed blockMetaData = GetLinkBaseBlockData<BlockMetaBed>(blockData.meta);

        //获取身体的旋转角度
        float angleY = direction == BlockDirectionEnum.UpForward
            ? 0 : direction == BlockDirectionEnum.UpBack
            ? 180 : direction == BlockDirectionEnum.UpLeft
            ? 90 : direction == BlockDirectionEnum.UpRight
            ? -90 : 0;
        //获取身体偏移位置
        float moveX = direction == BlockDirectionEnum.UpLeft
            ? 0.5f : direction == BlockDirectionEnum.UpRight
            ? -0.5f : 0;
        float moveZ = direction == BlockDirectionEnum.UpForward
            ? 0.5f : direction == BlockDirectionEnum.UpBack
            ? -0.5f : 0;

        Player player = GameHandler.Instance.manager.player;
        player.transform.position = blockMetaData.GetBasePosition() + new Vector3(0.5f, 0.5f, 0.5f);
        player.transform.eulerAngles = new Vector3(0, 180, 0);
        player.character.transform.localEulerAngles = new Vector3(-90, angleY, 0);
        player.character.transform.localPosition = new Vector3(moveX, 0, moveZ);
        //设置时间
        GameTimeHandler.Instance.SetGameTime(6, 0);

        Action callBackForFinish = () =>
        {
            //恢复控制
            controlForPlayer.EnabledControl(true);
            controlForCamera.EnabledControl(true);
            player.character.transform.localEulerAngles = new Vector3(0, 0, 0);
            player.character.transform.localPosition = new Vector3(0, 0, 0);
        };

        GameTimeHandler.Instance.WaitExecuteSeconds(3, callBackForFinish);

        //保存位置
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userPosition.SetWorldType(WorldCreateHandler.Instance.manager.worldType);
        userData.userPosition.SetPosition(player.transform.position);
        GameDataHandler.Instance.manager.SaveUserData(userData);
    }
}