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

    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _health;

    private bool _canMove = true;

    private void Update()
    {
        ChangeDirection();
        MoveOnGround();
    }

    private void MoveOnGround()
    {
        Vector3 curPos = this.transform.position;
        Vector3 direction = MapObjectUtils.GetDirection2D(Direction);
        Vector3 nextPos = Vector3.MoveTowards(curPos,  curPos + direction, _moveSpeed * Time.deltaTime);

        bool canMove = !Stage.Instance.MapRectCollisionDetect(this, ObstacleType.Ground);
        if (canMove) {
            this.transform.position = nextPos;
        }
    }

    private void ChangeDirection()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            Direction = Dir4.Up;
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            Direction = Dir4.Right;
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            Direction = Dir4.Down;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            Direction = Dir4.Left;
        }
    }
}
