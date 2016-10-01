/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 09:14
*/

using System;
using UnityEngine;

public class Bullet: MapObject
{
    //public static readonly Uid64 UNIQ = "EE3DC578BFF95402";

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

    public override Rect Rect
    {
        get {
            Vector2 pos = this.transform.position;
            return new Rect(pos.x - 0.5f, pos.y - 0.5f, (int)MapSize, (int)MapSize);
        }
    }
	
	void Update ()
	{
        //BulletFly();
	}

    /*
    private void BulletFly()
    {
        Vector3 curPos = this.transform.position;
        Vector3 direction = MapObjectUtils.GetDirection2D(Direction);
        Vector3 nextPos = Vector3.MoveTowards(curPos, curPos + direction, _moveSpeed * Time.deltaTime);

        if (_lastCollision != null) {
            if ((_lastCollision.Obstacle & ObstacleType.Flying) != 0) {
                BTGame.Current.Stage.UnregisterMapObject(_lastCollision);
                OnDestroy();
            }
            _lastCollision = null;
        }

        if (!BTGame.Current.Stage.InMapBound(this.Rect, this.Direction)) {
            OnDestroy();
        }
        this.transform.position = nextPos;
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _lastCollision = collider.GetComponent<MapObject>();
    }
    */
}
