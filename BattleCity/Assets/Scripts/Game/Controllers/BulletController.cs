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
    }

    [SerializeField]
    private float _damageRadius = 0.2f;
    public float DamageRadius
    {
        get { return _damageRadius; }
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
    private GameObject _owner;

    void Start()
    {
        _mapObj = this.GetComponent<MapObject>();
        _moveBehaviour = this.GetComponent<BehMove>();
        IDamageable dam = this.GetComponent<IDamageable>();
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
                if (cols[i].gameObject == _owner) continue;
                IDamageable dam = cols[i].GetComponent<IDamageable>();
                if (dam != null) {
                    dam.SetDamage(_damage);
                }
            }
            OnDestroy();
        }
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    private void NoHealthEventHandler(object sender, EventArgs arg)
    {
        IDamageable dam = this.GetComponent<IDamageable>();
        if (dam != null) {
            dam.NoHealthEvent -= NoHealthEventHandler;
            BTGame.Current.Stage.UnregisterMapObject(_mapObj);
        }
        if (this.gameObject != null) {
            Destroy(this.gameObject);
        }
    }
}
