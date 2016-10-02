/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 23:48
*/

using System;
using UnityEngine;

public class TowerBigController: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "BE3E92452FE8B102";
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
    private Transform _playerTarget;

    private Transform _bulletSpawner;

    void Start () 
	{
        GameObject targ = GameObject.FindGameObjectWithTag("Player");
        if (targ != null) {
            _playerTarget = targ.transform;
        }
        _bulletSpawner = this.transform.FindChild("BulletSpawn");
        _fireBehaviour = this.GetComponent<BehFire>();
        IDamageable dam = this.GetComponent<IDamageable>();
        if (dam != null) { dam.NoHealthEvent += NoHealthEventHandler; }
	}
	
	void Update ()
	{
        Fire();
        if (_playerTarget == null) {
            GameObject targ = GameObject.FindGameObjectWithTag("Player");
            if (targ != null) {
                _playerTarget = targ.transform;
            }
        }
	}

    private void Fire()
    {
        if (_fireBehaviour == null) { return; }
        bool detect = DetectTarget();
        if (detect) {
            float dx = this.transform.position.x - _playerTarget.position.x;
            float dy = this.transform.position.y - _playerTarget.position.y;

            if (Mathf.Abs(dx) > Math.Abs(dy)) {
                if (dx < 0) { MapObj.Direction = Dir4.Right; }
                else { MapObj.Direction = Dir4.Left; }
            }
            else {
                if (dy < 0) { MapObj.Direction = Dir4.Up; }
                else { MapObj.Direction = Dir4.Down; }
            }

            _fireBehaviour.Fire(this.gameObject, MapObj.Direction, _bulletSpawner);
        }
    }

    private bool DetectTarget()
    {
        if (_playerTarget == null) { return false; }
        float dist = Vector3.Distance(_playerTarget.position, this.transform.position);
        if (dist < 10) { return true; }
        return false;
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
