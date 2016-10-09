/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 07:44
*/

using System;
using UnityEngine;

public class NoMove: BehMove 
{
    //public static readonly Uid64 UNIQ = "CE483C7BA0ADC602";

    public override void ChangeDirection(MapObject mo, Dir4 newDirection)
    {
        return;
    }

    public override ObstacleType Move(Transform trans, MapObject mo)
    {
        return ObstacleType.None;
    }
}
