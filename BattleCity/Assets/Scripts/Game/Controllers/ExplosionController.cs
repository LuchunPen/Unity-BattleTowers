/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08/10/2016 14:53
*/

using System;
using UnityEngine;

public class ExplosionController: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "FE474F5FC04CFE02";
    void Start()
    {
        Destroy(this.gameObject, 1);
    }
}