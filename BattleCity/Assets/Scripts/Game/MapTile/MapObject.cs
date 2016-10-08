/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 30/09/2016 23:42
*/

using System;
using UnityEngine;

[Serializable]
public class MapObject: MonoBehaviour
{
    //public static readonly Uid64 UNIQ = "CE15B27912F4F002";
    [SerializeField]
    private int _mapObjectID;
    public int MapObjectID{
        get { return _mapObjectID; }
    }

    [SerializeField]
    protected MapSize _mapSize;
    public MapSize MapSize {
        get { return _mapSize; }
    }

    [SerializeField]
    protected ObstacleType _obstacle;
    public ObstacleType Obstacle
    {
        get { return _obstacle; }
    }

    public virtual Rect Rect
    {
        get {
            float xOff = this.transform.position.x - (float)MapSize / 2;
            float yOff = this.transform.position.y - (float)MapSize / 2;
            return new Rect(xOff, yOff, (int)MapSize, (int)MapSize);
        }
    }

    [SerializeField]
    private Dir4 _direction;
    public virtual Dir4 Direction
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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Rect.center, Rect.size);
    }
}
