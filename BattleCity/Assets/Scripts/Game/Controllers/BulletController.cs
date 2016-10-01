/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 23:09
*/

using System;
using UnityEngine;

public class BulletController: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "DE3E8932773DD602";

    [SerializeField]
    private Damage _damage;
    public Damage Damage
    {
       get { return _damage; }
    }

    private MapObject _mapObj;
    public MapObject MapObj
    {
        get {
            if (_mapObj == null) {
                _mapObj = this.GetComponent<MapObject>();
            }
            return _mapObj;
        }
    }

    private BehMove _moveBehaviour;
    private MapObject _owner;

    void Awake()
    {
        _mapObj = this.GetComponent<MapObject>();
        _moveBehaviour = this.GetComponent<BehMove>();
    }

	void Update ()
	{
        if (_moveBehaviour != null) {
            ObstacleType obst =  _moveBehaviour.Move();
            if (obst != ObstacleType.None) {
                SetDamage();
            }
        }
    }

    public void SetOwner(MapObject mapObj)
    {
        _owner = mapObj;
    }

    private void SetDamage()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, 0.2f);
        if (cols != null) {
            for (int i = 0; i < cols.Length; i++) {
                MapObject mo = cols[i].GetComponent<MapObject>();
                if (mo != null && mo != _owner) {
                    BTGame.Current.Stage.UnregisterMapObject(mo);
                }
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
