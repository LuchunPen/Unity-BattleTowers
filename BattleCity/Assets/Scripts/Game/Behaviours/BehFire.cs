/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:04
*/

using System;
using UnityEngine;

public abstract class BehFire: MonoBehaviour
{
    //public static readonly Uid64 UNIQ = "BE3E79ECD5E42A02";

    private MapObject _thisMapobj;
    public MapObject MapObj
    {
        get {
            if (_thisMapobj == null) {
                _thisMapobj = this.GetComponent<MapObject>();
            }
            return _thisMapobj;
        }
    }

    [SerializeField]
    protected BulletController _bulletPref;
    public BulletController Bullet { get { return _bulletPref; } }

    [SerializeField]
    protected float _fireCooldown;
    public float FireCoolown { get { return _fireCooldown; } }

    public abstract void Fire(object sender, Dir4 direction, Transform bulletSpawn);
}
