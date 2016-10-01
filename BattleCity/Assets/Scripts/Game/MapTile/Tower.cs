/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 10:05
*/

using System;
using UnityEngine;

[Serializable]
public class Tower : MapObject
{
    //public static readonly Uid64 UNIQ = "CE3DD16E466BCE02";

    [SerializeField]
    private float _health;

    [SerializeField]
    private Dir4 _direction;
    public override Dir4 Direction
    {
        get { return _direction; }
        set {
            if (_direction != value) {
                Quaternion newRot = MapObjectUtils.GetRotation2D(value);
                this.transform.rotation = newRot;
                _direction = (Dir4)((int)value % 4);
            }
        }
    }
    public override Rect Rect
    {
        get {
            float xOff = this.transform.position.x - (int)MapSize / 2;
            float yOff = this.transform.position.y - (int)MapSize / 2;

            return new Rect(xOff, yOff, (int)MapSize, (int)MapSize);
        }
    }
}
