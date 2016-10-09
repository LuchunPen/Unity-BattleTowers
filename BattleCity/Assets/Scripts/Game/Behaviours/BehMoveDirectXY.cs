/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:09
*/

using System;
using UnityEngine;

public class BehMoveDirectXY: BehMove, IModifiable<BehMove>
{
    //public static readonly Uid64 UNIQ = "BE3E7B010E036A02";
    private MapObject _lastCollision;
    protected Modificator<BehMove> _mod;

    public override void ChangeDirection(MapObject mo, Dir4 newDirection)
    {
        if (_mod != null && _mod.ModObject != null) {
            _mod.ModObject.ChangeDirection(mo, newDirection);
            return;
        }

        if (newDirection != mo.Direction) {
            mo.Direction = newDirection;
        }
        _lastCollision = null;
    }

    public override ObstacleType Move(Transform trans, MapObject mo)
    {
        if (_mod != null && _mod.ModObject != null) {
            return _mod.ModObject.Move(trans, mo);
        }

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

    public void SetModificator(Modificator<BehMove> modificator)
    {
        _mod = modificator;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        _lastCollision = collider.GetComponent<MapObject>();
    }
}
