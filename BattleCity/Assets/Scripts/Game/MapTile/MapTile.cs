/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 30/09/2016 23:42
*/

using System;
using UnityEngine;

[Serializable]
public abstract class MapObject: MonoBehaviour
{
    //public static readonly Uid64 UNIQ = "CE15B27912F4F002";
    [SerializeField]
    private int _mapObjectID;
    public int MapObjectID{
        get { return _mapObjectID; }
    }

    [SerializeField]
    private Vector2 _mapSize;
    public Vector2 MapSize {
        get { return _mapSize; }
    }

    [SerializeField]
    protected ObstacleType _obstacle;
    public ObstacleType Obstacle
    {
        get { return _obstacle; }
    }

    public Rect Rect
    {
        get {
            float xOff = this.transform.position.x - (int)MapSize.x / 2;
            float yOff = this.transform.position.y - (int)MapSize.y / 2;

            return new Rect(xOff, yOff, MapSize.x, MapSize.y);
        }
    }

    public abstract Dir4 Direction { get; set; }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Rect.center, Rect.size);
    }
}
