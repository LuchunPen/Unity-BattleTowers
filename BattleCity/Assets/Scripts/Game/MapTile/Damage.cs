/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 23:16
*/
using System;

public enum DamageType
{
    Simple = 0,
    ArmorPiercing = 1,
}

public struct Damage
{
    public DamageType DType;
    public int Value;
}
