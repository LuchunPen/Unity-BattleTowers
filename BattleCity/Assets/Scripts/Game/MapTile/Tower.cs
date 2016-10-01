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
    private MapObject _bulletFab;
    private GameObject _bulletSpawn;

    float coolDown = 0;
    float fireInteval = 1;

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
            float xOff = this.transform.position.x - (int)MapSize.x / 2;
            float yOff = this.transform.position.y - (int)MapSize.y / 2;

            return new Rect(xOff, yOff, MapSize.x, MapSize.y);
        }
    }

    private void Start()
    {
        _bulletSpawn = this.transform.FindChild("BulletSpawn").gameObject;
    }

    void Update()
    {
        if (Time.time > coolDown + fireInteval) {
            coolDown = Time.time;
            Fire();
        }
    }

    private void Fire()
    {
        Bullet b = Instantiate(_bulletFab, _bulletSpawn.transform.position, Quaternion.identity) as Bullet;
        b.Direction = this.Direction;
    }
}
