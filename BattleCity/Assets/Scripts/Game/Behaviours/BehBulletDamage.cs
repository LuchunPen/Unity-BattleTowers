/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 11:16
*/

using System;
using UnityEngine;

public class BehBulletDamage: BehDamage 
{
    //public static readonly Uid64 UNIQ = "FE486E0D642E5102";

    public override void SetDamage(Damage damage)
    {
        NoHealthTrigger();
    }
}
