/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 07:53
*/

using System;
using UnityEngine;

public class NoDamage: BehDamage 
{
    //public static readonly Uid64 UNIQ = "CE483E82F63A7702";

    public override void SetDamage(Damage damage)
    {
        return;
    }
}
