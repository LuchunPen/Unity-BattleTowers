/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:13
*/

using System;
using UnityEngine;

public class BehFireDirectXY: BehFire, IModifiable<BehFire>
{
    //public static readonly Uid64 UNIQ = "FE3E7C0CF8952902"
    private float _lastFire;

    protected Modificator<BehFire> _mod;
    public void SetModificator(Modificator<BehFire> modificator)
    {
        _mod = modificator;
    }

    public override void Fire(object sender, Dir4 direction, Transform bulletSpawn)
    {
        if (_mod != null && _mod.ModObject != null) {
            _mod.ModObject.Fire(sender, direction, bulletSpawn);
            return;
        }

        if (bulletSpawn == null) { return; }
        float curTime = Time.time;
        if (_lastFire + _fireCooldown < curTime) {
            BulletController b = BTGame.Current.BulletPool.GetItem(_bulletPref);
            b.transform.position = bulletSpawn.transform.position;
            b.MapObj.Direction = direction;
            b.transform.SetParent(BTGame.Current.Stage.transform);
            if (sender != null) {
                b.SetOwner(sender as GameObject);
            }
            b.GetComponent<BoxCollider2D>().enabled = true;
            b.gameObject.SetActive(true);
            _lastFire = curTime;
        }
    }
}
