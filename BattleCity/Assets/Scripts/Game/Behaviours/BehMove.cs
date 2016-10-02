/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:04
*/

using System;
using UnityEngine;

public abstract class BehMove: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "EE3E79D2A9FF3102";
    [SerializeField]
    protected float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    protected ObstacleType _moveObstacleMask;

    public abstract void ChangeDirection(MapObject mo, Dir4 newDirection);
    public abstract ObstacleType Move(Transform trans, MapObject mo);
}
