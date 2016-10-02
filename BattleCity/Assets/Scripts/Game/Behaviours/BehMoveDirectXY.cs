/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:09
*/

using System;
using UnityEngine;

public class BehMoveDirectXY: BehMove 
{
    //public static readonly Uid64 UNIQ = "BE3E7B010E036A02";
    private MapObject _lastCollision;

    public override void ChangeDirection(MapObject mo, Dir4 newDirection)
    {
        if (newDirection != mo.Direction) {
            mo.Direction = newDirection;
            _lastCollision = null;
        }
    }

    public override ObstacleType Move(Transform trans, MapObject mo)
    {
        Vector3 curPos = trans.position;
        Vector3 direction = MapObjectUtils.GetDirection2D(mo.Direction);
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
            canMove = BTGame.Current.Stage.InMapBound(mo.Rect, mo.Direction);
            if (!canMove) { obst = ObstacleType.GroundAndFlying; }
        }
        if (canMove) {
            trans.position = nextPos;
        }

        return obst;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        _lastCollision = collider.GetComponent<MapObject>();
    }
}
