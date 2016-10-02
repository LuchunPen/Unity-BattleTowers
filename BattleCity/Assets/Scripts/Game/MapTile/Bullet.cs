/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 09:14
*/

using System;
using UnityEngine;

public class Bullet: MapObject
{
    //public static readonly Uid64 UNIQ = "EE3DC578BFF95402";

    public override Rect Rect
    {
        get {
            Vector2 pos = this.transform.position;
            return new Rect(pos.x - 0.5f, pos.y - 0.5f, (int)MapSize, (int)MapSize);
        }
    }
}
