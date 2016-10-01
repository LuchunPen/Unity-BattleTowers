/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:09
*/

using System;
using UnityEngine;

public class BehMoveDirect: BehMove 
{
    //public static readonly Uid64 UNIQ = "BE3E7B010E036A02";
    private MapObject _lastCollision;

    public override void ChangeDirection(Dir4 newDirection)
    {
        if (newDirection != MapObj.Direction) {
            MapObj.Direction = newDirection;
            _lastCollision = null;
        }
    }

    public override ObstacleType Move()
    {
        Vector3 curPos = _myTrans.position;
        Vector3 direction = MapObjectUtils.GetDirection2D(MapObj.Direction);
        Vector3 nextPos = Vector3.MoveTowards(curPos, curPos + direction, _moveSpeed * Time.deltaTime);

        bool canMove = true;
        ObstacleType obst = ObstacleType.None;

        if (_lastCollision != null) {
            obst = _lastCollision.Obstacle;
            if ((_moveObstacleMask & obst) != 0) {
                canMove = false;
            }
            else { obst = ObstacleType.None; }
        }
        if (canMove) {
            canMove = BTGame.Current.Stage.InMapBound(MapObj.Rect, MapObj.Direction);
            if (!canMove) { obst = ObstacleType.GroundAndFlying; }
        }
        if (canMove) {
            _myTrans.position = nextPos;
        }

        return obst;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        _lastCollision = collider.GetComponent<MapObject>();
    }
}
