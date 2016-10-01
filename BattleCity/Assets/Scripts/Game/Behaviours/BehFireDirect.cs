/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:13
*/

using System;
using UnityEngine;

public class BehFireDirect: BehFire
{
    //public static readonly Uid64 UNIQ = "FE3E7C0CF8952902";
    [SerializeField]
    private Transform _bulletSpawn;

    private float _lastFire;

    void Start () 
	{
        if (_bulletSpawn == null) {
            _bulletSpawn = this.transform.FindChild("BulletSpawn");
        }
    }

    public override void Fire(Dir4 direction)
    {
        float curTime = Time.time;
        if (_lastFire + _fireCooldown < curTime) {
            BulletController b = Instantiate(_bulletPref, _bulletSpawn.transform.position, Quaternion.identity) as BulletController;
            b.SetOwner(MapObj);
            b.MapObj.Direction = direction;
            _lastFire = curTime;
        }
    }
}
