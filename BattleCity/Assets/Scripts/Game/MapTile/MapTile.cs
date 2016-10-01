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

    public abstract Rect Rect { get; }

    public abstract Dir4 Direction { get; set; }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Rect.center, Rect.size);
    }
}
