/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 23:09
*/

using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //public static readonly Uid64 UNIQ = "DE3E8932773DD602";

    [SerializeField]
    private Damage _damage;
    public Damage Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    [SerializeField]
    private float _damageRadius = 0.2f;
    public float DamageRadius
    {
        get { return _damageRadius; }
        set { _damageRadius = value; }
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
    [SerializeField]
    private GameObject _owner;

    void Start()
    {
        _mapObj = this.GetComponent<MapObject>();
        _moveBehaviour = this.GetComponent<BehMove>();
        this.transform.SetParent(BTGame.Current.Stage.transform);
        BehDamage dam = this.GetComponent<BehDamage>();
        if (dam != null) { dam.NoHealthEvent += NoHealthEventHandler; }
    }

    void Update()
    {
        if (_moveBehaviour != null) {
            ObstacleType obst = _moveBehaviour.Move(this.transform, this.MapObj);
            if (obst != ObstacleType.None) {
                SetDamage();
            }
        }
    }

    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }

    private void SetDamage()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, DamageRadius);
        if (cols != null) {
            for (int i = 0; i < cols.Length; i++) {
                if (cols[i].gameObject == _owner || cols[i].gameObject == this.gameObject) {
                    continue;
                }
                BehDamage dam = cols[i].GetComponent<BehDamage>();
                if (dam != null) {
                    dam.SetDamage(_damage);
                }
            }
            OnDestroy();
        }
    }

    private void OnDestroy()
    {
        _owner = null;
        BTGame.Current.BulletPool.AddItem(this);
    }

    private void NoHealthEventHandler(object sender, EventArgs arg)
    {
        OnDestroy();
    }
}
