/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 30/09/2016 23:40
*/

using System;
using UnityEngine;

[Serializable]
public class StageMap
{
	//public static readonly Uid64 UNIQ = "EE15B2046349D002";
    [SerializeField] private int _sizeX;
    [SerializeField] private int _sizeY;
    [SerializeField] private MapCell[] _items;

    public int SizeX { get { return _sizeX; } }
    public int SizeY { get { return _sizeY; } }

    [SerializeField]
    private string _mapName;
    public string MapName {
        get { return _mapName; }
        set { _mapName = value; }
    }

    public MapCell this[int x ,int y]
    {
        get {
            int index = GetMapIndex(x, y);
            if (index == -1) { return null; }
            else { return _items[index]; }
        }
    }

	public StageMap(int sizeX, int sizeY)
	{
        if (sizeX < 1) { throw new ArgumentOutOfRangeException("Map size X is null"); }
        if (sizeY < 1) { throw new ArgumentOutOfRangeException("Map size Y is null"); }

        _sizeX = sizeX;
        _sizeY = sizeY;

        int size = _sizeX * _sizeY;
        _items = new MapCell[size];
        for (int i = 0; i < _items.Length; i++) {
            _items[i] = new MapCell();
        }
    }

    public int GetMapIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _sizeX || y >= _sizeY) { return -1; }
        return x * _sizeY + y;
    }

    public override string ToString()
    {
        return MapName + ": " + SizeX + ", " + SizeY;
    }
}
