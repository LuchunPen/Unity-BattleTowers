/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 30/09/2016 23:37
*/

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stage: MonoBehaviour
{
    //public static readonly Uid64 UNIQ = "EE15B130F388F302";

    private static Stage _instance;
    public static Stage Instance { get { return _instance; } }

    private int _nextMapObjectID = 1;

    [SerializeField]
    private StageMap _map;
    public StageMap Map
    {
        get { return _map; }
        set {
            if (_map != null) {
                _map = value;
                ChangeFloorSize();
            }
        }
    }

    [SerializeField]
    private Transform FloorObject;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
        }
        else { Destroy(this); }
    }

	void Start () 
	{
	    
	}
	
	void Update ()
	{
	    
	}

    private void ChangeFloorSize()
    {
        if (_map == null) { return; }
        if (FloorObject == null) { FloorObject = this.transform.FindChild("Floor"); }
        FloorObject.localScale = new Vector3(_map.SizeX, _map.SizeY);
    }

    public bool IsMapBounded(Vector3 position)
    {
        if (_map == null) { return false; }
        int px = Mathf.FloorToInt(position.x);
        int py = Mathf.FloorToInt(position.y);

        return _map.GetMapIndex(px, py) != -1;
    }

    public bool IsMapBounded(int px, int py)
    {
        return _map.GetMapIndex(px, py) != -1;
    }

    public bool CanPlaceMapObject(MapObject mObj)
    {
        if (mObj == null) { return false; }
        Rect r = mObj.Rect;
        int sx = Mathf.FloorToInt(r.xMin);
        int sy = Mathf.FloorToInt(r.yMin);
        int ex = (int)(sx + r.width);
        int ey = (int)(sy + r.height);

        for (int x = sx; x < ex; x++) {
            for (int y = sy; y < ey; y++) {
                if (!IsMapBounded(x, y)) { return false; }
                if (_map[x, y].RegisterObject != null) {
                    return false;
                }
            }
        }
        
        for (int x = sx; x < ex; x++) {
            for (int y = sy; y < ey; y++) {
                _map[x, y].RegisterObject = mObj;
            }
        }
        return true;
    }

    public void UnregisterMapObject(MapObject mObj)
    {
        if (mObj == null) { return; }
        Rect r = mObj.Rect;
        for (int x = (int)r.xMin; x <= r.xMax; x++) {
            for (int y = (int)r.yMin; y <= r.yMax; y++) {
                MapCell mc = Map[x, y];
                if (mc != null && mc.RegisterObject == mObj) {
                    mc.RegisterObject = null;
                }
            }
        }

        if (!Application.isPlaying) {
            DestroyImmediate(mObj.gameObject);
        }
        else { Destroy(mObj.gameObject); }
    }

    public MapObject[] MapRectCollisionDetect(Rect r, Dir4 direction, ObstacleType collisionMask)
    {
        int minX, minY, maxX, maxY;
        if (direction == Dir4.Left) {
            minX = Mathf.FloorToInt(r.xMin);
            maxX = Mathf.FloorToInt(r.xMax);
            minY = Mathf.CeilToInt(r.yMin);
            maxY = Mathf.FloorToInt(r.yMax);
            if (minX == maxX) { maxX++; }
        }
        else if (direction == Dir4.Right) {
            minX = Mathf.CeilToInt(r.xMin);
            maxX = Mathf.CeilToInt(r.xMax);
            minY = Mathf.CeilToInt(r.yMin);
            maxY = Mathf.FloorToInt(r.yMax);
            if (minX == maxX) { minX--; }
        }

        else if (direction == Dir4.Up) {
            minX = Mathf.CeilToInt(r.xMin);
            maxX = Mathf.FloorToInt(r.xMax);
            minY = Mathf.CeilToInt(r.yMin);
            maxY = Mathf.CeilToInt(r.yMax);
            if (minY == maxY) { minY--; }
        }
        else {
            minX = Mathf.CeilToInt(r.xMin);
            maxX = Mathf.FloorToInt(r.xMax);
            minY = Mathf.FloorToInt(r.yMin);
            maxY = Mathf.FloorToInt(r.yMax);
            if (minY == maxY) { maxY++; }
        }

        for (int x = minX; x < maxX; x++) {
            for (int y = minY; y < maxY; y++) {
                if (!IsMapBounded(x, y)) {
                    return new MapObject[0];
                }
            }
        }

        //Other map object collision
        if (direction == Dir4.Up) {
            r.xMin += 0.2f; r.yMin += 0.5f; r.xMax -= 0.2f; r.yMax -= 0.1f;
        }
        if (direction == Dir4.Down) {
            r.xMin += 0.2f; r.yMin += 0.1f; r.xMax -= 0.2f; r.yMax -= 0.5f;
        }
        if (direction == Dir4.Left) {
            r.xMin += 0.1f; r.yMin += 0.2f; r.xMax -= 0.5f; r.yMax -= 0.2f;
        }
        if (direction == Dir4.Right) {
            r.xMin += 0.5f; r.yMin += 0.2f; r.xMax -= 0.1f; r.yMax -= 0.2f;
        }

        List<MapObject> mObjs = new List<MapObject>();

        for (int x = minX - 1; x <= maxX; x++) {
            for (int y = minY - 1; y <= maxY; y++) {

                MapCell mc = Map[x, y];
                if (mc != null) {
                    MapObject mo = mc.RegisterObject;
                    if (mo != null && (mo.Obstacle & collisionMask) != 0) {
                        if (mo.Rect.Overlaps(r)) {
                            mObjs.Add(mo);
                        }
                    }
                }
            }
        }

        if (mObjs.Count == 0) { return null; }
        return mObjs.ToArray();
    }

    public void ClearMap()
    {
        for (int x = 0; x < _map.SizeX; x++) {
            for (int y = 0; y < _map.SizeY; y++) {
                MapObject mo = _map[x, y].RegisterObject;
                if (mo == null) { continue; }
                _map[x, y].RegisterObject = null;
                if (Application.isPlaying) {
                    Destroy(mo.gameObject);
                }
                else {
                    DestroyImmediate(mo.gameObject);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        for (int x = 0; x < _map.SizeX; x++) {
            for (int y = 0; y < _map.SizeY; y++) {
                if (_map[x,y].RegisterObject != null) {
                    Gizmos.DrawCube(new Vector3(x + 0.5f, y + 0.5f, 0), new Vector3(0.3f, 0.3f, 0.3f));
                }
            }
        }
    }
}
