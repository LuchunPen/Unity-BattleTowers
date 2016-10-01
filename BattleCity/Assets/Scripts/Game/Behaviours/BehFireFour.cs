/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 23:53
*/

using System;
using UnityEngine;

public class BehFireFour: BehFire
{
    //public static readonly Uid64 UNIQ = "AE3E9379FC50EB02";

    [SerializeField]
    private Transform[] _bulletSpawns = new Transform[4];

    void Awake()
    {

    }

	void Start () 
	{
	    
	}
	
	void Update ()
	{
	    
	}

    public override void Fire(Dir4 direction)
    {
        throw new NotImplementedException();
    }
}
