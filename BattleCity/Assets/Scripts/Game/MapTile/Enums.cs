/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 04:10
*/
using System;

public enum Dir4
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3,
}

[Flags]
public enum ObstacleType
{
    None = 0,
    Ground = 1,
    Flying = 2,
    GroundAndFlying = Ground | Flying,
}

public enum  MapObjectRegisterType
{
    None = 0,
    PlayerSpawn = 1,
    BrickWall = 2,
    ConcreteWall = 3,
    WaterWall = 4,
    TowerMini = 5,
    TowerBig = 6,
}

public enum MapSize
{
    x1 = 1,
    x2 = 2,
}
