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

    void Start () 
	{
        GameObject targ = GameObject.FindGameObjectWithTag("Player");
        if (targ != null) {
            _playerTarget = targ.transform;
        }

        _fireBehaviour = this.GetComponent<BehFire>();
	}
	
	void Update ()
	{
        Fire();
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

            _fireBehaviour.Fire(MapObj.Direction);
        }
    }

    private bool DetectTarget()
    {
        if (_playerTarget == null) { return false; }
        float dist = Vector3.Distance(_playerTarget.position, this.transform.position);
        if (dist < 10) { return true; }
        return false;
    }
}
