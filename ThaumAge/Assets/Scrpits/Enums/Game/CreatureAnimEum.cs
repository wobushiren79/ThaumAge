using UnityEditor;
using UnityEngine;

public enum CreatureAnimBaseState
{
    Idle = 0,//闲置
    Walk = 1,//行走
    Seat = 2,//坐下
    Climb = 3,//攀爬
    Take = 4,//拿取

    AttackMelee = 11,//攻击 近战
    AttackRemote = 21, //攻击 远程
}

