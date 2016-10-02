/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:13
*/

using System;
using UnityEngine;

public class BehFireDirectXY: BehFire
{
    //public static readonly Uid64 UNIQ = "FE3E7C0CF8952902"
    private float _lastFire;

    public override void Fire(object sender, Dir4 direction, Transform bulletSpawn)
    {
        if (bulletSpawn == null) { return; }
        float curTime = Time.time;
        if (_lastFire + _fireCooldown < curTime) {
            Quaternion rot = MapObjectUtils.GetRotation2D(direction);
            BulletController b = null;
            b = Instantiate(_bulletPref, bulletSpawn.transform.position, rot) as BulletController;
            b.MapObj.Direction = direction;
            if (sender != null) {
                b.SetOwner(sender as GameObject);
            }
            _lastFire = curTime;
        }
    }
}
