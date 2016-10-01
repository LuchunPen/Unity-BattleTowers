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
            float xOff = this.transform.position.x - (int)MapSize.x / 2;
            float yOff = this.transform.position.y - (int)MapSize.y / 2;

            return new Rect(xOff, yOff, MapSize.x, MapSize.y);
        }
    }

    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _health;

    [SerializeField]
    private MapObject _bulletFab;
    private GameObject _bulletSpawn;

    private bool _canMove = true;

    private void Start()
    {
        _bulletSpawn = this.transform.FindChild("BulletSpawn").gameObject;
    }

    private void Update()
    {
        ChangeDirection();
        MoveOnGround();
        Fire();
    }

    private void MoveOnGround()
    {
        Vector3 curPos = this.transform.position;
        Vector3 direction = MapObjectUtils.GetDirection2D(Direction);
        Vector3 nextPos = Vector3.MoveTowards(curPos,  curPos + direction, _moveSpeed * Time.deltaTime);

        bool canMove = Stage.Instance.MapRectCollisionDetect(this.Rect, this.Direction, ObstacleType.Ground) == null;
        if (canMove) {
            this.transform.position = nextPos;
        }
    }

    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Bullet b = Instantiate(_bulletFab, _bulletSpawn.transform.position, Quaternion.identity) as Bullet;
            b.Direction = this.Direction;
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
