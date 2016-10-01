/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 01:04
*/

using System;
using UnityEngine;

[Serializable]
public class Tank : MapObject
{
    //public static readonly Uid64 UNIQ = "EE3D52A37C665E02";
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

    [SerializeField]
    private float _health;

    private void Start()
    {

    }

    private void Update()
    {
        
    }
}
