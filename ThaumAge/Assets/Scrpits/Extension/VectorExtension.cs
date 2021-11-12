using UnityEditor;
using UnityEngine;

public static class VectorExtension
{
    //------------Transform------------
    public static void AddPositionXYZ(this Transform self, float x, float y, float z)
    {
        self.position = self.position.AddXYZ(x, y, z);
    }

    public static void AddPositionXY(this Transform self, float x, float y)
    {
        self.position = self.position.AddXY(x, y);
    }

    public static void AddPositionXZ(this Transform self, float x, float z)
    {
        self.position = self.position.AddXZ(x, z);
    }

    public static void AddPositionYZ(this Transform self, float y, float z)
    {
        self.position = self.position.AddYZ(y, z);
    }

    public static void AddPositionX(this Transform self, float x)
    {
        self.position = self.position.AddX(x);
    }

    public static void AddPositionY(this Transform self, float y)
    {
        self.position = self.position.AddY(y);
    }

    public static void AddPositionZ(this Transform self, float z)
    {
        self.position = self.position.AddZ(z);
    }

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

    //-------Vector2--------
    public static Vector2 AddX(this Vector2 self, float add)
    {
        self.x += add;
        return self;
    }
    public static Vector2 AddY(this Vector2 self, float add)
    {
        self.y += add;
        return self;
    }
    public static Vector2 AddXY(this Vector2 self, float x, float y)
    {
        self.x += x;
        self.y += y;
        return self;
    }

    //-------Vector3--------
    public static Vector3 AddX(this Vector3 self, float add)
    {
        self.x += add;
        return self;
    }
    public static Vector3 AddY(this Vector3 self, float add)
    {
        self.y += add;
        return self;
    }
    public static Vector3 AddZ(this Vector3 self, float add)
    {
        self.z += add;
        return self;
    }
    public static Vector3 AddXY(this Vector3 self, float x, float y)
    {
        self.x += x;
        self.y += y;
        return self;
    }
    public static Vector3 AddXZ(this Vector3 self, float x, float z)
    {
        self.x += x;
        self.z += z;
        return self;
    }
    public static Vector3 AddYZ(this Vector3 self, float y, float z)
    {
        self.y += y;
        self.z += z;
        return self;
    }
    public static Vector3 AddXYZ(this Vector3 self, float x, float y, float z)
    {
        self.x += x;
        self.y += y;
        self.z += z;
        return self;
    }

    //-------Vector2Int--------
    public static Vector2Int AddX(this Vector2Int self, int add)
    {
        self.x += add;
        return self;
    }
    public static Vector2Int AddY(this Vector2Int self, int add)
    {
        self.y += add;
        return self;
    }
    public static Vector2Int AddXY(this Vector2Int self, int x, int y)
    {
        self.x += x;
        self.y += y;
        return self;
    }

    //-------Vector3Int--------
    public static Vector3Int AddX(this Vector3Int self, int add)
    {
        self.x += add;
        return self;
    }
    public static Vector3Int AddY(this Vector3Int self, int add)
    {
        self.y += add;
        return self;
    }
    public static Vector3Int AddZ(this Vector3Int self, int add)
    {
        self.z += add;
        return self;
    }
    public static Vector3Int AddXY(this Vector3Int self, int x, int y)
    {
        self.x += x;
        self.y += y;
        return self;
    }
    public static Vector3Int AddXZ(this Vector3Int self, int x, int z)
    {
        self.x += x;
        self.z += z;
        return self;
    }
    public static Vector3Int AddYZ(this Vector3Int self, int y, int z)
    {
        self.y += y;
        self.z += z;
        return self;
    }
    public static Vector3Int AddXYZ(this Vector3Int self, int x, int y, int z)
    {
        self.x += x;
        self.y += y;
        self.z += z;
        return self;
    }
}