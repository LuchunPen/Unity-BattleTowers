/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:04
*/

using System;
using UnityEngine;

public abstract class BehMove: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "EE3E79D2A9FF3102";
    protected Transform _myTrans;

    private MapObject _mapObject;
    public MapObject MapObj
    {
        get {
            if (_mapObject == null) {
                _mapObject = this.GetComponent<MapObject>();
            }
            return _mapObject;
        }
    }

    [SerializeField]
    protected float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    protected ObstacleType _moveObstacleMask;

    void Awake()
    {
        _myTrans = this.transform;
    }

    public abstract void ChangeDirection(Dir4 newDirection);
    public abstract ObstacleType Move();
}
