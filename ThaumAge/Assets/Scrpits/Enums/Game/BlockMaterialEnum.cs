﻿using UnityEditor;
using UnityEngine;

public enum BlockMaterialEnum
{
    Normal = 1,//单面
    BothFace = 2,//双面
    BothFaceSwing = 3,//双面 单数底部不动 双数顶部不动 摇曳
    BothFaceSwingUniform = 4,//双面 规则的摇曳
    Water = 5,//水流
    Magma = 6,//岩浆

}