/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08/10/2016 21:07
*/
using System;

public struct MapObjectSector
{
    public int MOType;
    public int TileDirection;

    public MapObjectSector(int moType, int tileDirection)
    {
        MOType = moType;
        TileDirection = tileDirection;
    }

    public override string ToString()
    {
        return MOType + ", " + TileDirection;
    }
}

public class SectorMap
{
    //public static readonly Uid64 UNIQ = "FE47A7035B1AE902";
    private int _sizeX;
    private int _sizeY;
    private string _mapName;

    private MapObjectSector[] _items;

    public int SizeX { get { return _sizeX; } }
    public int SizeY { get { return _sizeY; } }
    public string MapName { get { return _mapName; } }

    public MapObjectSector this[int x, int y] {
        get {
            int index = GetMapIndex(x, y);
            if (index == -1) { return default(MapObjectSector); }
            else { return _items[index]; }
        }
        set {
            int index = GetMapIndex(x, y);
            if (index == -1) { return; }
            else { _items[index] = value; }
        }
    }

    public SectorMap(int sizeX,int sizeY, string mapName)
	{
        if (sizeX < 1) { throw new ArgumentOutOfRangeException("Map size X is null"); }
        if (sizeY < 1) { throw new ArgumentOutOfRangeException("Map size Y is null"); }

        _sizeX = sizeX;
        _sizeY = sizeY;
        _mapName = mapName;

        int size = _sizeX * _sizeY;
        _items = new MapObjectSector[size];
    }

    public int GetMapIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) { return -1; }
        return x * _sizeY + y;
    }
}
