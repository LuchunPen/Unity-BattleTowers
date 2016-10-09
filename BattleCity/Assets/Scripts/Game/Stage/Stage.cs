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

    public event EventHandler WinEvent;
    public event EventHandler LoseEvent;

    private List<MapObject> _enemies;
    private PlayerSpawnerController _player;

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

    private void Start()
    {
        InitializeEnemies();
        InitializePlayer();
    }

    // HARDCODE ================================
    protected virtual void InitializeEnemies()
    {
        _enemies = new List<MapObject>();
        for (int x = 0; x < Map.SizeX; x += 2) {
            for (int y = 0; y < Map.SizeY; y += 2) {
                MapObject mo = Map[x, y].RegisterObject;

                if (mo == null) { continue; }
                if (mo.MOType == MapObjectRegisterType.TowerBig || mo.MOType == MapObjectRegisterType.TowerMini) {
                    _enemies.Add(mo);

                }
            }
        }
    }
    protected virtual void InitializePlayer()
    {
        for (int x = 0; x < Map.SizeX; x += 2) {
            for (int y = 0; y < Map.SizeY; y += 2) {
                MapObject mo = Map[x, y].RegisterObject;
                if (mo == null) { continue; }
                else if (mo.MOType == MapObjectRegisterType.PlayerSpawn) {
                    PlayerSpawnerController ps = mo.GetComponent<PlayerSpawnerController>();
                    if (ps != null) {
                        ps.PlayerDestoyEvent += PlayerDestroyEventHandler;
                        _player = ps;
                        return;
                    }
                }
            }
        }
    }
    // HARDCODE ================================

    private void ChangeFloorSize()
    {
        if (_map == null) { return; }
        if (FloorObject == null) {
            FloorObject = this.transform.FindChild("Floor");

        }
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

    public bool RegisterMapObject(MapObject mObj)
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

    public virtual void UnregisterMapObject(MapObject mObj)
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
        else {
            UnregisterEnemy(mObj);
            Destroy(mObj.gameObject);
        }
    }

    private void UnregisterEnemy(MapObject mObj)
    {
        bool en = _enemies.Remove(mObj);
        if (en) {
            if (_enemies.Count > 0) {
                CreateBonus();
            }
            else {
                WinTrigger();
            }
        }
    }

    private void CreateBonus()
    {
        int xRnd = UnityEngine.Random.Range(0, Map.SizeX / 2);
        int yRnd = UnityEngine.Random.Range(0, Map.SizeY / 2);

        BonusController bc = BTGame.Current.GetRandomBonus();
        if(bc != null) {
            BonusController bonus = Instantiate(bc, new Vector3(xRnd * 2 + 1, yRnd * 2 + 1, 0), Quaternion.identity) as BonusController;
            bonus.transform.SetParent(this.transform);
        }
    }

    public bool InMapBound(Rect r, Dir4 direction)
    {
        int minX, minY, maxX, maxY;
        if (direction == Dir4.Left) {
            minX = Mathf.FloorToInt(r.xMin);
            maxX = Mathf.FloorToInt(r.xMax);
            minY = Mathf.CeilToInt(r.yMin);
            maxY = Mathf.FloorToInt(r.yMax);
            if (minX == maxX) { maxX++; }
            if (minY == maxY) { maxY++; }
        }
        else if (direction == Dir4.Right) {
            minX = Mathf.CeilToInt(r.xMin);
            maxX = Mathf.CeilToInt(r.xMax);
            minY = Mathf.CeilToInt(r.yMin);
            maxY = Mathf.FloorToInt(r.yMax);
            if (minX == maxX) { minX--; }
            if (minY == maxY) { maxY++; }
        }

        else if (direction == Dir4.Up) {
            minX = Mathf.CeilToInt(r.xMin);
            maxX = Mathf.FloorToInt(r.xMax);
            minY = Mathf.CeilToInt(r.yMin);
            maxY = Mathf.CeilToInt(r.yMax);
            if (minY == maxY) { minY--; }
            if (minX == maxX) { maxX++; }
        }
        else {
            minX = Mathf.CeilToInt(r.xMin);
            maxX = Mathf.FloorToInt(r.xMax);
            minY = Mathf.FloorToInt(r.yMin);
            maxY = Mathf.FloorToInt(r.yMax);
            if (minY == maxY) { maxY++; }
            if (minX == maxX) { maxX++; }
        }

        for (int x = minX; x < maxX; x++) {
            for (int y = minY; y < maxY; y++) {
                if (!IsMapBounded(x, y)) {
                    return false;
                }
            }
        }

        return true;
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

    private void WinTrigger()
    {
        EventHandler ev = WinEvent;
        if (ev != null) { ev(this, null); }
    }
    private void PlayerDestroyEventHandler(object sender, EventArgs args)
    {
        if (_player != null) {
            _player.PlayerDestoyEvent -= PlayerDestroyEventHandler;
        }
        EventHandler ev = LoseEvent;
        if (ev != null) { ev(this, null); }
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
