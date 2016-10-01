/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 04:04
*/
using System;
using UnityEngine;

public static class MapObjectUtils
{
    public static Quaternion GetRotation2D(this Vector2 direction, float angle)
    {
        float atan2 = Mathf.Atan2(direction.y, direction.x);
        return Quaternion.Euler(0f, 0f, (atan2 * Mathf.Rad2Deg) + angle);
    }

    public static Quaternion GetRotation2D(Dir4 direction)
    {
        return Quaternion.Euler(0f, 0f, (int)direction * 90);
    }

    public static Vector3 GetDirection2D(Dir4 direction)
    {
        switch(direction) {
            case Dir4.Up: return new Vector3(0, 1, 0);
            case Dir4.Left: return new Vector3(-1, 0, 0);
            case Dir4.Down: return new Vector3(0, -1, 0);
            case Dir4.Right: return new Vector3(1, 0, 0);
            default: return new Vector3(0, 0, 0);
        }
    }
}
