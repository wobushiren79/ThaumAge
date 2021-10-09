﻿using UnityEditor;
using UnityEngine;

public static class VectorExtension
{
    //------------Vector3------------
    public static void SetPositionXYZ(this Transform self, float x, float y, float z)
    {
        self.position = new Vector3(x, y, z);
    }

    public static void SetPositionXY(this Transform self, float x, float y)
    {
        self.SetPositionXYZ(x, y, self.position.z);
    }

    public static void SetPositionXZ(this Transform self, float x, float z)
    {
        self.SetPositionXYZ(x, self.position.y, z);
    }

    public static void SetPositionYZ(this Transform self, float y, float z)
    {
        self.SetPositionXYZ(self.position.x, y, z);
    }

    public static void SetPositionX(this Transform self, float x)
    {
        self.SetPositionXYZ(x, self.position.y, self.position.z);
    }

    public static void SetPositionY(this Transform self, float y)
    {
        self.SetPositionXYZ(self.position.x, y, self.position.z);
    }

    public static void SetPositionZ(this Transform self, float z)
    {
        self.SetPositionXYZ(self.position.x, self.position.y, z);
    }
}