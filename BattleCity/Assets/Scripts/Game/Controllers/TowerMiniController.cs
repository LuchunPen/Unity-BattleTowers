/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 23:38
*/

using System;
using UnityEngine;

public class TowerMiniController: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "AE3E8FE95F83D502";
    private MapObject _mapObj;
    public MapObject MapObj
    {
        get {
            if (_mapObj == null) {
                _mapObj = this.GetComponent<MapObject>();
            }
            return _mapObj;
        }
    }

    private BehFire _fireBehaviour;
    private Transform _bulletSpawner;

    void Start()
    {
        _mapObj = this.GetComponent<MapObject>();
        _fireBehaviour = this.GetComponent<BehFire>();
        _bulletSpawner = this.transform.FindChild("BulletSpawn");
        IDamageable dam = this.GetComponent<IDamageable>();
        if (dam != null) { dam.NoHealthEvent += NoHealthEventHandler; }
    }

	void Update ()
	{
        Fire();
	}

    private void Fire()
    {
        if (_fireBehaviour == null) { return; }
        _fireBehaviour.Fire(this.gameObject, MapObj.Direction, _bulletSpawner);
    }

    private void NoHealthEventHandler(object sender, EventArgs arg)
    {
        IDamageable dam = this.GetComponent<IDamageable>();
        if (dam != null) {
            dam.NoHealthEvent -= NoHealthEventHandler;
            BTGame.Current.Stage.UnregisterMapObject(_mapObj);
        }
        if (this.gameObject != null) {
            Destroy(this.gameObject);
        }
    }
}
