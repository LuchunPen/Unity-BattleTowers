/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 07:44
*/

using System;
using UnityEngine;

public class NoFire: BehFire 
{
    //public static readonly Uid64 UNIQ = "DE483C4B479FEB02";

    public override void Fire(object sender, Dir4 direction, Transform bulletSpawn)
    {
        return;
    }

}
