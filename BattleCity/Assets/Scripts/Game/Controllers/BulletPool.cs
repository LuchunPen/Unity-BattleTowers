/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 10:45
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    //public static readonly Uid64 UNIQ = "FE4866BA4E299D02";
    private GameObject _poolRoot;
    private Stack<BulletController> _pool;

    public BulletPool()
	{
        _pool = new Stack<BulletController>();
        _poolRoot = GameObject.Find("BulletPool");

        if (_poolRoot == null) {
            _poolRoot = new GameObject();
            _poolRoot.name = "BulletPool";
        }
    }

    public BulletController GetItem(BulletController template)
    {
        if (template == null) { return null; }

        BulletController bt = null;
        if (_pool.Count > 0) {
            bt = _pool.Pop();
            if (bt != null) {
                bt.Damage = template.Damage;
                bt.DamageRadius = template.DamageRadius;
                bt.GetComponent<SpriteRenderer>().sprite = template.GetComponent<SpriteRenderer>().sprite;
                return bt;
            }
        }
        
        bt = GameObject.Instantiate(template);
        return bt;
    }

    public void AddItem(BulletController item)
    {
        if (item == null) { return; }
        item.gameObject.SetActive(false);
        if (_pool.Contains(item)) { return; }

        item.transform.SetParent(_poolRoot.transform);
        item.GetComponent<BehMove>().ChangeDirection(item.MapObj, item.MapObj.Direction);
        _pool.Push(item);
    }

    public void Clear()
    {

    }
}
