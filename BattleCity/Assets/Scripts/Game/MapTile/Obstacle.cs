/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 03:04
*/

using System;
using UnityEngine;

[Serializable]
public class Obstacle : MapObject
{
    //public static readonly Uid64 UNIQ = "FE3D6EC1E02D6A02";
    public override Dir4 Direction
    {
        get { return Dir4.Up; }
        set { return; }
    }

    private Rect r;
    public override Rect Rect
    {
        get {
            float xOff = this.transform.position.x - (int)MapSize.x / 2;
            float yOff = this.transform.position.y - (int)MapSize.y / 2;

            return new Rect(xOff, yOff, MapSize.x, MapSize.y);
        }
    }
}
